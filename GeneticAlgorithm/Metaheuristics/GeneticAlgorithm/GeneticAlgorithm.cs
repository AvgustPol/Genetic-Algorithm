namespace GeneticAlgorithm.Metaheuristics.GeneticAlgorithm
{
    public class GeneticAlgorithm : Metaheuristic
    {
        public Population Population { get; set; }

        public GeneticAlgorithm()
        {
        }

        public override object Run(object algorithmParameters)
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

        private void SaveDataForGA(int generationsCounter, LoopData<double> allGenerationsStatistics)
        {
            //log ("GA", "BestInd", ResultList)
            allGenerationsStatistics.SaveData(Population.GetBestFitness(), LoopData<double>.GaDataType.Best, allGenerationsStatistics.ListBest);
            allGenerationsStatistics.SaveData(Population.GetAverageFitness(), LoopData<double>.GaDataType.Avg, allGenerationsStatistics.ListAvg);
            allGenerationsStatistics.SaveData(Population.GetWorstFitness(), LoopData<double>.GaDataType.Worst, allGenerationsStatistics.ListOther);
        }
    }
}