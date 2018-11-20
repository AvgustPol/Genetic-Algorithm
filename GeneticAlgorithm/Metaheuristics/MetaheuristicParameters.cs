namespace GeneticAlgorithmLogic.Metaheuristics
{
    public class MetaheuristicParameters
    {
        public enum MetaheuristicType
        {
            GA,
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