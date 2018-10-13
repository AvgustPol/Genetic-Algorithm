using System;

namespace GeneticAlgorithm
{
    public class Permutator
    {
        public static void Shuffle(int[] permutation)
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

        public static int[] GetNextGreedyPermutation(int[] originalMatrix)
        {
            // нужно ли создавать tmp ???
            int[] tmp = new int[originalMatrix.Length];
            Array.Copy(originalMatrix, tmp, originalMatrix.Length);

            Shuffle(tmp);

            return tmp;
        }

        public static void Swap(int[] source, int index1, int index2)
        {
            int temp = source[index1];
            source[index1] = source[index2];
            source[index2] = temp;
        }

        public static int[] GetRandomPermutation(int[] originalMatrix)
        {
            // нужно ли создавать tmp ???
            int[] tmp = new int[originalMatrix.Length];
            Array.Copy(originalMatrix, tmp, originalMatrix.Length);

            Shuffle(tmp);

            return tmp;
        }

        public static void SwapBeetweenArrays(int[] array1, int[] array2, int index)
        {
            int temp = array1[index];
            array1[index] = array2[index];
            array2[index] = temp;
        }
    }
}