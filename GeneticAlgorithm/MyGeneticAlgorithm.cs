using System.Diagnostics;
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

            //ExcelWorker excel = new ExcelWorker();

            //while (!_stopCondition)
            while (generationsCounter < GeneticAlgorithmParameters.StopConditionGenerationNumbers)
            {
                SelectAndCross();
                Mutate();
                CountFitness();
                //LogPopulationData(generationsCounter, excel);
                LogPopulationData(generationsCounter);

                generationsCounter++;
            }

            //return the_best_solution;
        }

        private void LogPopulationData(int generationsCounter)
        {
            //excel.AddCellToWorksheetIntoColumnsABCD(generationsCounter, Population.GetBestFitness(), Population.GetAverageFitness(), Population.GetWorstFitness());

            //Debug.Write($"Best: {String.Format("{0:0.00}", Population.GetBestFitness()),-15 }");
            //Debug.Write($"Average: {String.Format("{0:0.00}", Population.GetAverageFitness()),-15}");
            //Debug.WriteLine($"Worst: {String.Format("{0:0.00}", Population.GetWorstFitness()),-15}");
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