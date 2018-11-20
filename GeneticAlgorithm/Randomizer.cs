using System;

namespace GeneticAlgorithmLogic
{
    public class Randomizer
    {
        public static readonly int Seed = (int)DateTime.UtcNow.Ticks;
        public static readonly Random Random = new Random(Seed);
    }
}