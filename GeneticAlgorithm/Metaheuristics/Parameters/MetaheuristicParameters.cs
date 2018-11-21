namespace GeneticAlgorithmLogic.Metaheuristics.Parameters
{
    public class MetaheuristicParameters
    {
        public enum MetaheuristicType
        {
            GA,
            SA,
            TS
        }

        public int AlgorithmStopCondition { get; set; } = 1779;
        //public int AlgorithmStopCondition { get; set; } = 2878;
    }
}