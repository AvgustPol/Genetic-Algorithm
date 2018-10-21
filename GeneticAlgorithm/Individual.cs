using System;

namespace GeneticAlgorithm
{
    public class Individual : ICloneable
    {
        public double Fitness { get; set; }

        /// <summary>
        /// KNP problem
        /// </summary
        public int[] PermutationItems { get; set; }

        /// <summary>
        /// TSP problem
        /// </summary>
        public int[] PermutationPlaces { get; set; }

        public object Clone()
        {
            Individual clone = new Individual();
            if (PermutationPlaces != null)
            {
                clone.PermutationPlaces = (int[])PermutationPlaces.Clone();
            }

            if (PermutationItems != null)
            {
                clone.PermutationItems = (int[])PermutationItems.Clone();
            }

            return clone;
        }

        public void CountFitness()
        {
            // Fitness = Sum(itemValue) - R * ( ti - t0)
            //
            // ti - czas z przedmiotem
            // t- czas z pustym plecakiem
            Fitness = 0;

            for (int i = 0; i < GeneticAlgorithmParameters.Dimension - 1; i++)
            {
                Fitness += GeneticAlgorithmParameters.GetDistance(PermutationPlaces[i], PermutationPlaces[i + 1]);
            }

            Fitness += GeneticAlgorithmParameters.GetDistance(PermutationPlaces[0], PermutationPlaces[GeneticAlgorithmParameters.Dimension - 1]);

            Fitness = 1 / Fitness;

            //for (int i = 0; i < PermutationPlaces.Length; i++)
            //{
            //    for (int j = 0; j < PermutationPlaces.Length; j++)
            //    {
            //        //result += StaticMatrixObject.GetFlow(array[i], array[j]) * StaticMatrixObject.GetDistance(i, j);

            //        Fitness += GeneticAlgorithmParameters.GetDistance(i, j);
            //    }
            //}
        }

        public void Mutate()
        {
            int randomNumber = Randomizer.random.Next(GeneticAlgorithmParameters.MaxProbability);
            if (GeneticAlgorithmParameters.MutationProbability > randomNumber)
            {
                int randomIndex1 = Randomizer.random.Next(GeneticAlgorithmParameters.Dimension);
                int randomIndex2 = Randomizer.random.Next(GeneticAlgorithmParameters.Dimension);
                while (randomIndex1 == randomIndex2)
                {
                    randomIndex2 = Randomizer.random.Next(GeneticAlgorithmParameters.Dimension);
                }
                //MUTATE
                Permutator.Swap(PermutationPlaces, randomIndex1, randomIndex2);
            }
        }
    }
}