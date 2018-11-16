using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class AllGenerationsStatistics
    {
        public List<double> GenerationCounterList;

        #region Genetic Algorithm

        public List<double> BestFitnessListGA { get; set; }
        public List<double> WorstFitnessListGA { get; set; }
        public List<double> AverageFitnessListGA { get; set; }

        #endregion Genetic Algorithm

        #region Tabu Search

        public List<double> BestFitnessListTS { get; set; }
        public List<double> BestNeighborFitnessListTS { get; set; }

        #endregion Tabu Search

        #region SimulatedAnnealing

        public List<double> BestFitnessListSA { get; set; }
        public List<double> BestNeighborFitnessListSA { get; set; }
        public List<double> SATemperature { get; set; }

        #endregion SimulatedAnnealing

        public AllGenerationsStatistics()
        {
            GenerationCounterList = new List<double>();
            BestFitnessListGA = new List<double>();
            AverageFitnessListGA = new List<double>();
            WorstFitnessListGA = new List<double>();

            BestFitnessListTS = new List<double>();
            BestNeighborFitnessListTS = new List<double>();

            BestFitnessListSA = new List<double>();
            BestNeighborFitnessListSA = new List<double>();
            SATemperature = new List<double>();
        }

        public void AddGAData(AllGenerationsStatistics allGenerationsStatistics)
        {
            BestFitnessListGA = allGenerationsStatistics.BestFitnessListGA;
            WorstFitnessListGA = allGenerationsStatistics.WorstFitnessListGA;
            AverageFitnessListGA = allGenerationsStatistics.AverageFitnessListGA;
        }

        public void AddTabuSearchData(AllGenerationsStatistics allGenerationsStatistics)
        {
            BestFitnessListTS = allGenerationsStatistics.BestFitnessListTS;
            BestNeighborFitnessListTS = allGenerationsStatistics.BestNeighborFitnessListTS;
        }

        public void AddSimulatedAnnealingData(AllGenerationsStatistics allGenerationsStatistics)
        {
            BestFitnessListSA = allGenerationsStatistics.BestFitnessListSA;
            BestNeighborFitnessListSA = allGenerationsStatistics.BestNeighborFitnessListSA;
            SATemperature = allGenerationsStatistics.SATemperature;
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

        public void SaveBestNeighborFitnessForTS(double bestNeighborFitness)
        {
            BestNeighborFitnessListTS.Add(bestNeighborFitness);
        }

        /// <summary>
        /// Saves best fitness for current generation for Simulated Annealing
        /// </summary>
        /// <param name="bestFitness"></param>
        public void SaveBestNeighborFitnessForSA(double bestFitness)
        {
            BestNeighborFitnessListSA.Add(bestFitness);
        }

        /// <summary>
        /// Saves temperature for current generation for Simulated Annealing
        /// </summary>
        /// <param name="bestFitness"></param>
        public void SaveTemperatureForSA(double temp)
        {
            SATemperature.Add(temp);
        }
    }
}