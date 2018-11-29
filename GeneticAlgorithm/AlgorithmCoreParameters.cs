using GeneticAlgorithmLogic.Сommon;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic
{
    public class AlgorithmCoreParameters
    {
        static AlgorithmCoreParameters()
        {
            DataLoader dataLoader = new DataLoader();
            DataContainer container = dataLoader.GetCreatedDataContainerFromFile(GlobalParameters.PathToTestData);

            Dimension = container.Dimension;
            MinSpeed = container.MinSpeed;
            MaxSpeed = container.MaxSpeed;
            RentingRatio = container.RentingRatio;
            DistanceMatrix = container.DistanceMatrix;
            _items = container.Items;
            NumberOfItems = container.NumberOfItems;
            MaxCapacityOfKnapsack = container.MaxCapacityOfKnapsack;
            MaxMinusMinDividedByWeight = (MaxSpeed - MinSpeed) / MaxCapacityOfKnapsack;
        }

        /// <summary>
        /// number of places at TSP problem
        /// </summary>
        public static readonly int Dimension;

        public static readonly double MaxMinusMinDividedByWeight;
        public static readonly double MinSpeed;

        public static readonly double MaxSpeed;
        public static readonly double RentingRatio;
        public static readonly int NumberOfItems;
        public static readonly int MaxCapacityOfKnapsack;

        public static double[,] DistanceMatrix;
        private static Dictionary<int, Item> _items;

        public static Item GetItem(int itemId)
        {
            _items.TryGetValue(itemId, out Item item);
            return item;
        }

        public static double GetDistance(int i, int j)
        {
            return DistanceMatrix[i, j];
        }
    }
}