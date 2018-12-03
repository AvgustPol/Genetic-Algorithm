using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Сommon.Individuals
{
    public interface INeighborly
    {
        List<int[]> GetNeighbors(int numberOfNeighbors);
    }
}