using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmLogic.Metaheuristics.TabuSearch
{
    public class TabuSearch : Metaheuristic
    {
        public static TabuSearchParameters TabuSearchParameters;
        public Queue<int[]> TabuList { get; set; }

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

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters)
        {
            TabuSearchParameters = (TabuSearchParameters)algorithmParameters;
            TabuList = new Queue<int[]>(TabuSearchParameters.TabuListSize);

            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();
            List<int[]> neighbors;

            Individual best = new Individual(Population.CreateRandomIndividual());
            Individual current = best;

            //best fount
            //current -> best Neighbor

            double bestNeighborFitness = best.Fitness;
            double averageNeighborFitness = best.Fitness;

            AddToTabuList(current.Places);

            List<double> neighborsFitness = new List<double>(TabuSearchParameters.NumberOfNeighbors);

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                neighbors = NeighborsGenerator.GetNeighbors(current, TabuSearchParameters.NumberOfNeighbors);

                foreach (var candidate in neighbors)
                {
                    if (!IsContains(candidate))
                    {
                        Individual tmpCandidate = new Individual(candidate);

                        #region Save to neighbors fitness list

                        neighborsFitness.Add(tmpCandidate.Fitness);

                        #endregion Save to neighbors fitness list

                        if (tmpCandidate.Fitness > current.Fitness)
                            current = tmpCandidate;
                    }
                }
                bestNeighborFitness = current.Fitness;

                averageNeighborFitness = neighborsFitness.Average();
                neighborsFitness.Clear();

                AddToTabuList(current.Places);

                metaheuristicResult.SaveBestFitnessForCurrentGeneration(bestNeighborFitness);
                metaheuristicResult.SaveAverageFitnessForCurrentGeneration(averageNeighborFitness);

                //TODO delete 42 and change to TS list XD
                metaheuristicResult.SaveWorstFitnessForCurrentGeneration(42);
            }

            return metaheuristicResult;
        }
    }
}