using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Individuals
{
    public interface INeighborly
    {
        List<int[]> GetNeighbors(int numberOfNeighbors);
    }
}