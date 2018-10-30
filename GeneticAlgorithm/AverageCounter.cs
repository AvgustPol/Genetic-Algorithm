using System.Collections.Generic;

namespace GeneticAlgorithm
{
    internal class AverageCounter
    {
        public static double CountAverageFitness(List<GenerationsStatistics> list, int index)
        {
            double sum = 0;
            int counter = 0;
            foreach (var item in list)
            {
                sum += item.AverageFitnessListGA[index];
                counter++;
            }
            return counter > 0 ? sum / counter : 0;
        }
    }
}