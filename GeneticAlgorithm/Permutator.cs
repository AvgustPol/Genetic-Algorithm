using System.Linq;

namespace GeneticAlgorithm
{
    public class Permutator
    {
        /// <summary>
        /// Shuffles source array
        /// </summary>
        /// <param name="array"></param>
        public static void Shuffle(int[] array)
        {
            int n = array.Length;

            while (n > 1)
            {
                n--;
                int k = Randomizer.random.Next(n + 1);
                int value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }

        public static int[] GetNextGreedyPermutation(int[] originalArray)
        {
            int[] arrayCopy = originalArray.ToArray();

            Shuffle(arrayCopy);

            return arrayCopy;
        }

        public static void Swap(int[] source, int index1, int index2)
        {
            int temp = source[index1];
            source[index1] = source[index2];
            source[index2] = temp;
        }

        public static int[] GetRandomPermutation(int[] originalArray)
        {
            int[] arrayCopy = originalArray.ToArray();

            Shuffle(arrayCopy);

            return arrayCopy;
        }

        public static void SwapBetweenArrays(int[] array1, int[] array2, int index)
        {
            int temp = array1[index];
            array1[index] = array2[index];
            array2[index] = temp;
        }
    }
}