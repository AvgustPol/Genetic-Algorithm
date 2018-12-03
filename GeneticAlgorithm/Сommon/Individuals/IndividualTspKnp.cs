using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Сommon.Individuals
{
    public class IndividualTspKnp : Individual
    {
        /// <summary>
        /// TSP problem
        /// Places sequence is a road
        /// </summary>
        public int[] Places { get; set; }

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

        public IndividualTspKnp()
        {
        }

        public IndividualTspKnp(int[] road)
        {
            Places = road;
            CreateItems();
            CountFitness();
        }

        public void CreateItems()
        {
            List<AcceptableItem> acceptableItems = new List<AcceptableItem>();

            for (int i = 0; i < AlgorithmCoreParameters.NumberOfItems; i++)
            {
                Item tmpItem = AlgorithmCoreParameters.GetItem(i);
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
            int[] idPlaceValueItemToTake = new int[AlgorithmCoreParameters.Dimension];
            for (int i = 0; i < AlgorithmCoreParameters.Dimension; i++)
            {
                idPlaceValueItemToTake[i] = Permutator.NotFoundCode;
            }

            for (int i = 0; i < Items.Length; i++)
            {
                //если я беру предмет
                if (Items[i])
                {
                    Item item = AlgorithmCoreParameters.GetItem(i);
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
            double itemFitness = item.Profit - AlgorithmCoreParameters.RentingRatio * timeDifference;

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
            return road / AlgorithmCoreParameters.MaxSpeed;
        }

        private double CountTimeWithItem(Item item, int lastPlaceId, double road)
        {
            double speedWithItem = CountSpeedWithItem(item, item.PlaceId, lastPlaceId);
            return road / speedWithItem;
        }

        private void CountWhichItemsToTake(List<AcceptableItem> acceptableItems)
        {
            Items = new bool[AlgorithmCoreParameters.NumberOfItems];

            int capacityOfKnapsack = 0;

            // while
            // we have space in the knapsack (capacityOfKnapsack == GeneticAlgorithmParameters.MaxCapacityOfKnapsack)
            // and
            // there are acceptable items (acceptableItems.Count > 0)
            for (int i = acceptableItems.Count - 1; i >= 0; i--)
            {
                AcceptableItem acceptableItem = acceptableItems[i];
                int itemId = acceptableItem.Id;
                int itemWeight = AlgorithmCoreParameters.GetItem(itemId).Weight;
                if (capacityOfKnapsack + itemWeight <= AlgorithmCoreParameters.MaxCapacityOfKnapsack)
                {
                    capacityOfKnapsack += itemWeight;
                    Items[itemId] = true;
                }
            }
        }

        private double CountFullRoad()
        {
            //from start place (0) to end (AlgorithmCoreParameters.Dimension - 1)
            return CountRoad(0, AlgorithmCoreParameters.Dimension - 1);
        }

        private double CountRoad(int start, int end)
        {
            double road = 0;
            for (int i = start; i < end; i++)
            {
                road += AlgorithmCoreParameters.GetDistance(Places[i], Places[i + 1]);
            }

            road += AlgorithmCoreParameters.GetDistance(Places[end], Places[0]);
            return road;
        }

        private double CountSpeedWithItem(Item item, int start, int end)
        {
            double minSpeed = AlgorithmCoreParameters.MinSpeed;
            double maxSpeed = AlgorithmCoreParameters.MaxSpeed;

            double currentSpeed = maxSpeed - AlgorithmCoreParameters.MaxMinusMinDividedByWeight * item.Weight;

            //TODO: useless ? can current speed be lower than min speed?
            return Math.Max(minSpeed, currentSpeed);
        }

        private double CountSpeedWithWeight(int weight, int start, int end)
        {
            double minSpeed = AlgorithmCoreParameters.MinSpeed;
            double maxSpeed = AlgorithmCoreParameters.MaxSpeed;

            double currentSpeed = maxSpeed - AlgorithmCoreParameters.MaxMinusMinDividedByWeight * weight;

            //TODO: useless ? can current speed be lower than min speed?
            return Math.Max(minSpeed, currentSpeed);
        }

        public override object Clone()
        {
            IndividualTspKnp clone = new IndividualTspKnp();
            if (Places != null)
            {
                clone.Places = (int[])Places.Clone();
            }

            if (Items != null)
            {
                clone.Items = (bool[])Items.Clone();
            }
            //TODO : точно ли тебе нужно клонировать все таблицы?

            //if (ItemsLocation != null)
            //{
            //    clone.ItemsLocation = (int[])ItemsLocation.Clone();
            //}

            clone.Fitness = Fitness;

            return clone;
        }

        public override void CountFitness()
        {
            // Fitness = Sum(allItemsValue) - R * ti
            //
            // ti - czas z przedmiotami
            Fitness = 0;

            double sumProfit = CountSumProfit();
            double ti = CountTime();

            Fitness = sumProfit - AlgorithmCoreParameters.RentingRatio * ti;
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
                distance = AlgorithmCoreParameters.GetDistance(Places[i], Places[i + 1]);
                int itemId = ItemsLocation[i];
                if (itemId != Permutator.NotFoundCode)
                {
                    item = AlgorithmCoreParameters.GetItem(itemId);
                    knapsack += item.Weight;
                    speed = CountSpeedWithWeight(knapsack, i, i + 1);
                }

                speed = Math.Max(AlgorithmCoreParameters.MinSpeed, speed);
                time += distance / speed;
            }

            #endregion Visit all places

            #region Returning to first place

            distance = AlgorithmCoreParameters.GetDistance(Places[0], Places[Places.Length - 1]);
            int lastPlaceItem = ItemsLocation[Places.Length - 1];
            if (lastPlaceItem != Permutator.NotFoundCode)
            {
                item = AlgorithmCoreParameters.GetItem(lastPlaceItem);
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
            for (int i = 0; i < AlgorithmCoreParameters.NumberOfItems; i++)
            {
                if (Items[i])
                {
                    sum += AlgorithmCoreParameters.GetItem(i).Profit;
                }
            }

            return sum;
        }

        public override bool Mutate()
        {
            bool didMutate = false;
            int randomNumber = Randomizer.Random.Next(GeneticAlgorithmParameters.MaxProbability);
            if (GeneticAlgorithm.GeneticAlgorithmParameters.MutationProbability > randomNumber)
            {
                int randomIndex1 = Randomizer.Random.Next(AlgorithmCoreParameters.Dimension);
                int randomIndex2 = Randomizer.Random.Next(AlgorithmCoreParameters.Dimension);
                while (randomIndex1 == randomIndex2)
                {
                    randomIndex2 = Randomizer.Random.Next(AlgorithmCoreParameters.Dimension);
                }
                //MUTATE
                Permutator.Swap(Places, randomIndex1, randomIndex2);
                CountFitness();
                didMutate = true;
            }

            return didMutate;
        }

        public override int[] GetMutation()
        {
            int[] mutation = new int[AlgorithmCoreParameters.Dimension];
            Array.Copy(Places, mutation, AlgorithmCoreParameters.Dimension);

            int randomIndex1 = Randomizer.Random.Next(AlgorithmCoreParameters.Dimension);
            int randomIndex2 = Randomizer.Random.Next(AlgorithmCoreParameters.Dimension);
            while (randomIndex1 == randomIndex2)
            {
                randomIndex2 = Randomizer.Random.Next(AlgorithmCoreParameters.Dimension);
            }
            //MUTATE
            Permutator.Swap(mutation, randomIndex1, randomIndex2);

            return mutation;
        }

        public override List<int[]> GetNeighbors(int numberOfNeighbors)
        {
            List<int[]> neighbors = new List<int[]>(numberOfNeighbors);

            for (int i = 0; i < numberOfNeighbors; i++)
            {
                neighbors.Add(GetMutation());
            }

            return neighbors;
        }

        public static int[] CreateRandomIndividual()
        {
            #region Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            int[] defaultArray = new int[AlgorithmCoreParameters.Dimension];
            for (int i = 0; i < AlgorithmCoreParameters.Dimension; i++)
            {
                defaultArray[i] = i;
            }

            #endregion Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            return Permutator.GetRandomPermutation(defaultArray);
        }
    }
}