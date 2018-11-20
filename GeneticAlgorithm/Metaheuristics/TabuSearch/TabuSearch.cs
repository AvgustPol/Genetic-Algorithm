using System.Collections.Generic;

namespace GeneticAlgorithm.Metaheuristics.TabuSearch
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

        public bool IsContains(int[] array)
        {
            return TabuList.Contains(array);
        }
    }
}