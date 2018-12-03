using System;
using System.Collections.Generic;
using static GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm.GeneticAlgorithmParameters;

namespace GeneticAlgorithmLogic.Individuals
{
    public class IndividualGenerator : Individual
    {
        public int[] gaParameters;

        public IndividualGenerator()
        {
            gaParameters = new int[Enum.GetNames(typeof(GeneticAlgorithmParametersType)).Length];
        }

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void CountFitness()
        {
            throw new NotImplementedException();
        }

        public override int[] GetMutation()
        {
            throw new NotImplementedException();
        }

        public override List<int[]> GetNeighbors(int numberOfNeighbors)
        {
            throw new NotImplementedException();
        }

        public override bool Mutate()
        {
            throw new NotImplementedException();
        }
    }
}