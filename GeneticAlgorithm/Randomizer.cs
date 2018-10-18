using System;

namespace GeneticAlgorithm
{
    internal class Randomizer
    {
        public static readonly Random random = new Random((int)DateTime.UtcNow.Ticks);
    }
}