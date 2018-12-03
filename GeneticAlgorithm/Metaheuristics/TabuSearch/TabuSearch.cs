//#define Generator
#define TspKnp
#undef Generator
//#undef TspKnp

using GeneticAlgorithmLogic.Individuals;
using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Сommon;
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
            ItIsTimeToStopAlg = false;
            int nonChangeFitnessCounter = 0;

            TabuSearchParameters = (TabuSearchParameters)algorithmParameters;
            TabuList = new Queue<int[]>(TabuSearchParameters.TabuListSize);

            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();
            List<int[]> neighbors;

            IndividualTspKnp best = new IndividualTspKnp(Population.CreateRandomIndividual());
            IndividualTspKnp current = best;

            //best fount
            //current -> best Neighbor

            double bestNeighborFitness = best.Fitness;
            double averageNeighborFitness = best.Fitness;

            AddToTabuList(current.Places);

            List<double> neighborsFitness = new List<double>(TabuSearchParameters.NumberOfNeighbors);

            while (!ItIsTimeToStopAlg)
            {
                neighbors = current.GetNeighbors(TabuSearchParameters.NumberOfNeighbors);

                foreach (var candidate in neighbors)
                {
                    if (!IsContains(candidate))
                    {
                        IndividualTspKnp tmpCandidate = new IndividualTspKnp(candidate);

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

                if (metaheuristicResult.Fitness.ListBest.Count > 1)
                {
                    int currentGenerationFitness = metaheuristicResult.Fitness.ListBest.Count - 1;
                    int previousGenerationFitness = metaheuristicResult.Fitness.ListBest.Count - 2;

                    if (metaheuristicResult.Fitness.ListBest.ElementAt(currentGenerationFitness) == metaheuristicResult.Fitness.ListBest.ElementAt(previousGenerationFitness))
                    {
                        nonChangeFitnessCounter++;
                        if (nonChangeFitnessCounter > GlobalParameters.NumberOfNonchangedFitness)
                        {
                            ItIsTimeToStopAlg = true;
                        }
                    }
                    else
                    {
                        nonChangeFitnessCounter = 0;
                    }
                }
            }

            return metaheuristicResult;
        }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters, int generationsNumber)
        {
            TabuSearchParameters = (TabuSearchParameters)algorithmParameters;
            TabuList = new Queue<int[]>(TabuSearchParameters.TabuListSize);

            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();
            List<int[]> neighbors;

            IndividualTspKnp best = new IndividualTspKnp(Population.CreateRandomIndividual());
            IndividualTspKnp current = best;

            //best fount
            //current -> best Neighbor

            double bestNeighborFitness = best.Fitness;
            double averageNeighborFitness = best.Fitness;

            AddToTabuList(current.Places);

            List<double> neighborsFitness = new List<double>(TabuSearchParameters.NumberOfNeighbors);

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                neighbors = current.GetNeighbors(TabuSearchParameters.NumberOfNeighbors);

                foreach (var candidate in neighbors)
                {
                    if (!IsContains(candidate))
                    {
                        IndividualTspKnp tmpCandidate = new IndividualTspKnp(candidate);

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