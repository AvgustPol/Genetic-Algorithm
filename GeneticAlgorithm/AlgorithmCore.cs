using DataModel;
using GeneticAlgorithm.Metaheuristics;
using GeneticAlgorithm.Metaheuristics.GeneticAlgorithm;
using System.Collections.Generic;
using static GeneticAlgorithm.Metaheuristics.MetaheuristicParameters;

namespace GeneticAlgorithm
{
    public class AlgorithmCore
    {
        public Metaheuristic Metaheuristic { get; set; }
        public MetaheuristicType MetaheuristicType { get; set; }

        public AlgorithmCore(MetaheuristicType metaheuristicType)
        {
            MetaheuristicType = metaheuristicType;
            Metaheuristic = MetaheuristicFactory.CreateMetaheuristic(metaheuristicType);
        }

        public void Run()
        {
            MetaheuristicParameters parameters = new GeneticAlgorithmParameters()
            {
                MutationProbability = 5,
                CrossProbability = 60,
                NumberOfTournamentParticipants = 5,
                PopulationSize = 100
            };

            RunAlgorithm(parameters);
        }

        public void RunAlgorithm(MetaheuristicParameters parameters)
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} {MetaheuristicType} result.csv");

            List<MetaheuristicResult> allLoopsData = new List<MetaheuristicResult>(GlobalParameters.NumberOfRuns);
            for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
            {
                MetaheuristicResult metaheuristicResult = Metaheuristic.Run(parameters);
                allLoopsData.Add(metaheuristicResult);
            }

            MetaheuristicResult allMetaheuristicsAverage = CalculateAverageForAllRunsOfTheAlgorithm(allLoopsData);

            //TODO:
            //CountStandardDeviation

            toFileLogger.LogToFile(metaheuristicType, allMetaheuristicsAverage);
        }

        private MetaheuristicResult CalculateAverageForAllRunsOfTheAlgorithm(List<MetaheuristicResult> dataList)
        {
            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                #region Get GA MetaheuristicResult

                double averageBestFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListGA);
                double averageAverageFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.AverageFitnessListGA);
                double averageWorstFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.WorstFitnessListGA);

                #endregion Get GA MetaheuristicResult

                #region Get TS data

                double averageBestFitnessTS = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListTS);

                #endregion Get TS data

                #region Get SA data

                double averageBestFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);
                double averageBestNeighborFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);

                #endregion Get SA data

                #region Save GA

                allAlgorithmsAverage.SaveData(averageBestFitnessGA);
                allAlgorithmsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
                allAlgorithmsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);

                #endregion Save GA

                #region Save TS

                allAlgorithmsAverage.SaveBestFitnessForTS(averageBestFitnessTS);

                #endregion Save TS

                #region Save SA

                allAlgorithmsAverage.SaveBestFitnessForSA(averageBestFitnessSA);
                allAlgorithmsAverage.SaveBestNeighborFitnessForSA(averageBestFitnessSA);

                #endregion Save SA
            }

            return allAlgorithmsAverage;
        }
    }
}