using DataModel;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using Loader;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm
{
    public class GeneticAlgorithmParameters : MetaheuristicParameters
    {
        public enum GeneticAlgorithmParametersType
        {
            PopulationSize,
            NumberOfTournamentParticipants,
            MutationProbability,
            CrossProbability
        }

        public static GeneticAlgorithmParameters GetNeighbor(GeneticAlgorithmParametersType type)
        {
            switch (type)
            {
            }
            return null;
        }

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
        public int MutationProbability;

        public const int MinProbability = 1;
        public const int MaxProbability = 100;

        public int NumberOfTournamentParticipants;
        public const int MinNumberOfTournamentParticipants = 2;
        public static readonly int MaxNumberOfTournamentParticipants = Dimension;

        /// <summary>
        /// Number of population individuals
        /// </summary>
        public int PopulationSize;

        public const int MinPopulationSize = 2;
        public const int MaxPopulationSize = 10000;

        /// <summary>
        /// The probability of crossing two individuals. (e. g. 1% )
        /// </summary>
        public int CrossProbability;

        static GeneticAlgorithmParameters()
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