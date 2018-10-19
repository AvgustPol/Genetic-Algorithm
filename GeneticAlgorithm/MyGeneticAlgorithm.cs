using StatisticsCounter;

namespace GeneticAlgorithm
{
    public class MyGeneticAlgorithm
    {
        //private bool _stopCondition; //=> TODO write stopCondition logics
        public Population Population { get; set; }

        public void StartGeneticAlgorithm()
        {
            int generationsCounter = 0;
            CreatePopulation();
            CountFitness();

            ToFileLogger toFileLogger = new ToFileLogger();

            //while (!_stopCondition)
            while (generationsCounter < GeneticAlgorithmParameters.StopConditionGenerationNumbers)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                LogPopulationData(generationsCounter, toFileLogger);

                generationsCounter++;
            }

            //return the_best_solution;
        }

        private void LogPopulationData(int generationsCounter, ToFileLogger toFileLogger)
        {
            toFileLogger.Log(generationsCounter, Population.GetBestFitness(), Population.GetAverageFitness(), Population.GetWorstFitness());
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