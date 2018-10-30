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

        public GenerationsStatistics StartSimulatedAnnealing()
        {
            _generationsCounter = 0;

            GenerationsStatistics generationsStatistics = new GenerationsStatistics();

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

                generationsStatistics.SaveBestFitnessForSA(best.Fitness);

                SimulatedAnnealing.DecreaseTemperature(ref currentTemperature, ++_generationsCounter);
            } while (_algoritmStopCondition);

            return generationsStatistics;
        }

        public GenerationsStatistics StartTabuSearch()
        {
            GenerationsStatistics generationsStatistics = new GenerationsStatistics();
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

                generationsStatistics.SaveBestFitnessForTS(best.Fitness);
            }

            return generationsStatistics;
        }

        private List<GenerationsStatistics> RunAllAlgorithmsAndGetResult()
        {
            List<GenerationsStatistics> allAlgorithmsResults = new List<GenerationsStatistics>();

            for (_generationsCounter = 0; _exploringStopCondition; _generationsCounter++)
            {
                GenerationsStatistics generationsStatistics = new GenerationsStatistics();
                generationsStatistics.AddGAData(RunGeneticAlgorithm());
                generationsStatistics.AddTabuSearchData(StartTabuSearch());
                generationsStatistics.AddSimulatedAnnealingData(StartSimulatedAnnealing());

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
                double averageAverageFitnessGA = 0; //CountAverageAverageFitnessGaGetFitnessGa(dataList, _generationsCounter);
                double averageWorstFitnessGA = CountAverageWorstFitnessGA(dataList, _generationsCounter);

                #endregion Get GA Data

                #region Get TS data

                double averageBestFitnessTS = CountAverageBestFitnessTS(dataList, _generationsCounter);

                #endregion Get TS data

                #region Get SA data

                double averageBestFitnessSA = CountAverageBestFitnessSA(dataList, _generationsCounter);

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

        public void RunAllAlgorithms()
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} all algorithms result.csv");

            List<GenerationsStatistics> allAlgorithmsResults = RunAllAlgorithmsAndGetResult();
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

        private double CountAverageBestFitnessSA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            int counter = 0;
            foreach (var item in list)
            {
                sum += item.BestFitnessListSA[index];
                counter++;
            }

            return counter > 0 ? sum / counter : 0;
        }

        private double CountAverageBestFitnessGA(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            double itemFitness;
            int counter = 0;
            foreach (var item in list)
            {
                itemFitness = item.BestFitnessListGA[index];
                sum += itemFitness;
                counter++;
            }

            return counter > 0 ? sum / counter : 0;
        }

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