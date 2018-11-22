using DataModel;
using GeneticAlgorithmLogic.Metaheuristics;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using StatisticsCounter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmLogic
{
    public class AlgorithmCore
    {
        public Metaheuristic Metaheuristic { get; set; }
        public MetaheuristicParameters.MetaheuristicType MetaheuristicType { get; set; }

        public AlgorithmCore(MetaheuristicParameters.MetaheuristicType metaheuristicType)
        {
            MetaheuristicType = metaheuristicType;
            Metaheuristic = MetaheuristicFactory.CreateMetaheuristic(metaheuristicType);
        }

        public void Run()
        {
            MetaheuristicParameters parameters = MetaheuristicParametersFactory.CreateParameters(MetaheuristicType);

            RunAlgorithm(parameters);
        }

        public void RunAlgorithm(MetaheuristicParameters metaheuristicParameters)
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} {MetaheuristicType} result.csv");

            List<MetaheuristicResult> allLoopsData = new List<MetaheuristicResult>(GlobalParameters.NumberOfRuns);
            for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
            {
                MetaheuristicResult metaheuristicResult = Metaheuristic.Run(metaheuristicParameters);
                allLoopsData.Add(metaheuristicResult);
            }

            MetaheuristicResult allMetaheuristicsAverage = CalculateAverageFintessForAllRunsOfTheAlgorithm(allLoopsData);

            var analizeResult = Analize(allLoopsData, allMetaheuristicsAverage._fitnessResult.ListBest.Last());

            toFileLogger.LogAnalytic(analizeResult);
            toFileLogger.LogMetaheuristicToFile(MetaheuristicType, metaheuristicParameters, allMetaheuristicsAverage);
        }

        private Tuple<double, double, double> Analize(List<MetaheuristicResult> allLoopsData, double averageBest)
        {
            double max, avg, dev;

            double[] bestArray = GetOnlyBest(allLoopsData);
            max = bestArray.Max();
            avg = bestArray.Average();
            dev = StandardDeviationCounter.CountStandardDeviation(averageBest, bestArray);

            return new Tuple<double, double, double>(max, avg, dev);
        }

        private double[] GetOnlyBest(List<MetaheuristicResult> allLoopsData)
        {
            double[] bestFintess = new double[GlobalParameters.NumberOfRuns];
            for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
            {
                var metaheuristicResult = allLoopsData[i];
                double best = metaheuristicResult._fitnessResult.ListBest.Last();
                bestFintess[i] = best;
            }

            return bestFintess;
        }

        private MetaheuristicResult CalculateAverageFintessForAllRunsOfTheAlgorithm(List<MetaheuristicResult> allLoopsData)
        {
            MetaheuristicResult allAlgorithmsAverage = new MetaheuristicResult();
            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                double averageBestFitness = AverageCounter.CountAverageFitnessFor(allLoopsData, i, GlobalParameters.BestFitness);
                double averageAverageFitness = AverageCounter.CountAverageFitnessFor(allLoopsData, i, GlobalParameters.AverageFitness);
                double averageWorstFitness = AverageCounter.CountAverageFitnessFor(allLoopsData, i, GlobalParameters.WorstFitness);

                allAlgorithmsAverage._fitnessResult.ListBest.Add(averageBestFitness);
                allAlgorithmsAverage._fitnessResult.ListAverage.Add(averageAverageFitness);
                allAlgorithmsAverage._fitnessResult.ListWorst.Add(averageWorstFitness);
            }

            return allAlgorithmsAverage;
        }
    }
}