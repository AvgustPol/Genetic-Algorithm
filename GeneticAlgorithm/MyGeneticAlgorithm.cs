using StatisticsCounter;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class MyGeneticAlgorithm
    {
        public int _generationsCounter { get; set; }

        private bool _stopCondition => _generationsCounter < GeneticAlgorithmParameters.StopConditionGenerationNumbers;
        public Population Population { get; set; }

        public void StartTabuSearch()
        {
            TabuSearch tabuSearch = new TabuSearch();
            _generationsCounter = 1;
            ToFileLogger toFileLogger = new ToFileLogger($"hard_4.ttp result TabuSearch.csv");

            Individual best = new Individual()
            {
                PermutationPlaces = Population.CreateRandomIndividual()
            };
            Individual current = best;

            tabuSearch.AddToTabuList(current.PermutationPlaces);

            while (_stopCondition)
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

                toFileLogger.LogToObject(_generationsCounter, best.Fitness, Randomizer.Seed, 0);
                _generationsCounter++;
            }

            toFileLogger.LogToFile();
        }

        public void StartGeneticAlgorithm()
        {
            _generationsCounter = 1;
            CreatePopulation();
            CountFitness();

            //ToFileLogger toFileLogger = new ToFileLogger($"trivial_0 result GA.csv");
            ToFileLogger toFileLogger = new ToFileLogger($"easy_0 result GA.csv");

            while (_stopCondition)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                LogGeneration(_generationsCounter, toFileLogger);

                _generationsCounter++;
            }

            LogAllGenerationsToFile(toFileLogger);
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