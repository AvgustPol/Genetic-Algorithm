using GeneticAlgorithmLogic.Metaheuristics;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Ñommon;
using StatisticsCounter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmLogic
{
    public class AlgorithmCore
    {
        public AlgorithmCore(MetaheuristicParameters.MetaheuristicType metaheuristicType, string sourceDataFile)
        {
            AlgorithmCoreParameters.ReadDataFromFileAndCreateAlgorithmCoreParameters(sourceDataFile);

            SourceDataFile = sourceDataFile;
            MetaheuristicType = metaheuristicType;
            Metaheuristic = MetaheuristicFactory.CreateMetaheuristic(metaheuristicType);
        }

        public Metaheuristic Metaheuristic { get; set; }
        public MetaheuristicParameters.MetaheuristicType MetaheuristicType { get; set; }
        public string SourceDataFile { get; set; }

        public void RunAndLogAlgorithm(MetaheuristicParameters metaheuristicParameters)
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{SourceDataFile} {MetaheuristicType} result ");

            MetaheuristicResult metaheuristicResult = Metaheuristic.Run(metaheuristicParameters);

            toFileLogger.LogMetaheuristicToFile(metaheuristicParameters, metaheuristicResult);
        }

        public void RunAnalytic(MetaheuristicParameters metaheuristicParameters)
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{SourceDataFile} {MetaheuristicType} result ");

            List<MetaheuristicResult> allLoopsData = new List<MetaheuristicResult>();

            for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
            {
                MetaheuristicResult metaheuristicResult = Metaheuristic.Run(metaheuristicParameters);
                allLoopsData.Add(metaheuristicResult);
            }

            var analizeResult = Analize(allLoopsData);

            toFileLogger.LogAnalytic(analizeResult);
        }

        public void RunAnalyticForCurrentFile()
        {
            MetaheuristicParameters parameters = MetaheuristicParametersFactory.CreateParameters(MetaheuristicType);

            RunAnalytic(parameters);
        }

        public void RunAndLogForCurrentFile()
        {
            MetaheuristicParameters parameters = MetaheuristicParametersFactory.CreateParameters(MetaheuristicType);

            RunAndLogAlgorithm(parameters);
        }

        private Tuple<double, double, double> Analize(List<MetaheuristicResult> allLoopsData)
        {
            double max, avg, dev;

            double[] bestArray = GetOnlyBest(allLoopsData);
            max = bestArray.Max();
            avg = bestArray.Average();
            dev = StandardDeviationCounter.CountStandardDeviation(avg, bestArray);

            return new Tuple<double, double, double>(max, avg, dev);
        }

        private MetaheuristicResult CalculateAverageFintessForAllRunsOfTheAlgorithm(List<MetaheuristicResult> allLoopsData)
        {
            MetaheuristicResult allAlgorithmsAverage = new MetaheuristicResult();
            for (int i = 0; i < GlobalParameters.IntegerAlgorithmStopCondition; i++)
            {
                double averageBestFitness = AverageCounter.CountAverageFitnessFor(allLoopsData, i, GlobalParameters.BestFitness);
                double averageAverageFitness = AverageCounter.CountAverageFitnessFor(allLoopsData, i, GlobalParameters.AverageFitness);
                double averageWorstFitness = AverageCounter.CountAverageFitnessFor(allLoopsData, i, GlobalParameters.WorstFitness);

                allAlgorithmsAverage.Fitness.ListBest.Add(averageBestFitness);
                allAlgorithmsAverage.Fitness.ListAverage.Add(averageAverageFitness);
                allAlgorithmsAverage.Fitness.ListWorst.Add(averageWorstFitness);
            }

            return allAlgorithmsAverage;
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