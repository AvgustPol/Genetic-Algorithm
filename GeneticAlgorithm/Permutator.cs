using System.Linq;

namespace GeneticAlgorithm
{
    public class Permutator
    {
        public static int NOT_FOUND_CODE = -1;

        private static readonly int MAX_PERMUTATION_PLACES_INDEX = GeneticAlgorithmParameters.Dimension - 1;

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

        /// <summary>
        /// The pivot is a range [1, Dimension - 2]
        /// so length is a range [1, Dimension - 3]
        /// </summary>
        /// <param name="pivot"></param>
        /// <returns></returns>
        private static int GetRandomLength(int pivot)
        {
            int minlength = 2;
            int maxlength = MAX_PERMUTATION_PLACES_INDEX - pivot + 2;
            return Randomizer.random.Next(minlength, maxlength);
        }

        /// <summary>
        /// Returns random value from first (array id = 0)
        /// to last (array id = Dimension - 1)
        /// </summary>
        /// <returns></returns>
        private static int GetRandomPivot()
        {
            int randomPivot = Randomizer.random.Next(0, MAX_PERMUTATION_PLACES_INDEX);
            return randomPivot;
        }

        public static int[] CrossPermutations(int[] firstIndividualCopy, int[] secondIndividualCopy)
        {
            return CrossWithPMXoperator(firstIndividualCopy, secondIndividualCopy);
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

        /// <summary>
        /// Source :
        /// http://www.rubicite.com/Tutorials/GeneticAlgorithms/CrossoverOperators/PMXCrossoverOperator.aspx/
        /// </summary>
        /// <param name="parent2"></param>
        private static int[] CrossWithPMXoperator(int[] parent1, int[] parent2)
        {
            #region Select pivot

            int pivot = GetRandomPivot();
            int length = GetRandomLength(pivot);
            int pivotPlusLength = pivot + length + 1;

            #endregion Select pivot

            #region Create child permutation

            int[] childPermutationPlaces = new int[GeneticAlgorithmParameters.Dimension];
            for (int i = 0; i < GeneticAlgorithmParameters.Dimension; i++)
            {
                // NOT_FOUND_CODE = -1 - because indexes are integers
                childPermutationPlaces[i] = NOT_FOUND_CODE;
            }

            //Randomly select a swath of alleles from parent 1 and copy them directly to the child.
            //Note the indexes of the segment:
            //  start - pivot
            //  end - pivotPlusLength
            //Array.Copy((PermutationPlaces, pivot, childPermutationPlaces, pivot, length);
            for (int i = pivot; i < pivotPlusLength; i++)
            {
                childPermutationPlaces[i] = parent1[i];
            }

            #endregion Create child permutation

            #region Copy swath

            for (int i = pivot; i < pivotPlusLength; i++)
            {
                int valueToFindIndex = parent2[i];

                if (!childPermutationPlaces.Contains(valueToFindIndex))
                {
                    // заменить из parent2.PermutationPlaces[i] с array
                    // FindChildIndex должен найти индекс, куда значение parent2.PermutationPlaces[i] будет вставлено , то есть:
                    int childIndex = FindChildIndex(valueToFindIndex, pivot, pivotPlusLength, parent1, parent2);
                    childPermutationPlaces[childIndex] = valueToFindIndex;
                }
            }

            #endregion Copy swath

            #region Fill Empty Array Elements

            for (int i = 0; i < childPermutationPlaces.Length; i++)
            {
                if (childPermutationPlaces[i] == NOT_FOUND_CODE)
                {
                    childPermutationPlaces[i] = parent2[i];
                }
            }

            #endregion Fill Empty Array Elements

            return childPermutationPlaces;
        }

        private static int FindChildIndex(
            int valueToFindIndex,
            int pivot,
            int pivotPlusLength,
            int[] parent1,
            int[] parent2)
        {
            //Note the index of this value in Parent 2.
            int index = GetIndexOfValueInArray(valueToFindIndex, parent2);

            //Locate the value, V, from parent 1 in this same position.
            int valueInParent1 = parent1[index];

            //Locate this same value in parent 2.
            int indexInParent2 = GetIndexOfValueInArray(valueInParent1, parent2);

            //If the index of this value in Parent 2 is part of the original swath, go to step i. using this value.
            if (indexInParent2 >= pivot && indexInParent2 <= pivotPlusLength)
            {
                return FindChildIndex(valueInParent1, pivot, pivotPlusLength, parent1, parent2);
            }

            return indexInParent2;
        }

        private static int GetIndexOfValueInArray(int value, int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    return i;
                }
            }

            return NOT_FOUND_CODE;
        }
    }
}