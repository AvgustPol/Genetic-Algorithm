namespace GeneticAlgorithm
{
    public class GlobalParameters
    {
        /// <summary>
        /// STOP_CONDITION
        /// Number of generations that will be generated before stop.
        /// </summary>
        public static readonly int AlgorithmStopCondition = 101;

        /// <summary>
        /// STOP_CONDITION
        /// Number of runs algorithm to exlore it
        /// </summary>
        public static readonly int ExploringAlgorythmStopCondition = 10;

        /// <summary>
        /// Using same value for GA population size and TS (Tabu Search ) size
        /// </summary>
        public static readonly int SameSize = 100;

        /// <summary>
        /// Using same value for GA tournament size and TS (Tabu Search )  neighbors number
        /// </summary>
        public static readonly int SameNumber = 3;
    }
}