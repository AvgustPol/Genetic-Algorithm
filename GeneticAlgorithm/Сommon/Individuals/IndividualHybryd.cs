using GeneticAlgorithmLogic.Metaheuristics;
using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using StatisticsCounter;
using System;
using System.Collections.Generic;
using System.Linq;
using static GeneticAlgorithmLogic.Metaheuristics.Parameters.MetaheuristicParameters;

namespace GeneticAlgorithmLogic.Сommon.Individuals
{
    public class IndividualHybryd : Individual
    {
        public IndividualHybryd()
        {
            CreateParameters();
            Metaheuristic = (GeneticAlgorithm)MetaheuristicFactory.CreateMetaheuristic(Type);

            CountFitness();
        }

        public IndividualHybryd(int[] parameters)
        {
            Parameters = parameters;
            MetaheuristicParameters = new GeneticAlgorithmParameters(parameters);
            Metaheuristic = (GeneticAlgorithm)MetaheuristicFactory.CreateMetaheuristic(Type);

            CountFitness();
        }

        private void CreateParameters()
        {
            Parameters = CreateRandomParameters();
        }

        private int[] CreateRandomParameters()
        {
            int length = Enum.GetNames(typeof(GeneticAlgorithmParameters.GeneticAlgorithmParametersType)).Length;
            var RandomParameters = new int[length];
            for (int i = 0; i < length; i++)
            {
                RandomParameters[i] = Randomizer.Random.Next(GeneticAlgorithmParameters.MinProbability, GeneticAlgorithmParameters.MaxProbability);
            }

            return RandomParameters;
        }

        public GeneticAlgorithm Metaheuristic { get; set; }
        public const MetaheuristicType Type = MetaheuristicType.GA;
        public MetaheuristicParameters MetaheuristicParameters { get; set; }
        public int[] Parameters { get; set; }

        public override object Clone()
        {
            IndividualHybryd clone = new IndividualHybryd();
            if (Parameters != null)
            {
                clone.Parameters = (int[])Parameters.Clone();
            }

            if (Metaheuristic != null)
            {
                clone.Metaheuristic = Metaheuristic;
            }

            if (MetaheuristicParameters != null)
            {
                clone.MetaheuristicParameters = MetaheuristicParameters;
            }
            //TODO : точно ли тебе нужно клонировать все таблицы?

            //if (ItemsLocation != null)
            //{
            //    clone.ItemsLocation = (int[])ItemsLocation.Clone();
            //}

            clone.Fitness = Fitness;

            return clone;
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
            int[] tmp = new int[Parameters.Length];

            for (int i = 0; i < Parameters.Length; i++)
            {
                //double mean = 0;
                //double sigma = 10;
                //NormalDist dist = new NormalDist(mean, sigma);
                //var NormalDistribution = (int)dist.PDF(Parameters[i]);

                //var max = Math.Max(NormalDistribution, GeneticAlgorithmParameters.MinProbability);
                //var newParameter = Math.Min(max, GeneticAlgorithmParameters.MaxProbability);

                //tmp[i] = newParameter;

                tmp[i] = Randomizer.Random.Next(GeneticAlgorithmParameters.MinProbability,
                    GeneticAlgorithmParameters.MaxProbability);
            }

            return tmp;
        }

        public override List<int[]> GetNeighbors(int numberOfNeighbors)
        {
            List<int[]> neighbors = new List<int[]>(numberOfNeighbors);

            for (int i = 0; i < numberOfNeighbors; i++)
            {
                neighbors.Add(GetMutation());
            }

            return neighbors;
        }

        public override bool Mutate()
        {
            throw new NotImplementedException();
        }

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
    }
}