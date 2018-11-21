using GeneticAlgorithmLogic.Metaheuristics.Parameters;

namespace GeneticAlgorithmLogic.Metaheuristics.TabuSearch
{
    public class TabuSearchParameters : MetaheuristicParameters
    {
        public int TabuListSize;
        public const int MinTabuListSize = 1;
        public const int MaxTabuListSize = 10000;

        public int NumberOfNeighbors;
        public const int MinNumberOfNeighbors = 1;
        public const int MaxNumberOfNeighbors = 10000;
    }
}