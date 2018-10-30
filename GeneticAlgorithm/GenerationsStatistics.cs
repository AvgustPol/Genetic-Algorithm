using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class GenerationsStatistics
    {
        public List<double> GenerationCounterList;

        #region Genetic Algorithm

        public List<double> BestFitnessListGA { get; set; }
        public List<double> WorstFitnessListGA { get; set; }
        public List<double> AverageFitnessListGA { get; set; }

        #endregion Genetic Algorithm

        #region Tabu Search

        public List<double> BestFitnessListTS { get; set; }

        #endregion Tabu Search

        #region SimulatedAnnealing

        public List<double> BestFitnessListSA { get; set; }
        public List<double> BestNeighborFitnessListSA { get; set; }

        #endregion SimulatedAnnealing

        public GenerationsStatistics()
        {
            GenerationCounterList = new List<double>();
            BestFitnessListGA = new List<double>();
            AverageFitnessListGA = new List<double>();
            WorstFitnessListGA = new List<double>();

            BestFitnessListTS = new List<double>();

            BestFitnessListSA = new List<double>();
            BestNeighborFitnessListSA = new List<double>();
        }

        public void AddGAData(GenerationsStatistics generationsStatistics)
        {
            BestFitnessListGA = generationsStatistics.BestFitnessListGA;
            WorstFitnessListGA = generationsStatistics.WorstFitnessListGA;
            AverageFitnessListGA = generationsStatistics.AverageFitnessListGA;
        }

        public void AddTabuSearchData(GenerationsStatistics generationsStatistics)
        {
            BestFitnessListTS = generationsStatistics.BestFitnessListTS;
        }

        public void AddSimulatedAnnealingData(GenerationsStatistics generationsStatistics)
        {
            BestFitnessListSA = generationsStatistics.BestFitnessListSA;
            BestNeighborFitnessListSA = generationsStatistics.BestNeighborFitnessListSA;
        }

        /// <summary>
        /// Saves Generation number
        /// </summary>
        /// <param name="counter"></param>
        public void SaveGenerationCounter(double counter)
        {
            GenerationCounterList.Add(counter);
        }

        /// <summary>
        /// Saves best fitness for current generation
        /// </summary>
        /// <param name="bestFitness"></param>
        public void SaveBestFitnessForGA(double bestFitness)
        {
            BestFitnessListGA.Add(bestFitness);
        }

        /// <summary>
        /// Saves Worst fitness for current generation
        /// </summary>
        /// <param name="worstFitness"></param>
        public void SaveWorstFitnessForGA(double worstFitness)
        {
            WorstFitnessListGA.Add(worstFitness);
        }

        /// <summary>
        /// Saves Average fitness for current generation
        /// </summary>
        /// <param name="averageFitness"></param>
        public void SaveAverageFitnessForGA(double averageFitness)
        {
            AverageFitnessListGA.Add(averageFitness);
        }

        /// <summary>
        /// Saves best fitness for current generation
        /// </summary>
        /// <param name="bestFitness"></param>
        public void SaveBestFitnessForTS(double bestFitness)
        {
            BestFitnessListTS.Add(bestFitness);
        }

        /// <summary>
        /// Saves best fitness for current generation for Simulated Annealing
        /// </summary>
        /// <param name="bestFitness"></param>
        public void SaveBestFitnessForSA(double bestFitness)
        {
            BestFitnessListSA.Add(bestFitness);
        }

        /// <summary>
        /// Saves best fitness for current generation for Simulated Annealing
        /// </summary>
        /// <param name="bestFitness"></param>
        public void SaveBestNeighborFitnessListSAForSA(double bestFitness)
        {
            BestNeighborFitnessListSA.Add(bestFitness);
        }
    }
}