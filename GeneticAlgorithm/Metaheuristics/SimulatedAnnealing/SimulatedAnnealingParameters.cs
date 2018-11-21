using GeneticAlgorithmLogic.Metaheuristics.Parameters;

namespace GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing
{
    public class SimulatedAnnealingParameters : MetaheuristicParameters
    {
        //public const double InitializeTemperature = double.MaxValue;
        public double InitializeTemperature;

        public int NumberOfNeighbors;
    }
}