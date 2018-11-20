namespace GeneticAlgorithm.Metaheuristics
{
    public class MetaheuristicParameters
    {
        public enum MetaheuristicType
        {
            ,
            SA,
            TS
        }

        public int AlgorithmStopCondition { get; set; }

        public MetaheuristicParameters()
        {
            AlgorithmStopCondition = 100;
        }
    }
}