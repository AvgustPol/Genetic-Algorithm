using System;

namespace GeneticAlgorithm
{
    public class Randomizer
    {
        public static readonly Random random = new Random((int)DateTime.UtcNow.Ticks);
    }
}