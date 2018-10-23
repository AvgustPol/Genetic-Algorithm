using DataModel;
using Loader;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithmParameters
    {
        static GeneticAlgorithmParameters()
        {
            DataLoader dataLoader = new DataLoader();
            DataContainer container = dataLoader.GetCreatedDataContainerFromFileAsync(GlobalParameters.PathToTestData).Result;

            Dimension = container.Dimension;
            MinSpeed = container.MinSpeed;
            MaxSpeed = container.MaxSpeed;
            RentingRatio = container.RentingRatio;
            DistanceMatrix = container.DistanceMatrix;
            _items = container.Items;
            NumberOfItems = container.NumberOfItems;
            CapacityOfKnapsack = container.CapacityOfKnapsack;
        }

        public static double GetDistance(int i, int j)
        {
            return DistanceMatrix[i, j];
        }

        /// <summary>
        /// number of places at TSP problem
        /// </summary>
        public static readonly int Dimension;

        public static readonly double MaxMinusMinDividedByWeight = (MaxSpeed - MinSpeed) / CapacityOfKnapsack;
        public static readonly double MinSpeed;

        public static readonly double MaxSpeed;
        public static readonly double RentingRatio;
        public static readonly int NumberOfItems;
        public static readonly int CapacityOfKnapsack;

        /// <summary>
        /// The probability of crossing two individuals. (e. g. 1% )
        /// </summary>
        public const int CrossProbability = 60;

        /// <summary>
        /// The mutations probability of an individual gene  (e. g. 1% )
        /// Example :
        /// Genome (permutation) looks like {1 2 3 4 5} and it is successfully mutated
        /// If it mutated in a position 2 we must swap value at position 2 (permutation[2]) with a Random gene
        ///     For example Random gene = 4
        /// So we must swap 2 and 4 positions in an array.
        ///
        /// Result:
        /// Genome (permutation) looks like {1 2 5 4 3} after mutation
        /// </summary>
        public static readonly int MutationProbability = 5;

        public static readonly int MaxProbability = 100;

        public static readonly int NumberOfTournamentParticipants = GlobalParameters.SameNumber;

        public static double[,] DistanceMatrix;
        private static Dictionary<int, Item> _items;

        public static Item GetItem(int itemId)
        {
            _items.TryGetValue(itemId, out Item item);
            return item;
        }
    }
}