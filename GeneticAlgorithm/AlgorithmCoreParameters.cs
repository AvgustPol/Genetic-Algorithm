using GeneticAlgorithmLogic.Сommon;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic
{
    public class AlgorithmCoreParameters
    {
        public static void ReadDataFromFileAndCreateAlgorithmCoreParameters(string fileName)
        {
            DataLoader dataLoader = new DataLoader();
            DataContainer container = dataLoader.GetCreatedDataContainerFromFile(GlobalParameters.PathToTestFolder + fileName);

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
        public static int Dimension;

        public static double MaxMinusMinDividedByWeight;
        public static double MinSpeed;

        public static double MaxSpeed;
        public static double RentingRatio;
        public static int NumberOfItems;
        public static int MaxCapacityOfKnapsack;

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