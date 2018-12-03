using GeneticAlgorithmLogic.Metaheuristics.Parameters;

namespace GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing
{
    public class SimulatedAnnealingParameters : MetaheuristicParameters
    {
        public const int InitializeTemperatureIndex = 0;
        public double InitializeTemperature;

        public int NumberOfNeighbors;
        public const int NumberOfNeighborsIndex = 1;

        public SimulatedAnnealingParameters()
        {
        }

        public SimulatedAnnealingParameters(int[] parameters)
        {
            InitializeTemperature = parameters[InitializeTemperatureIndex];
            NumberOfNeighbors = parameters[NumberOfNeighborsIndex];
        }
    }
}