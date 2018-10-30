using System;

namespace GeneticAlgorithm
{
    public class SimulatedAnnealing
    {
        public static void TryAvoidLocalOptimum(ref Individual current, ref Individual candidate, double temperature)
        {
            //отнимаем candidate - current, потому что для данной проблемы best = biggest
            double expValue = Math.Exp((candidate.Fitness - current.Fitness) / temperature);
            double randomValue = Randomizer.Random.NextDouble();

            //if random[0, 1] < exp{ (f(Vn)– f(Vc))/ T}
            if (randomValue < expValue)
            {
                current = candidate;
            }
        }

        public static void DecreaseTemperature(ref double temperature, int generationsCounter)
        {
            temperature *= 0.95;
        }
    }
}