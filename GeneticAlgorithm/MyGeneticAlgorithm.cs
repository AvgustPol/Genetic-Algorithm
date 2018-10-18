using System;

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
            CountFitness(Population);

            while (!_stopCondition)
            {
                SelectAndCross(Population);
                Mutate(Population);
                CountFitness(Population);
                //localCounter++;
            }

            //return the_best_solution;
        }

        /// <summary>
        /// Counts fitness for each element in a population
        /// </summary>
        /// <param name="population"></param>
        private void CountFitness(Population population)
        {
            population.CountFitnessForTheEntirePopulation();
        }

        private void CreatePopulation()
        {
            Population = new Population(GeneticAlgorithmParameters.Dimension);
        }

        private void SelectAndCross(Population population)
        {
            population.SelectAndCross();
        }

        private void Mutate(Population population)
        {
            //TODO - compare foreach and for
            for (int i = 0; i < population.Individuals.Count; i++)
            {
                //population.
                throw new NotImplementedException();
                //population.Individuals.ElementAt(i).Mutate();
            }

            //TODO - compare foreach and for
            //for (Individual individual in population.Individuals.Count)
            //{
            //    individual.CountFitness();
            //}
        }
    }
}