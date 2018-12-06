using GeneticAlgorithmLogic.Metaheuristics.Parameters;

namespace GeneticAlgorithmLogic.Metaheuristics
{
    public class MetaheuristicFactory
    {
        public static Metaheuristic CreateMetaheuristic(MetaheuristicParameters.MetaheuristicType type)
        {
            Metaheuristic metaheuristic = null;
            switch (type)
            {
                case MetaheuristicParameters.MetaheuristicType.GA:
                    metaheuristic = new GeneticAlgorithm.GeneticAlgorithm();
                    break;

                case MetaheuristicParameters.MetaheuristicType.SA:
                    metaheuristic = new SimulatedAnnealing.SimulatedAnnealing();
                    break;

                case MetaheuristicParameters.MetaheuristicType.TS:
                    metaheuristic = new TabuSearch.TabuSearch();
                    break;

                default:
                    break;
            }

            return metaheuristic;
        }
    }
}