using DataModel;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Individual : ICloneable
    {
        /// <summary>
        /// TSP problem
        /// Places sequence is a road
        /// </summary>
        public int[] Places { get; set; }

        public double Fitness { get; set; }

        /// <summary>
        /// id - placeId
        /// value - itemId (int or NotFoundCode)
        /// </summary>
        public int[] ItemsLocation { get; set; }

        /// <summary>
        /// KNP problem
        /// index: item id
        /// value: if true then thief takes item
        /// </summary
        public bool[] Items { get; set; }

        public Individual()
        {
        }

        public Individual(int[] road)
        {
            Places = road;
            CreateItems();
            CountFitness();
        }

        public void CreateItems()
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

            //sort from smallest fitness to biggest
            acceptableItems.Sort((item1, item2) => item1.FitnessForCurrentRoad.CompareTo(item2.FitnessForCurrentRoad));

            CountWhichItemsToTake(acceptableItems);
            CreatePlacesP();
        }

        private void CreatePlacesP()
        {
            //id - id города
            //value - id предмета, который нужно взять
            int[] idPlaceValueItemToTake = new int[GeneticAlgorithmParameters.Dimension];
            for (int i = 0; i < GeneticAlgorithmParameters.Dimension; i++)
            {
                idPlaceValueItemToTake[i] = Permutator.NotFoundCode;
            }

            for (int i = 0; i < Items.Length; i++)
            {
                //если я беру предмет
                if (Items[i])
                {
                    Item item = GeneticAlgorithmParameters.GetItem(i);
                    idPlaceValueItemToTake[item.PlaceId] = item.Id;
                }
            }

            ItemsLocation = idPlaceValueItemToTake;
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
            Items = new bool[GeneticAlgorithmParameters.NumberOfItems];

            int capacityOfKnapsack = 0;

            // while
            // we have space in the knapsack (capacityOfKnapsack == GeneticAlgorithmParameters.MaxCapacityOfKnapsack)
            // and
            // there are acceptable items (acceptableItems.Count > 0)
            for (int i = acceptableItems.Count - 1; i >= 0; i--)
            {
                AcceptableItem acceptableItem = acceptableItems[i];
                int itemId = acceptableItem.Id;
                int itemWeight = GeneticAlgorithmParameters.GetItem(itemId).Weight;
                if (capacityOfKnapsack + itemWeight <= GeneticAlgorithmParameters.MaxCapacityOfKnapsack)
                {
                    capacityOfKnapsack += itemWeight;
                    Items[itemId] = true;
                }
            }
        }

        private double CountFullRoad()
        {
            //from start place (0) to end (GeneticAlgorithmParameters.Dimension - 1)
            return CountRoad(0, GeneticAlgorithmParameters.Dimension - 1);
        }

        private double CountRoad(int start, int end)
        {
            double road = 0;
            for (int i = start; i < end; i++)
            {
                road += GeneticAlgorithmParameters.GetDistance(Places[i], Places[i + 1]);
            }

            road += GeneticAlgorithmParameters.GetDistance(Places[end], Places[0]);
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

        private double CountSpeedWithWeight(int weight, int start, int end)
        {
            double minSpeed = GeneticAlgorithmParameters.MinSpeed;
            double maxSpeed = GeneticAlgorithmParameters.MaxSpeed;

            double currentSpeed = maxSpeed - GeneticAlgorithmParameters.MaxMinusMinDividedByWeight * weight;

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

            if (Items != null)
            {
                clone.Items = (bool[])Items.Clone();
            }
            //TODO : точно ли тебе нужно клонировать все таблицы?

            if (ItemsLocation != null)
            {
                clone.ItemsLocation = (int[])ItemsLocation.Clone();
            }

            clone.Fitness = Fitness;

            return clone;
        }

        public void CountFitness()
        {
            // Fitness = Sum(allItemsValue) - R * ti
            //
            // ti - czas z przedmiotami
            Fitness = 0;

            double sumProfit = CountSumProfit();
            double ti = CountTime();

            Fitness = sumProfit - GeneticAlgorithmParameters.RentingRatio * ti;
        }

        /// <summary>
        /// Count time for all road
        /// просчитывает время всего пути с учетом того, что вор ворует какие-то предметы
        /// </summary>
        /// <returns></returns>
        private double CountTime()
        {
            Item item = null;
            double time = 0;
            double distance = 0;
            double speed = 0;

            int knapsack = 0;

            #region Visit all places

            for (int i = 0; i < Places.Length - 1; i++)
            {
                distance = GeneticAlgorithmParameters.GetDistance(Places[i], Places[i + 1]);
                int itemId = ItemsLocation[i];
                if (itemId != Permutator.NotFoundCode)
                {
                    item = GeneticAlgorithmParameters.GetItem(itemId);
                    knapsack += item.Weight;
                    speed = CountSpeedWithWeight(knapsack, i, i + 1);
                }

                speed = Math.Max(GeneticAlgorithmParameters.MinSpeed, speed);
                time += distance / speed;
            }

            #endregion Visit all places

            #region Returning to first place

            distance = GeneticAlgorithmParameters.GetDistance(Places[0], Places[Places.Length - 1]);
            int lastPlaceItem = ItemsLocation[Places.Length - 1];
            if (lastPlaceItem != Permutator.NotFoundCode)
            {
                item = GeneticAlgorithmParameters.GetItem(lastPlaceItem);
                knapsack += item.Weight;
                speed = CountSpeedWithWeight(knapsack, 0, Places.Length - 1);
            }

            time += distance / speed;

            #endregion Returning to first place

            return time;
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