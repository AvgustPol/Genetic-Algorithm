﻿namespace GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm
{
    public class GeneticAlgorithm : Metaheuristic
    {
        public static GeneticAlgorithmParameters GeneticAlgorithmParameters;

        public Population Population { get; set; }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters)
        {
            GeneticAlgorithmParameters = (GeneticAlgorithmParameters)algorithmParameters;
            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();

            CreatePopulation();
            CountFitness();

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                SaveFitnessData(metaheuristicResult);
            }

            return metaheuristicResult;
        }

        public void SaveFitnessData(MetaheuristicResult metaheuristicResult)
        {
            metaheuristicResult.SaveBestFitnessForCurrentGeneration(Population.GetBestFitness());
            metaheuristicResult.SaveAverageFitnessForCurrentGeneration(Population.GetAverageFitness());
            metaheuristicResult.SaveWorstFitnessForCurrentGeneration(Population.GetWorstFitness());
        }

        private void CreatePopulation()
        {
            Population = new Population();
            Population.CreatePopulationIndividuals();
        }

        /// <summary>
        /// Counts fitness for each element in a population
        /// </summary>
        /// <param name="population"></param>
        private void CountFitness()
        {
            Population.CountFitnessForTheEntirePopulation();
        }

        private void Mutate()
        {
            Population.Mutate();
        }

        private void SelectAndCross()
        {
            Population.SelectAndCross();
        }

        //protected override object CalculateAverageForAllRunsOfTheAlgorithm(object allAlgorithmsResult)
        //{
        //    MetaheuristicResult allMetaheuristicsAverage = new MetaheuristicResult();

        //    #region Get GA MetaheuristicResult

        //    double averageBestFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitness);
        //    double averageAverageFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.AverageFitness);
        //    double averageWorstFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.WorstFitness);

        //    #endregion Get GA MetaheuristicResult

        //    #region Save GA

        //    allMetaheuristicsAverage.SaveData(averageBestFitnessGA);
        //    allMetaheuristicsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
        //    allMetaheuristicsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);

        //    #endregion Save GA

        //    return allMetaheuristicsAverage;
        //}
    }
}