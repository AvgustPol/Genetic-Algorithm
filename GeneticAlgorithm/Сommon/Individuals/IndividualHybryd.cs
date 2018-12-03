using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing;
using StatisticsCounter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmLogic.Сommon.Individuals
{
    public class IndividualHybryd : Individual
    {
        public IndividualHybryd()
        {
            Parameters = new int[Enum.GetNames(typeof(GeneticAlgorithmParameters.GeneticAlgorithmParametersType)).Length];
        }

        public IndividualHybryd(int[] parameters)
        {
            Parameters = parameters;
            MetaheuristicParameters = new SimulatedAnnealingParameters(parameters);

            CountFitness();
        }

        public int[] Parameters { get; set; }

        public SimulatedAnnealing Metaheuristic { get; set; }
        public MetaheuristicParameters MetaheuristicParameters { get; set; }

        private double GetMedian(List<MetaheuristicResult> allLoopsData)
        {
            double[] bestArray = GetOnlyBest(allLoopsData);
            double median = MedianCounter.CountMedian(bestArray);

            return median;
        }

        private double[] GetOnlyBest(List<MetaheuristicResult> allLoopsData)
        {
            double[] bestFintess = new double[GlobalParameters.NumberOfRuns];
            for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
            {
                var metaheuristicResult = allLoopsData[i];
                double best = metaheuristicResult.Fitness.ListBest.Last();
                bestFintess[i] = best;
            }

            return bestFintess;
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void CountFitness()
        {
            List<MetaheuristicResult> allLoopsData = new List<MetaheuristicResult>();

            for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
            {
                MetaheuristicResult metaheuristicResult = Metaheuristic.Run(MetaheuristicParameters);
                allLoopsData.Add(metaheuristicResult);
            }

            double median = GetMedian(allLoopsData);
            Fitness = median;
        }

        public override int[] GetMutation()
        {
            throw new NotImplementedException();
        }

        public override List<int[]> GetNeighbors(int numberOfNeighbors)
        {
            throw new NotImplementedException();
        }

        public override bool Mutate()
        {
            throw new NotImplementedException();
        }
    }
}