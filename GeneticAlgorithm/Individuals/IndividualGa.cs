using System;
using static GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm.GeneticAlgorithmParameters;

namespace GeneticAlgorithmLogic.Individuals
{
    public class IndividualGa
    {
        public int[] gaParameters;

        public double Fitness { get; set; }

        public IndividualGa()
        {
            gaParameters = new int[Enum.GetNames(typeof(GeneticAlgorithmParametersType)).Length];
        }
    }
}