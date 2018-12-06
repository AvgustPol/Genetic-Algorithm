using System;

namespace GeneticAlgorithmLogic.Сommon.Distribution
{
    public class GaussDistribution
    {
        public static void NewM()
        {
            Random Rand = new Random((int)DateTime.UtcNow.Ticks); //reuse this if you are generating many

            double u1 = 1.0 - Rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - Rand.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

            //double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        }
    }
}