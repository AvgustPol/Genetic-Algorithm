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

        ///// <summary>
        ///// Run Tabu Search
        ///// </summary>
        ///// <returns></returns>
        //private MetaheuristicResult RunTS()
        //{
        //    MetaheuristicResult<double> metaheuristicResult = new MetaheuristicResult<double>();
        //    List<int[]> neighbors;
        //    TabuSearch tabuSearch = new TabuSearch();

        //    Individual best = new Individual(Population.CreateRandomIndividual());
        //    Individual current = best;

        //    //best fount
        //    //current -> best Neighbor

        //    double bestNeighborFitness = best.Fitness;
        //    double bestAlgorithmFitness = best.Fitness;

        //    tabuSearch.AddToTabuList(current.Places);

        //    for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
        //    {
        //        neighbors = NeighborsGenerator.GetNeighbors(current, TabuSearchParameters.NumberOfNeighbors);

        //        foreach (var candidate in neighbors)
        //        {
        //            if (!tabuSearch.IsContains(candidate))
        //            {
        //                Individual tmpCandidate = new Individual(candidate);
        //                if (tmpCandidate.Fitness > current.Fitness)
        //                    current = tmpCandidate;
        //            }
        //        }
        //        bestNeighborFitness = current.Fitness;

        //        if (bestNeighborFitness > bestAlgorithmFitness)
        //        {
        //            bestAlgorithmFitness = bestNeighborFitness;
        //        }

        //        tabuSearch.AddToTabuList(current.Places);

        //        metaheuristicResult.SaveBestNeighborFitnessForTS(bestNeighborFitness);
        //        metaheuristicResult.SaveBestFitnessForTS(bestAlgorithmFitness);
        //    }

        //    return metaheuristicResult;
        //}
    }
}