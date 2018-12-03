using System.Collections.Generic;
using GeneticAlgorithmLogic.Individuals;

namespace GeneticAlgorithmLogic
{
    internal class NeighborsGenerator
    {
        public static List<int[]> GetNeighbors(IndividualTspKnp current, int numberOfNeighbors)
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