using static GeneticAlgorithm.Metaheuristics.MetaheuristicParameters;

namespace GeneticAlgorithm.Metaheuristics
{
    public class MetaheuristicFactory
    {
        public static Metaheuristic<T> CreateMetaheuristic(MetaheuristicType type)
        {
            Metaheuristic<T> metaheuristic = null;
            switch (type)
            {
                case MetaheuristicType.GA:
                    metaheuristic = new GeneticAlgorithm.GeneticAlgorithm();
                    break;

                //case MetaheuristicType.SA:
                //    metaheuristic = new SimulatedAnnealing();
                //    break;

                //case MetaheuristicType.TS:
                //    algorithmResult = RunTS();
                //    break;

                default:
                    break;
            }

            return metaheuristic;
        }
    }
}