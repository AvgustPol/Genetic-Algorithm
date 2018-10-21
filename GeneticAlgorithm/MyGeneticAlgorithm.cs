using StatisticsCounter;

namespace GeneticAlgorithm
{
    public class MyGeneticAlgorithm
    {
        //private bool _stopCondition; //=> TODO write stopCondition logics
        public Population Population { get; set; }

        public void StartGeneticAlgorithm()
        {
            int generationsCounter = 1;
            CreatePopulation();
            CountFitness();

            //TODO : add new ToFileLoggerAccumulator
            ToFileLogger toFileLogger = new ToFileLogger($"trivial_0 result.csv");

            //while (!_stopCondition)
            while (generationsCounter < GeneticAlgorithmParameters.StopConditionGenerationNumbers)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                LogGeneration(generationsCounter, toFileLogger);

                generationsCounter++;
            }

            LogAllGenerationsToFile(toFileLogger);

            //return the_best_solution;
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