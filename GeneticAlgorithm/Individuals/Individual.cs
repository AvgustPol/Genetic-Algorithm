using System;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Individuals
{
    public abstract class Individual : ICloneable, INeighborly
    {
        public double Fitness { get; set; }

        public abstract void CountFitness();

        public abstract bool Mutate();

        public abstract int[] GetMutation();

        public abstract object Clone();

        public abstract List<int[]> GetNeighbors(int numberOfNeighbors);
    }
}