using System;

namespace GeneticAlgorithm
{
    public class SimulatedAnnealing
    {
        public static void TryAvoidLocalOptimum(ref Individual best, ref Individual tmpCandidate, double temperature)
        {
            //отнимаем tmpCandidate - best, потому что для данной проблемы best = biggest
            double expValue = Math.Exp((tmpCandidate.Fitness - best.Fitness) / temperature);
            double randomValue = Randomizer.Random.NextDouble();

            //if random[0, 1] < exp{ (f(Vn)– f(Vc))/ T}
            if (randomValue < expValue)
            {
                best = tmpCandidate;
            }
        }

        public static void DecreaseTemperature(ref double temperature, int generationsCounter)
        {
            //golden ratio
            //temperature *= 0.61803398875;

            temperature *= 0.99;
            if (temperature < 0.5)
            {
                temperature = Int64.MaxValue;
            }
        }
    }
}