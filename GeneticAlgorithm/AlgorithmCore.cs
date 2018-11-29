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
        public Metaheuristic Metaheuristic { get; set; }
        public MetaheuristicParameters.MetaheuristicType MetaheuristicType { get; set; }
        public string SourceDataFile { get; set; }

        public AlgorithmCore(MetaheuristicParameters.MetaheuristicType metaheuristicType, string sourceDataFile)
        {
            SourceDataFile = sourceDataFile;
            MetaheuristicType = metaheuristicType;
            Metaheuristic = MetaheuristicFactory.CreateMetaheuristic(metaheuristicType);
        }

        //public AlgorithmCore(MetaheuristicParameters.MetaheuristicType metaheuristicType)
        //{
        //    MetaheuristicType = metaheuristicType;
        //    Metaheuristic = MetaheuristicFactory.CreateMetaheuristic(metaheuristicType);
        //}

        public void RunForCurrentFile()
        {
            MetaheuristicParameters parameters = MetaheuristicParametersFactory.CreateParameters(MetaheuristicType);

            RunAlgorithm(parameters);
        }

        public void RunAlgorithm(MetaheuristicParameters metaheuristicParameters)
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{SourceDataFile} {MetaheuristicType} result ");

            MetaheuristicResult metaheuristicResult = Metaheuristic.Run(metaheuristicParameters);

            //MetaheuristicResult allMetaheuristicsAverage = CalculateAverageFintessForAllRunsOfTheAlgorithm(allLoopsData);

            //var analizeResult = Analize(allLoopsData, allMetaheuristicsAverage.Fitness.ListBest.Last());

            //toFileLogger.LogAnalytic(metaheuristicResult);
            toFileLogger.LogMetaheuristicToFile(metaheuristicParameters, metaheuristicResult);
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
                double best = metaheuristicResult.Fitness.ListBest.Last();
                bestFintess[i] = best;
            }

            return bestFintess;
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
    }
}