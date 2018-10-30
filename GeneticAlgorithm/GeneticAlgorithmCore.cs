using DataModel;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithmCore
    {
        private int _generationsCounter { get; set; }

        private bool _algoritmStopCondition => _generationsCounter < GlobalParameters.AlgorithmStopCondition;
        private bool _exploringStopCondition => _generationsCounter < GlobalParameters.ExploringAlgorithmStopCondition;
        public Population Population { get; set; }

        public AllGenerationsStatistics RunSimulatedAnnealing()
        {
            _generationsCounter = 0;

            AllGenerationsStatistics allGenerationsStatistics = new AllGenerationsStatistics();

            double currentTemperature = SimulatedAnnealingParameters.InitializeTemperature;
            Individual best = new Individual(Population.CreateRandomIndividual());
            List<int[]> neighbors;

            do
            {
                neighbors = NeighborsGenerator.GetNeighbors(best, TabuSearchParameters.NumberOfNeighbors);

                foreach (var candidate in neighbors)
                {
                    Individual tmpCandidate = new Individual(candidate);
                    if (tmpCandidate.Fitness > best.Fitness)
                        best = tmpCandidate;
                    else
                        //тут понижаем лушего!
                        SimulatedAnnealing.TryAvoidLocalOptimum(ref best, ref tmpCandidate, currentTemperature);
                }

                allGenerationsStatistics.SaveBestFitnessForSA(best.Fitness);

                SimulatedAnnealing.DecreaseTemperature(ref currentTemperature, ++_generationsCounter);
            } while (_algoritmStopCondition);

            return allGenerationsStatistics;
        }

        public AllGenerationsStatistics RunTabuSearch()
        {
            AllGenerationsStatistics allGenerationsStatistics = new AllGenerationsStatistics();
            List<int[]> neighbors;
            TabuSearch tabuSearch = new TabuSearch();

            Individual best = new Individual(Population.CreateRandomIndividual());
            Individual current = best;

            //best fount
            //current -> best Neighbor

            tabuSearch.AddToTabuList(current.Places);

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                neighbors = NeighborsGenerator.GetNeighbors(current, TabuSearchParameters.NumberOfNeighbors);

                foreach (var candidate in neighbors)
                {
                    Individual tmpCandidate = new Individual(candidate);
                    if ((!tabuSearch.IsContains(candidate)) && (tmpCandidate.Fitness > current.Fitness))
                        current = tmpCandidate;
                }
                if (current.Fitness > best.Fitness)
                    best = current;

                tabuSearch.AddToTabuList(current.Places);

                allGenerationsStatistics.SaveBestFitnessForTS(best.Fitness);
            }

            return allGenerationsStatistics;
        }

        private List<AllGenerationsStatistics> RunAllAlgorithmsAndGetResult()
        {
            List<AllGenerationsStatistics> allAlgorithmsResults = new List<AllGenerationsStatistics>();

            for (_generationsCounter = 0; _exploringStopCondition; _generationsCounter++)
            {
                AllGenerationsStatistics allGenerationsStatistics = new AllGenerationsStatistics();

                allGenerationsStatistics.AddGAData(RunGeneticAlgorithm());
                allGenerationsStatistics.AddTabuSearchData(RunTabuSearch());
                allGenerationsStatistics.AddSimulatedAnnealingData(RunSimulatedAnnealing());

                allAlgorithmsResults.Add(allGenerationsStatistics);
            }

            return allAlgorithmsResults;
        }

        private AllGenerationsStatistics CalculateAllAlgorithmsAverage(List<AllGenerationsStatistics> dataList)
        {
            AllGenerationsStatistics allAlgorithmsAverage = new AllGenerationsStatistics();

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                #region Get GA Data

                double averageBestFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListGA);
                double averageAverageFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.AverageFitnessListGA);
                double averageWorstFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.WorstFitnessListGA);

                #endregion Get GA Data

                #region Get TS data

                double averageBestFitnessTS = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListTS);

                #endregion Get TS data

                #region Get SA data

                double averageBestFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);

                #endregion Get SA data

                #region Save GA

                allAlgorithmsAverage.SaveGenerationCounter(_generationsCounter + 1);

                allAlgorithmsAverage.SaveBestFitnessForGA(averageBestFitnessGA);
                allAlgorithmsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
                allAlgorithmsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);

                #endregion Save GA

                #region Save TS

                allAlgorithmsAverage.SaveBestFitnessForTS(averageBestFitnessTS);

                #endregion Save TS

                #region Save SA

                allAlgorithmsAverage.SaveBestFitnessForSA(averageBestFitnessSA);

                #endregion Save SA
            }

            return allAlgorithmsAverage;
        }

        /// <summary>
        /// Runs Only simulated annealing
        /// </summary>
        public void RunOnlySA()
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} Simulated annealing result.csv");

            AllGenerationsStatistics simulatedAnnealingResult = RunSimulatedAnnealing();
            //AllGenerationsStatistics allAlgorithmsAverage = CalculateAllAlgorithmsAverage(allAlgorithmsResults);

            toFileLogger.LogSaToFile(simulatedAnnealingResult);
        }

        /// <summary>
        /// Runs Only Tabu Search
        /// </summary>
        public void RunOnlyTS()
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} all algorithms result.csv");

            AllGenerationsStatistics tabuSearchResult = RunTabuSearch();
            //AllGenerationsStatistics allAlgorithmsAverage = CalculateAllAlgorithmsAverage(allAlgorithmsResults);

            toFileLogger.LogToFile(tabuSearchResult);
        }

        public void RunAllAlgorithms()
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} all algorithms result.csv");

            List<AllGenerationsStatistics> allAlgorithmsResults = RunAllAlgorithmsAndGetResult();
            AllGenerationsStatistics allAlgorithmsAverage = CalculateAllAlgorithmsAverage(allAlgorithmsResults);

            toFileLogger.LogToFile(allAlgorithmsAverage);

            //TODO:
            //CountStandardDeviation
        }

        public AllGenerationsStatistics RunGeneticAlgorithm()
        {
            AllGenerationsStatistics allGenerationsStatistics = new AllGenerationsStatistics();

            CreatePopulation();
            CountFitness();

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                SaveDataForGA(_generationsCounter + 1, allGenerationsStatistics);
            }

            return allGenerationsStatistics;
        }

        private void SaveDataForGA(int generationsCounter, AllGenerationsStatistics allGenerationsStatistics)
        {
            allGenerationsStatistics.SaveGenerationCounter(generationsCounter);

            allGenerationsStatistics.SaveBestFitnessForGA(Population.GetBestFitness());
            allGenerationsStatistics.SaveAverageFitnessForGA(Population.GetAverageFitness());
            allGenerationsStatistics.SaveWorstFitnessForGA(Population.GetWorstFitness());
        }

        /// <summary>
        /// Counts fitness for each element in a population
        /// </summary>
        /// <param name="population"></param>
        private void CountFitness()
        {
            Population.CountFitnessForTheEntirePopulation();
        }

        private void CreatePopulation()
        {
            Population = new Population();
            Population.CreatePopulationIndividuals();
        }

        private void SelectAndCross()
        {
            Population.SelectAndCross();
        }

        private void Mutate()
        {
            Population.Mutate();
        }
    }
}