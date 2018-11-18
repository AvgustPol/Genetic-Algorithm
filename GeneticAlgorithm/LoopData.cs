using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class LoopData<T>
    {
        public List<T> ListBest { get; set; }

        public List<T> ListAvg { get; set; }
        public List<T> ListOther { get; set; }

        public enum GaDataType
        {
            /// <summary>
            /// Best individual fitness
            /// </summary>
            Best,

            /// <summary>
            /// Avg individual fitness
            /// </summary>
            Avg,

            /// <summary>
            /// Worst individual fitness
            /// </summary>
            Worst
        }

        public enum SaDataType
        {
            /// <summary>
            /// Best neighbor fitness
            /// </summary>
            Best,

            /// <summary>
            /// Avg neighbors fitness
            /// </summary>
            Avg,

            /// <summary>
            /// Current generation temperature
            /// </summary>
            Temperature
        }

        public enum TsDataType
        {
            /// <summary>
            /// Best neighbor fitness
            /// </summary>
            Best,

            /// <summary>
            /// Avg neighbors fitness
            /// </summary>
            Avg
        }

        //// key - name of stat result. E.g. best fitness
        //// value
        //public Dictionary<string, T> Data { get; set; }
        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        public LoopData()
        {
            ListBest = new List<T>();
            ListOther = new List<T>();
            ListAvg = new List<T>();
        }

        public void AddGAData(LoopData loopData)
        {
            ListBest = loopData.ListBest;
            ListAvg = loopData.ListAvg;
            ListOther = loopData.ListOther;
        }

        public void AddTabuSearchData(LoopData loopData)
        {
            BestFitnessListTS = loopData.BestFitnessListTS;
            BestNeighborFitnessListTS = loopData.BestNeighborFitnessListTS;
        }

        public void AddSimulatedAnnealingData(LoopData loopData)
        {
            BestFitnessListSA = loopData.BestFitnessListSA;
            BestNeighborFitnessListSA = loopData.BestNeighborFitnessListSA;
            SATemperature = loopData.SATemperature;
        }

        public void SaveData(double bestFitness, LoopData<double>.GaDataType best, List<double> listBest)
        {
            ListBest.Add(bestFitness);
        }

        /// <summary>
        /// Saves Worst fitness for current generation
        /// </summary>
        /// <param name="worstFitness"></param>
        public void SaveWorstFitnessForGA(double worstFitness)
        {
            ListAvg.Add(worstFitness);
        }

        /// <summary>
        /// Saves Average fitness for current generation
        /// </summary>
        /// <param name="averageFitness"></param>
        public void SaveAverageFitnessForGA(double averageFitness)
        {
            ListOther.Add(averageFitness);
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