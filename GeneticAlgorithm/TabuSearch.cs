using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class TabuSearch
    {
        public Queue<int[]> TabuList { get; set; }

        public TabuSearch()
        {
            TabuList = new Queue<int[]>(TabuSearchParameters.TabuListSize);
        }

        public void AddToTabuList(int[] array)
        {
            if (!TabuList.Contains(array))
            {
                TabuList.Enqueue(array);
            }

            if (TabuList.Count >= TabuSearchParameters.TabuListSize)
            {
                TabuList.Dequeue();
            }
        }

        public int[] GetNeighbour(Individual individual)
        {
            int[] res = new int[GeneticAlgorithmParameters.Dimension];

            res = individual.GetMutatation();

            return res;
        }

        public bool IsContains(int[] array)
        {
            return TabuList.Contains(array);
        }

        public List<int[]> GetNeighbors(Individual current, int numberOfNeighbors)
        {
            List<int[]> neighbors = new List<int[]>(numberOfNeighbors);

            for (int i = 0; i < numberOfNeighbors; i++)
            {
                neighbors.Add(current.GetMutatation());
            }

            return neighbors;
        }
    }
}