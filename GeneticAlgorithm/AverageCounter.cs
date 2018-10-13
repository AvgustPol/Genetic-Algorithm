using System.Collections.Generic;

namespace GeneticAlgorithm
{
    internal class AverageCounter
    {
        public List<double> generationCounterList;
        public List<double> bestIndividualCostList;
        public List<double> worstCostList;
        public List<double> averageCostList;

        public AverageCounter()
        {
            generationCounterList = new List<double>();
            bestIndividualCostList = new List<double>();
            averageCostList = new List<double>();
            worstCostList = new List<double>();
        }

        internal void SaveData(double counter, double bestIndividualCost, double countAverage, double worstCost)
        {
            generationCounterList.Add(counter);
            bestIndividualCostList.Add(bestIndividualCost);
            averageCostList.Add(countAverage);
            worstCostList.Add(worstCost);
        }
    }
}