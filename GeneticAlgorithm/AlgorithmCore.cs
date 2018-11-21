using DataModel;
using GeneticAlgorithmLogic.Metaheuristics;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using System.Collections.Generic;

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

            //TODO:
            //CountStandardDeviation
            MetaheuristicResult allMetaheuristicsAverage = CalculateAverageFintessForAllRunsOfTheAlgorithm(allLoopsData);

            toFileLogger.LogMetaheuristicToFile(MetaheuristicType, metaheuristicParameters, allMetaheuristicsAverage);
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