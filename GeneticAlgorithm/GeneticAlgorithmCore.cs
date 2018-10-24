//#define OnlyPositiveFitness
#undef OnlyPositiveFitness

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

        public GenerationsStatistics StartTabuSearch()
        {
            GenerationsStatistics generationsStatistics = new GenerationsStatistics();

            TabuSearch tabuSearch = new TabuSearch();

            Individual best = new Individual(Population.CreateRandomIndividual());
            Individual current = best;

            tabuSearch.AddToTabuList(current.Places);

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                List<int[]> neighbors = tabuSearch.GetNeighbors(current, TabuSearchParameters.NumberOfNeighbors);
                //CountFitness();
                foreach (var candidate in neighbors)
                {
                    Individual tmpCandidate = new Individual(candidate);
                    if ((!tabuSearch.IsContains(candidate)) && (tmpCandidate.Fitness > current.Fitness))
                        current = tmpCandidate;
                }
                if (current.Fitness > best.Fitness)
                    best = current;

                tabuSearch.AddToTabuList(current.Places);

                generationsStatistics.SaveBestFitnessForTS(best.Fitness);
            }

            return generationsStatistics;
        }

        private List<GenerationsStatistics> RunAllAlgorithms()
        {
            List<GenerationsStatistics> allAlgorithmsResults = new List<GenerationsStatistics>();

            for (_generationsCounter = 0; _exploringStopCondition; _generationsCounter++)
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

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                #region Get GA Data

                double averageBestFitnessGA = CountAverageBestFitnessGA(dataList, _generationsCounter);
                double averageAverageFitnessGA = CountAverageAverageFitnessGaGetFitnessGa(dataList, _generationsCounter);
                double averageWorstFitnessGA = CountAverageWorstFitnessGA(dataList, _generationsCounter);

                #endregion Get GA Data

                #region Get TS data

                double averageBestFitnessTS = CountAverageBestFitnessTS(dataList, _generationsCounter);

                #endregion Get TS data

                #region Save GA

                allAlgorithmsAverage.SaveGenerationCounter(_generationsCounter + 1);
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
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} TS and GA result.csv");

            List<GenerationsStatistics> allAlgorithmsResults = RunAllAlgorithms();
            GenerationsStatistics allAlgorithmsAverage = CalculateAllAlgorithmsAverage(allAlgorithmsResults);

            toFileLogger.LogToFile(allAlgorithmsAverage);

            //TODO:
            //CountStandardDeviation
        }

#if OnlyPositiveFitness
         /// <summary>
        /// Only for positive fitness
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#endif

        private double CountAverageBestFitnessTS(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            int counter = 0;
            foreach (var item in list)
            {
#if OnlyPositiveFitness
                if (item.BestFitnessListTS[index] > 0)
                {
#endif

                sum += item.BestFitnessListTS[index];
                counter++;
#if OnlyPositiveFitness
                }
#endif
            }

            return counter > 0 ? sum / counter : 0;
        }

#if OnlyPositiveFitness
        /// <summary>
        /// Only for positive fitness
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#endif

        private double CountAverageBestFitnessGA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            double itemFitness;
            int counter = 0;
            foreach (var item in list)
            {
                itemFitness = item.BestFitnessListGA[index];
#if OnlyPositiveFitness
    if (itemFitness > 0)
                {
#endif
                sum += itemFitness;
                counter++;
#if OnlyPositiveFitness
                }
#endif
            }

            return counter > 0 ? sum / counter : 0;
        }

#if OnlyPositiveFitness
        /// <summary>
        /// Only for positive items
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#endif

        private double CountAverageAverageFitnessGaGetFitnessGa(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            int counter = 0;
            foreach (var item in list)
            {
#if OnlyPositiveFitness
                if (item.AverageFitnessListGA[index] > 0)
                {
#endif

                sum += item.AverageFitnessListGA[index];
                counter++;
#if OnlyPositiveFitness
             }
#endif
            }
            return counter > 0 ? sum / counter : 0;
        }

#if OnlyPositiveFitness
         /// <summary>
        /// Only for positive items
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
#endif

        private double CountAverageWorstFitnessGA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            int counter = 0;
            foreach (var item in list)
            {
#if OnlyPositiveFitness
                if (item.WorstFitnessListGA[index] > 0)
                {
#endif
                sum += item.WorstFitnessListGA[index];
                counter++;
#if OnlyPositiveFitness
                }
#endif
            }
            return counter > 0 ? sum / counter : 0;
        }

        public GenerationsStatistics RunGeneticAlgorithm()
        {
            GenerationsStatistics generationsStatistics = new GenerationsStatistics();

            CreatePopulation();
            CountFitness();

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                SaveDataForGA(_generationsCounter + 1, generationsStatistics);
            }

            return generationsStatistics;
        }

        private void SaveDataForGA(int generationsCounter, GenerationsStatistics generationsStatistics)
        {
            generationsStatistics.SaveGenerationCounter(generationsCounter);
            generationsStatistics.SaveBestFitnessForGA(Population.GetBestFitness());
            generationsStatistics.SaveAverageFitnessForGA(Population.GetAverageFitness());
            generationsStatistics.SaveWorstFitnessForGA(Population.GetWorstFitness());
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