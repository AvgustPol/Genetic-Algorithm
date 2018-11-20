namespace GeneticAlgorithm.Metaheuristics.SimulatedAnnealing
{
    internal class SimulatedAnnealingParameters : MetaheuristicParameters
    {
        //public const double InitializeTemperature = double.MaxValue;
        public const double InitializeTemperature = 100000;

        public static int NumberOfNeighbors = 50;
    }
}