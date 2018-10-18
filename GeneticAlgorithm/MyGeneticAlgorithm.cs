using System;
using System.Diagnostics;

namespace GeneticAlgorithm
{
    public class MyGeneticAlgorithm
    {
        //private bool _stopCondition; //=> TODO write stopCondition logics
        public Population Population { get; set; }

        public void StartGeneticAlgorithm()
        {
            int localCounter = 0;
            CreatePopulation();
            CountFitness();

            //while (!_stopCondition)
            while (localCounter < 42)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                LogPopulationData();

                localCounter++;
            }

            //return the_best_solution;
        }

        private void LogPopulationData()
        {
            Debug.Write($"Best: {String.Format("{0:0.00}", Population.GetBestFitness()),-15 }");
            Debug.Write($"Average: {String.Format("{0:0.00}", Population.GetAverageFitness()),-15}");
            Debug.WriteLine($"Worst: {String.Format("{0:0.00}", Population.GetWorstFitness()),-15}");
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