using DataModel;
using GeneticAlgorithmLogic.Metaheuristics;
using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
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
            MetaheuristicParameters parameters = new GeneticAlgorithmParameters()
            {
                MutationProbability = 5,
                CrossProbability = 60,
                NumberOfTournamentParticipants = 5,
                PopulationSize = 100
            };

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
            //MetaheuristicResult allMetaheuristicsAverage = CalculateAverageForAllRunsOfTheAlgorithm(allLoopsData);
            //toFileLogger.LogMetaheuristicToFile(MetaheuristicType, metaheuristicParameters, allMetaheuristicsAverage);

            //TODO : change allLoopsData[0] to allMetaheuristicsAverage
            toFileLogger.LogMetaheuristicToFile(MetaheuristicType, metaheuristicParameters, allLoopsData[0]);
        }

        //private MetaheuristicResult CalculateAverageForAllRunsOfTheAlgorithm(List<MetaheuristicResult> dataList)
        //{
        //    for (int _generationsCounter = 0; _; _generationsCounter++)
        //    {
        //        #region Get GA MetaheuristicResult

        //        double averageBestFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListGA);
        //        double averageAverageFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.AverageFitnessListGA);
        //        double averageWorstFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.WorstFitnessListGA);

        //        #endregion Get GA MetaheuristicResult

        //        #region Get TS data

        //        double averageBestFitnessTS = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListTS);

        //        #endregion Get TS data

        //        #region Get SA data

        //        double averageBestFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);
        //        double averageBestNeighborFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);

        //        #endregion Get SA data

        //        #region Save GA

        //        allAlgorithmsAverage.SaveData(averageBestFitnessGA);
        //        allAlgorithmsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
        //        allAlgorithmsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);

        //        #endregion Save GA

        //        #region Save TS

        //        allAlgorithmsAverage.SaveBestFitnessForTS(averageBestFitnessTS);

        //        #endregion Save TS

        //        #region Save SA

        //        allAlgorithmsAverage.SaveBestFitnessForSA(averageBestFitnessSA);
        //        allAlgorithmsAverage.SaveBestNeighborFitnessForSA(averageBestFitnessSA);

        //        #endregion Save SA
        //    }

        //    return allAlgorithmsAverage;
        //}
    }
}