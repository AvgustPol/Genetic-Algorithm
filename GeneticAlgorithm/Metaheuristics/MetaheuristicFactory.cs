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