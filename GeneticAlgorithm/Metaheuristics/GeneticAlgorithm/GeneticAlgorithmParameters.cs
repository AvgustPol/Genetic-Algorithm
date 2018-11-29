using GeneticAlgorithmLogic.Metaheuristics.Parameters;

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
                case GeneticAlgorithmParametersType.MutationProbability:

                    break;

                case GeneticAlgorithmParametersType.CrossProbability:

                    break;

                case GeneticAlgorithmParametersType.NumberOfTournamentParticipants:

                    break;

                case GeneticAlgorithmParametersType.PopulationSize:

                    break;

                default:

                    break;
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
        public static readonly int MaxNumberOfTournamentParticipants = AlgorithmCoreParameters.Dimension;

        /// <summary>
        /// Number of population individuals
        /// </summary>
        public int PopulationSize;

        public const int MinPopulationSize = 2;
        public const int MaxPopulationSize = 1000;

        /// <summary>
        /// The probability of crossing two individuals. (e. g. 1% )
        /// </summary>
        public int CrossProbability;
    }
}