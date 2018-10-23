using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Individual : ICloneable
    {
        public double Fitness { get; set; }

        /// <summary>
        /// KNP problem
        /// index: item id
        /// value: if true then thief takes item
        /// </summary
        public bool[] Items { get; set; }

        /// <summary>
        /// TSP problem
        /// Places sequence is a road
        /// </summary>
        public int[] Places { get; set; }

        public Individual()
        {
        }

        public Individual(int[] road)
        {
            Places = road;
            CreateItems();
            CountFitness();
        }

        private void CreateItems()
        {
            List<AcceptableItem> acceptableItems = new List<AcceptableItem>();

            for (int i = 0; i < GeneticAlgorithmParameters.NumberOfItems; i++)
            {
                Item tmpItem = GeneticAlgorithmParameters.GetItem(i);
                double itemFitnessForCurrentRoad = CountItemFitness(tmpItem);
                if (itemFitnessForCurrentRoad > 0)
                {
                    acceptableItems.Add(new AcceptableItem()
                    {
                        Id = i,
                        FitnessForCurrentRoad = itemFitnessForCurrentRoad
                    });
                }
            }

            acceptableItems.Sort((item1, item2) => item1.FitnessForCurrentRoad.CompareTo(item2.FitnessForCurrentRoad));

            CountWhichItemsToTake(acceptableItems);
        }

        private class AcceptableItem
        {
            public int Id { get; set; }
            public double FitnessForCurrentRoad { get; set; }
        }

        private double CountItemFitness(Item item)
        {
            double timeDifference = CountTimeDifference(item);
            double itemFitness = item.Profit - GeneticAlgorithmParameters.RentingRatio * timeDifference;

            return itemFitness;
        }

        private double CountTimeDifference(Item item)
        {
            int lastPlaceId = Places[Places.Length - 1];
            double road = CountRoad(item.PlaceId, lastPlaceId);

            double timeWithItem = CountTimeWithItem(item, lastPlaceId, road);
            double timeWithEmptyKnapsack = CountTimeWithEmptyKnapsack(road);

            return timeWithItem - timeWithEmptyKnapsack;
        }

        private double CountTimeWithEmptyKnapsack(double road)
        {
            return road / GeneticAlgorithmParameters.MaxSpeed;
        }

        private double CountTimeWithItem(Item item, int lastPlaceId, double road)
        {
            double speedWithItem = CountSpeedWithItem(item, item.PlaceId, lastPlaceId);
            return road / speedWithItem;
        }

        private void CountWhichItemsToTake(List<AcceptableItem> acceptableItems)
        {
            int capacityOfKnapsack = 0;

            // while
            // we have space in the knapsack (capacityOfKnapsack == GeneticAlgorithmParameters.MaxCapacityOfKnapsack)
            // and
            // there are acceptable items (acceptableItems.Count > 0)
            while (acceptableItems.Count > 0 && capacityOfKnapsack == GeneticAlgorithmParameters.MaxCapacityOfKnapsack)
            {
                AcceptableItem acceptableItem = acceptableItems.First();
                int itemId = acceptableItem.Id;
                int itemWeight = GeneticAlgorithmParameters.GetItem(itemId).Weight;
                if (capacityOfKnapsack + itemWeight <= GeneticAlgorithmParameters.MaxCapacityOfKnapsack)
                {
                    capacityOfKnapsack += itemWeight;
                    Items[itemId] = true;
                }

                acceptableItems.Remove(acceptableItem);
            }
        }

        private double CountRoad(int start, int end)
        {
            double road = 0;
            for (int i = start; i < end; i++)
            {
                road += GeneticAlgorithmParameters.GetDistance(i, i + 1);
            }

            road += GeneticAlgorithmParameters.GetDistance(end, 0);
            return road;
        }

        private double CountSpeedWithItem(Item item, int start, int end)
        {
            double minSpeed = GeneticAlgorithmParameters.MinSpeed;
            double maxSpeed = GeneticAlgorithmParameters.MaxSpeed;

            double currentSpeed = maxSpeed - GeneticAlgorithmParameters.MaxMinusMinDividedByWeight * item.Weight;

            //TODO: useless ? can current speed be lower than min speed?
            return Math.Max(minSpeed, currentSpeed);
        }

        public object Clone()
        {
            Individual clone = new Individual();
            if (Places != null)
            {
                clone.Places = (int[])Places.Clone();
            }

            if (ShouldTakeItems != null)
            {
                clone.ShouldTakeItems = (bool[])ShouldTakeItems.Clone();
            }

            return clone;
        }

        public void CountFitness()
        {
            // Fitness = Sum(itemValue) - R * ( ti - t0)
            //
            // ti - czas z przedmiotami
            // t0 - czas z pustym plecakiem
            Fitness = 0;

            double sumProfit = CountSumProfit();
            double ti = 0;
            double t0 = 0;

            //for (int i = 0; i < GeneticAlgorithmParameters.Dimension - 1; i++)
            //{
            //    Fitness += GeneticAlgorithmParameters.GetDistance(Places[i], Places[i + 1]);
            //}

            //Fitness += GeneticAlgorithmParameters.GetDistance(Places[0], Places[GeneticAlgorithmParameters.Dimension - 1]);

            //Fitness = 1 / Fitness;
        }

        private double CountSumProfit()
        {
            double sum = 0;
            for (int i = 0; i < GeneticAlgorithmParameters.NumberOfItems; i++)
            {
                if (Items[i])
                {
                    sum += GeneticAlgorithmParameters.GetItem(i).Profit;
                }
            }

            return sum;
        }

        public void Mutate()
        {
            int randomNumber = Randomizer.Random.Next(GeneticAlgorithmParameters.MaxProbability);
            if (GeneticAlgorithmParameters.MutationProbability > randomNumber)
            {
                int randomIndex1 = Randomizer.Random.Next(GeneticAlgorithmParameters.Dimension);
                int randomIndex2 = Randomizer.Random.Next(GeneticAlgorithmParameters.Dimension);
                while (randomIndex1 == randomIndex2)
                {
                    randomIndex2 = Randomizer.Random.Next(GeneticAlgorithmParameters.Dimension);
                }
                //MUTATE
                Permutator.Swap(Places, randomIndex1, randomIndex2);
            }
        }

        public int[] GetMutation()
        {
            int[] mutation = new int[GeneticAlgorithmParameters.Dimension];
            Array.Copy(Places, mutation, GeneticAlgorithmParameters.Dimension);

            int randomIndex1 = Randomizer.Random.Next(GeneticAlgorithmParameters.Dimension);
            int randomIndex2 = Randomizer.Random.Next(GeneticAlgorithmParameters.Dimension);
            while (randomIndex1 == randomIndex2)
            {
                randomIndex2 = Randomizer.Random.Next(GeneticAlgorithmParameters.Dimension);
            }
            //MUTATE
            Permutator.Swap(mutation, randomIndex1, randomIndex2);

            return mutation;
        }
    }
}