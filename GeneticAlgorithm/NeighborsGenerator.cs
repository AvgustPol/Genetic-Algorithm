using System.Collections.Generic;

namespace GeneticAlgorithmLogic
{
    internal class NeighborsGenerator
    {
        public static List<int[]> GetNeighbors(Individual current, int numberOfNeighbors)
        {
            List<int[]> neighbors = new List<int[]>(numberOfNeighbors);

            for (int i = 0; i < numberOfNeighbors; i++)
            {
                neighbors.Add(current.GetMutation());
            }

            return neighbors;
        }
    }
}