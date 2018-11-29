using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Сommon
{
    public class DataContainer
    {
        public double[,] DistanceMatrix;

        public int Dimension { get; set; }
        public int NumberOfItems { get; set; }
        public int MaxCapacityOfKnapsack { get; set; }

        public double MinSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public double RentingRatio { get; set; }

        public Dictionary<int, Item> Items { get; set; }

        public List<Place> Places { get; set; }
    }
}