using DataModel;
using Loader;
using System;
using System.Linq;

namespace GeneticAlgorithm
{
    public class MyGeneticAlgorithm
    {
        private bool _stopCondition; //=> TODO write stopCondition logics
        public Population Population { get; set; }

        private readonly FileLoader _fileLoader;
        private readonly DataContainer _container;

        public MyGeneticAlgorithm()
        {
            _fileLoader = new FileLoader();

            _container = _fileLoader.CreateDataContainer();
        }

        public void StartGeneticAlgorithm()
        {
            //int localCounter = 0;
            CreatePopulation();
            CountFitness(Population);

            while (!_stopCondition)
            {
                SelectThatCross(Population);
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

        private void SelectThatCross(Population population)
        {
        }

        private void Mutate(Population population)
        {
            //TODO - compare foreach and for
            for (int i = 0; i < population.Individuals.Count; i++)
            {
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