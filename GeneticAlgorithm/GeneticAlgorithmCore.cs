using StatisticsCounter;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithmCore
    {
        private int _generationsCounter { get; set; }

        private bool _algorytmStopCondition => _generationsCounter < GeneticAlgorithmParameters.StopConditionGenerationNumbers;
        private bool _exploringStopCondition => _generationsCounter < GeneticAlgorithmParameters.ExploringStopCondition;
        public Population Population { get; set; }

        public AverageCounter StartTabuSearch()
        {
            AverageCounter averageCounter = new AverageCounter();

            TabuSearch tabuSearch = new TabuSearch();

            ToFileLogger toFileLogger = new ToFileLogger($"trivial_0 result TabuSearch.csv");
            //ToFileLogger toFileLogger = new ToFileLogger($"hard_4.ttp result TabuSearch.csv");

            Individual best = new Individual()
            {
                PermutationPlaces = Population.CreateRandomIndividual()
            };
            Individual current = best;

            tabuSearch.AddToTabuList(current.PermutationPlaces);

            for (_generationsCounter = 1; _algorytmStopCondition; _generationsCounter++)
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

                //toFileLogger.LogToObject(_generationsCounter, best.Fitness, Randomizer.Seed, 0);\
                averageCounter.SaveBestFitnessForTS(best.Fitness);
            }

            //toFileLogger.LogToFile();
            return averageCounter;
        }

        public void Explore()
        {
            List<AverageCounter> dataList = new List<AverageCounter>();

            for (_generationsCounter = 1; _exploringStopCondition; _generationsCounter++)
            {
                AverageCounter averageCounter = new AverageCounter();
                averageCounter.AddGAData(RunGeneticAlgorithm());
                averageCounter.AddTabuSearchData(StartTabuSearch());

                dataList.Add(averageCounter);
            }

            AverageCounter allAlgorithmsAverage = new AverageCounter();

            for (int i = 0; i < ; i++)
            {
                double averageBestFitnessGA = GetAverageBestFitnessGA(dataList, i);
                double averageAverageFitnessGA = GetAverageAverageFitnessGA(dataList, i);
                double averageWorstFitnessGA = GetAverageWorstFitnessGA(dataList, i);

                double averageBestFitnessTS = GetAverageBestFitnessTS(dataList, i);

                allAlgorithmsAverage.SaveGenerationCounter(i + 1);
                allAlgorithmsAverage.SaveBestFitnessForGA(averageBestFitnessGA);
                allAlgorithmsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
                allAlgorithmsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);
                allAlgorithmsAverage.SaveBestFitnessForTS(averageBestFitnessTS);
            }
        }

        private double GetAverageBestFitnessTS(List<AverageCounter> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.BestFitnessListTS[index];
            }
            return sum / list.Count;
        }

        private double GetAverageBestFitnessGA(List<AverageCounter> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.BestFitnessListGA[index];
            }
            return sum / list.Count;
        }

        private double GetAverageAverageFitnessGA(List<AverageCounter> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.AverageFitnessListGA[index];
            }
            return sum / list.Count;
        }

        private double GetAverageWorstFitnessGA(List<AverageCounter> list, int index)
        {
            double sum = 0;
            foreach (var item in list)
            {
                sum += item.WorstFitnessListGA[index];
            }
            return sum / list.Count;
        }

        public AverageCounter RunGeneticAlgorithm()
        {
            AverageCounter averageCounter = new AverageCounter();

            CreatePopulation();
            CountFitness();

            ToFileLogger toFileLogger = new ToFileLogger($"trivial_0 result GA.csv");
            //ToFileLogger toFileLogger = new ToFileLogger($"easy_0 result GA.csv");

            for (_generationsCounter = 1; _algorytmStopCondition; _generationsCounter++)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                SaveDataForGA(_generationsCounter, averageCounter);
                //LogGeneration(_generationsCounter, toFileLogger);
            }

            //LogAllGenerationsToFile(toFileLogger);
            return averageCounter;
        }

        private void SaveDataForGA(int generationsCounter, AverageCounter averageCounter)
        {
            averageCounter.SaveGenerationCounter(generationsCounter);
            averageCounter.SaveBestFitnessForGA(Population.GetBestFitness());
            averageCounter.SaveAverageFitnessForGA(Population.GetAverageFitness());
            averageCounter.SaveWorstFitnessForGA(Population.GetWorstFitness());
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