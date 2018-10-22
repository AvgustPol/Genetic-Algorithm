using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class AverageCounter
    {
        public List<double> generationCounterList;

        #region Genetic Algorithm

        public List<double> bestIndividualFitnessListGA;
        public List<double> worstFitnessListGA;
        public List<double> averageFitnessListGA;

        #endregion Genetic Algorithm

        #region Tabu Search

        public List<double> worstFitnessListTS { get; set; }

        #endregion Tabu Search

        public AverageCounter()
        {
            generationCounterList = new List<double>();
            bestIndividualFitnessListGA = new List<double>();
            averageFitnessListGA = new List<double>();
            worstFitnessListGA = new List<double>();

            worstFitnessListTS = new List<double>();
        }

        public void SaveData(double counter, double bestIndividualCost, double countAverage, double worstCost)
        {
            generationCounterList.Add(counter);
            bestIndividualFitnessListGA.Add(bestIndividualCost);
            averageFitnessListGA.Add(countAverage);
            worstFitnessListGA.Add(worstCost);
        }
    }
}