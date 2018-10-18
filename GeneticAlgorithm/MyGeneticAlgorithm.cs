namespace GeneticAlgorithm
{
    public class MyGeneticAlgorithm
    {
        private bool _stopCondition; //=> TODO write stopCondition logics
        public Population Population { get; set; }

        public void StartGeneticAlgorithm()
        {
            //int localCounter = 0;
            CreatePopulation();
            CountFitness();

            while (!_stopCondition)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                LogPopulationData();
                //localCounter++;
            }

            //return the_best_solution;
        }

        private void LogPopulationData()
        {
            throw new System.NotImplementedException();
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