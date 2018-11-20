using DataModel;
using GeneticAlgorithm.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithm.Metaheuristics.SimulatedAnnealing;
using GeneticAlgorithm.Metaheuristics.TabuSearch;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    /// <summary>
    /// Metaheuristic
    /// </summary>
    public class AlgorithmCore
    {
        private bool _algoritmStopCondition => _generationsCounter < GlobalParameters.AlgorithmStopCondition;
        private bool _exploringStopCondition => _generationsCounter < GlobalParameters.ExploringAlgorithmStopCondition;
        private int _generationsCounter { get; set; }

        /// <summary>
        /// Run genetic algorithm
        /// </summary>
        /// <returns></returns>
        public LoopData<double> RunGA()
        {
            LoopData<double> loopData = new LoopData<double>();

            CreatePopulation();
            CountFitness();

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                SaveDataForGA(_generationsCounter + 1, loopData);
            }

            return loopData;
        }

        public void RunAnyAlgorithm(GlobalParameters.AlgorithmType algorithmType)
        {
            ToFileLogger toFileLogger = new ToFileLogger($"{GlobalParameters.FileName} {algorithmType} result.csv");

            List<LoopData<double>> allLoopsData = new List<LoopData<double>>(GlobalParameters.ExploringAlgorithmStopCondition);
            for (int i = 0; i < GlobalParameters.ExploringAlgorithmStopCondition; i++)
            {
                LoopData<double> algorithmResult = null;
                switch (algorithmType)
                {
                    case GlobalParameters.AlgorithmType.GA:
                        algorithmResult = RunGA();
                        break;

                    case GlobalParameters.AlgorithmType.SA:
                        algorithmResult = RunSA();
                        break;

                    case GlobalParameters.AlgorithmType.TS:
                        algorithmResult = RunTS();
                        break;

                    default:

                        break;
                }
                allLoopsData.Add(algorithmResult);
            }

            LoopData<double> allAlgorithmsAverage = CalculateAllAlgorithmsAverage(allLoopsData);

            //TODO:
            //CountStandardDeviation

            toFileLogger.LogToFile(algorithmType, allAlgorithmsAverage);
        }

        /// <summary>
        /// Run Simulated Annealing
        /// </summary>
        /// <returns></returns>
        private LoopData<double> RunSA()
        {
            _generationsCounter = 0;

            LoopData<double> loopData = new LoopData<double>();

            double currentTemperature = SimulatedAnnealingParameters.InitializeTemperature;
            Individual best = new Individual(Population.CreateRandomIndividual());

            List<int[]> neighbors;

            double bestAlgorithmFitness = best.Fitness;
            double bestNeighborFitness = best.Fitness;

            do
            {
                neighbors = NeighborsGenerator.GetNeighbors(best, TabuSearchParameters.NumberOfNeighbors);

                foreach (var neighborsRoad in neighbors)
                {
                    Individual neighbor = new Individual(neighborsRoad);
                    if (neighbor.Fitness > best.Fitness)
                    {
                        best = neighbor;
                        bestNeighborFitness = neighbor.Fitness;
                    }
                    else
                        //тут понижаем лушего!
                        SimulatedAnnealing.TryAvoidLocalOptimum(ref best, ref neighbor, currentTemperature);
                }

                if (bestNeighborFitness > bestAlgorithmFitness)
                {
                    bestAlgorithmFitness = bestNeighborFitness;
                }

                loopData.SaveBestNeighborFitnessForSA(bestNeighborFitness);
                loopData.SaveBestFitnessForSA(bestAlgorithmFitness);
                loopData.SaveTemperatureForSA(currentTemperature);

                SimulatedAnnealing.DecreaseTemperature(ref currentTemperature, ++_generationsCounter);
                //if (currentTemperature < 0.5)
                //    break;
            } while (_algoritmStopCondition);

            return loopData;
        }

        /// <summary>
        /// Run Tabu Search
        /// </summary>
        /// <returns></returns>
        private LoopData<double> RunTS()
        {
            LoopData<double> loopData = new LoopData<double>();
            List<int[]> neighbors;
            TabuSearch tabuSearch = new TabuSearch();

            Individual best = new Individual(Population.CreateRandomIndividual());
            Individual current = best;

            //best fount
            //current -> best Neighbor

            double bestNeighborFitness = best.Fitness;
            double bestAlgorithmFitness = best.Fitness;

            tabuSearch.AddToTabuList(current.Places);

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                neighbors = NeighborsGenerator.GetNeighbors(current, TabuSearchParameters.NumberOfNeighbors);

                foreach (var candidate in neighbors)
                {
                    if (!tabuSearch.IsContains(candidate))
                    {
                        Individual tmpCandidate = new Individual(candidate);
                        if (tmpCandidate.Fitness > current.Fitness)
                            current = tmpCandidate;
                    }
                }
                bestNeighborFitness = current.Fitness;

                if (bestNeighborFitness > bestAlgorithmFitness)
                {
                    bestAlgorithmFitness = bestNeighborFitness;
                }

                tabuSearch.AddToTabuList(current.Places);

                loopData.SaveBestNeighborFitnessForTS(bestNeighborFitness);
                loopData.SaveBestFitnessForTS(bestAlgorithmFitness);
            }

            return loopData;
        }

        private LoopData<double> CalculateAllAlgorithmsAverage(List<LoopData<double>> dataList)
        {
            LoopData<double> allAlgorithmsAverage = new LoopData<double>();

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                #region Get GA LoopData

                double averageBestFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListGA);
                double averageAverageFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.AverageFitnessListGA);
                double averageWorstFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.WorstFitnessListGA);

                #endregion Get GA LoopData

                #region Get TS data

                double averageBestFitnessTS = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListTS);

                #endregion Get TS data

                #region Get SA data

                double averageBestFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);
                double averageBestNeighborFitnessSA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitnessListSA);

                #endregion Get SA data

                #region Save GA

                allAlgorithmsAverage.SaveGenerationCounter(_generationsCounter + 1);

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