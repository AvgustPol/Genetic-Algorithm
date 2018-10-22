using StatisticsCounter;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithmCore
    {
        private int _generationsCounter { get; set; }

        private bool _algoritmStopCondition => _generationsCounter < GlobalParameters.AlgorithmStopCondition;
        private bool _exploringStopCondition => _generationsCounter < GlobalParameters.ExploringAlgorythmStopCondition;
        public Population Population { get; set; }

        public GenerationsStatistics StartTabuSearch()
        {
            GenerationsStatistics generationsStatistics = new GenerationsStatistics();

            TabuSearch tabuSearch = new TabuSearch();

            Individual best = new Individual()
            {
                PermutationPlaces = Population.CreateRandomIndividual()
            };
            Individual current = best;

            tabuSearch.AddToTabuList(current.PermutationPlaces);

            for (_generationsCounter = 1; _algoritmStopCondition; _generationsCounter++)
            {
                List<int[]> neighbors = tabuSearch.GetNeighbors(current, TabuSearchParameters.NumberOfNeighbors);
                CountFitness();
                foreach (var candidate in neighbors)
                {
                    Individual tmpCandidate = new Individual(candidate);
                    if ((!tabuSearch.IsContains(candidate)) && (tmpCandidate.Fitness > current.Fitness))
                        current = tmpCandidate;
                }
                if (current.Fitness > best.Fitness)
                    best = current;

                tabuSearch.AddToTabuList(current.PermutationPlaces);

                generationsStatistics.SaveBestFitnessForTS(best.Fitness);
            }

            return generationsStatistics;
        }

        private List<GenerationsStatistics> RunAllAlgorithms()
        {
            List<GenerationsStatistics> allAlgorithmsResults = new List<GenerationsStatistics>();

            for (_generationsCounter = 1; _exploringStopCondition; _generationsCounter++)
            {
                GenerationsStatistics generationsStatistics = new GenerationsStatistics();
                generationsStatistics.AddGAData(RunGeneticAlgorithm());
                generationsStatistics.AddTabuSearchData(StartTabuSearch());

                allAlgorithmsResults.Add(generationsStatistics);
            }

            return allAlgorithmsResults;
        }

        private GenerationsStatistics CalculateAllAlgorithmsAverage(List<GenerationsStatistics> dataList)
        {
            GenerationsStatistics allAlgorithmsAverage = new GenerationsStatistics();

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                #region Get GA Data

                double averageBestFitnessGA = GetAverageBestFitnessGA(dataList, i);
                double averageAverageFitnessGA = GetAverageAverageFitnessGA(dataList, i);
                double averageWorstFitnessGA = GetAverageWorstFitnessGA(dataList, i);

                #endregion Get GA Data

                #region Get TS data

                double averageBestFitnessTS = GetAverageBestFitnessTS(dataList, i);

                #endregion Get TS data

                #region Save GA

                allAlgorithmsAverage.SaveGenerationCounter(i + 1);
                allAlgorithmsAverage.SaveBestFitnessForGA(averageBestFitnessGA);
                allAlgorithmsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
                allAlgorithmsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);

                #endregion Save GA

                #region Save TS

                allAlgorithmsAverage.SaveBestFitnessForTS(averageBestFitnessTS);

                #endregion Save TS
            }

            return allAlgorithmsAverage;
        }

        public void Explore()
        {
            ToFileLogger toFileLogger = new ToFileLogger($"trivial_0 result.csv");

            List<GenerationsStatistics> allAlgorithmsResults = RunAllAlgorithms();
        }

        private double GetAverageBestFitnessTS(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.BestFitnessListTS[index];
            }
            return sum / list.Count;
        }

        private double GetAverageBestFitnessGA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.BestFitnessListGA[index];
            }
            return sum / list.Count;
        }

        private double GetAverageAverageFitnessGA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.AverageFitnessListGA[index];
            }
            return sum / list.Count;
        }

        private double GetAverageWorstFitnessGA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.WorstFitnessListGA[index];
            }
            return sum / list.Count;
        }

        public GenerationsStatistics RunGeneticAlgorithm()
        {
            GenerationsStatistics generationsStatistics = new GenerationsStatistics();

            CreatePopulation();
            CountFitness();

            ToFileLogger toFileLogger = new ToFileLogger($"trivial_0 result GA.csv");
            //ToFileLogger toFileLogger = new ToFileLogger($"easy_0 result GA.csv");

            for (_generationsCounter = 1; _algoritmStopCondition; _generationsCounter++)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                SaveDataForGA(_generationsCounter, generationsStatistics);
                //LogGeneration(_generationsCounter, toFileLogger);
            }

            //LogAllGenerationsToFile(toFileLogger);
            return generationsStatistics;
        }

        private void SaveDataForGA(int generationsCounter, GenerationsStatistics generationsStatistics)
        {
            generationsStatistics.SaveGenerationCounter(generationsCounter);
            generationsStatistics.SaveBestFitnessForGA(Population.GetBestFitness());
            generationsStatistics.SaveAverageFitnessForGA(Population.GetAverageFitness());
            generationsStatistics.SaveWorstFitnessForGA(Population.GetWorstFitness());
        }

        private void LogAllGenerationsToFile(ToFileLogger toFileLogger)
        {
            toFileLogger.LogToFile();
        }

        private void LogGeneration(int generationsCounter, ToFileLogger toFileLogger)
        {
            toFileLogger.LogToObject(generationsCounter, Population.GetBestFitness(), Population.GetAverageFitness(), Population.GetWorstFitness());
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