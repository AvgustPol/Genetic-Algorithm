using DataModel;
using System.Linq;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private bool _stopCondition; //=> TODO write stopCondition logics
        public Population Population { get; set; }

        private readonly DataContainer _container;

        public void StartGeneticAlgorithm()
        {
            //int localCounter = 0;
            CreatePopulation();
            CountFitness(Population);

            while (!_stopCondition)
            {
                Selection(Population);
                Crossover();
                Mutation(Population);
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
            //TODO - compare foreach and for
            for (int i = 0; i < population.Individuals.Count; i++)
            {
                population.Individuals.ElementAt(i).CountFitness();
            }

            //foreach (Individual individual in population.Individuals)
            //{
            //    individual.CountFitness();
            //}
        }

        private void CreatePopulation()
        {
            Population = new Population(_container.Dimension);
        }

        public void Selection(Population population)
        {
        }

        public void Crossover()
        {
        }

        public void Mutation(Population population)
        {
            //TODO - compare foreach and for
            for (int i = 0; i < population.Individuals.Count; i++)
            {
                population.Individuals.ElementAt(i).Mutate();
            }

            //TODO - compare foreach and for
            //for (Individual individual in population.Individuals.Count)
            //{
            //    individual.CountFitness();
            //}
        }
    }
}