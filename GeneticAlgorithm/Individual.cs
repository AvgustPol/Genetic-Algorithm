using System;

namespace GeneticAlgorithm
{
    public class Individual : ICloneable
    {
        public int Fitness { get; set; }
        public int[] Permutation { get; set; }

        public int[] PermutationItems { get; set; }

        public int[] PermutationPlaces { get; set; }

        public object Clone()
        {
            Individual clone = new Individual();
            clone.Fitness = Fitness;
            clone.Permutation = (int[])Permutation.Clone();
            return clone;
        }

        public int[] GetNextGreedyPermutation(int[] originalMatrix)
        {
            // нужно ли создавать tmp ???
            int[] tmp = new int[originalMatrix.Length];
            Array.Copy(originalMatrix, tmp, originalMatrix.Length);

            Shuffle(tmp);

            return tmp;
        }

        public int[] GetRandomPermutation(int[] originalMatrix)
        {
            // нужно ли создавать tmp ???
            int[] tmp = new int[originalMatrix.Length];
            Array.Copy(originalMatrix, tmp, originalMatrix.Length);

            Shuffle(tmp);

            return tmp;
        }

        public void Shuffle(int[] permutation)
        {
            int n = permutation.Length;
            Random random = new Random(n);

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                int value = permutation[k];
                permutation[k] = permutation[n];
                permutation[n] = value;
            }
        }

        public void SwapBeetweenArrays(int[] array1, int[] array2, int index)
        {
            int temp = array1[index];
            array1[index] = array2[index];
            array2[index] = temp;
        }

        public void CountFitness()
        {
            throw new NotImplementedException();
            // Fitness = 42;
        }
    }
}