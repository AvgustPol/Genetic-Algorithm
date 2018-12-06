using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing;
using GeneticAlgorithmLogic.Metaheuristics.TabuSearch;
using static GeneticAlgorithmLogic.Metaheuristics.Parameters.MetaheuristicParameters;

namespace GeneticAlgorithmLogic.Metaheuristics.Parameters
{
    public class MetaheuristicParametersFactory
    {
        public static MetaheuristicParameters CreateParameters(MetaheuristicType MetaheuristicType)
        {
            MetaheuristicParameters parameters;
            switch (MetaheuristicType)
            {
                case MetaheuristicType.GA:
                    parameters = CreateGaParameters();
                    break;

                case MetaheuristicType.TS:
                    parameters = CreateTsParameters();
                    break;

                case MetaheuristicType.SA:
                    parameters = CreateSaSarameters();
                    break;

                default:
                    parameters = null;
                    break;
            }

            return parameters;
        }

        private static GeneticAlgorithmParameters CreateGaParameters()
        {
            return new GeneticAlgorithmParameters()
            {
                MutationProbability = 5,
                CrossProbability = 60,
                NumberOfTournamentParticipants = 5,
                PopulationSize = 100
            };
        }

        private static MetaheuristicParameters CreateTsParameters()
        {
            return new TabuSearchParameters()
            {
                TabuListSize = 322,
                NumberOfNeighbors = 25
            };
        }

        private static SimulatedAnnealingParameters CreateSaSarameters()
        {
            return new SimulatedAnnealingParameters()
            {
                InitializeTemperature = 1000,
                NumberOfNeighbors = 3
            };
        }
    }
}