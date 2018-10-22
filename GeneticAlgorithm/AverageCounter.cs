using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class AverageCounter
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

        public AverageCounter()
        {
            GenerationCounterList = new List<double>();
            BestFitnessListGA = new List<double>();
            AverageFitnessListGA = new List<double>();
            WorstFitnessListGA = new List<double>();

            BestFitnessListTS = new List<double>();
        }

        public void AddGAData(AverageCounter averageCounter)
        {
            BestFitnessListGA = averageCounter.BestFitnessListGA;
            WorstFitnessListGA = averageCounter.WorstFitnessListGA;
            AverageFitnessListGA = averageCounter.AverageFitnessListGA;
        }

        public void AddTabuSearchData(AverageCounter averageCounter)
        {
            BestFitnessListTS = averageCounter.BestFitnessListTS;
        }

        public void SaveGenerationCounter(double counter)
        {
            GenerationCounterList.Add(counter);
        }

        public void SaveBestFitnessForGA(double bestIndividualCost)
        {
            BestFitnessListGA.Add(bestIndividualCost);
        }

        public void SaveWorstFitnessForGA(double worstFitness)
        {
            AverageFitnessListGA.Add(worstFitness);
        }

        public void SaveAverageFitnessForGA(double averageFitness)
        {
            AverageFitnessListGA.Add(averageFitness);
        }

        public void SaveBestFitnessForTS(double averageFitness)
        {
            BestFitnessListTS.Add(averageFitness);
        }
    }
}