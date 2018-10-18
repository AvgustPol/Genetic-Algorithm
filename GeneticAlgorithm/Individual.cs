﻿using System;

namespace GeneticAlgorithm
{
    public class Individual : ICloneable
    {
        public readonly int MAX_PERMUTATION_PLACES_INDEX = GeneticAlgorithmParameters.Dimension - 1;

        public double Fitness { get; set; }

        /// <summary>
        /// KNP problem
        /// </summary>
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

        /// <summary>
        /// Source :
        /// http://www.rubicite.com/Tutorials/GeneticAlgorithms/CrossoverOperators/PMXCrossoverOperator.aspx/
        /// </summary>
        /// <param name="parent2"></param>
        public void CrossWithPMXoperator(Individual parent2)
        {
            #region Select pivot

            int pivot = GetRandomPivot();
            int length = GetRandomLength(pivot);
            int pivotPlusLength = pivot + length;

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
            Array.Copy(PermutationPlaces, pivot, childPermutationPlaces, pivot, length);

            #endregion Create child permutation

            #region Copy swath

            for (int i = pivot; i < pivotPlusLength; i++)
            {
                int valueToFindIndex = parent2.PermutationPlaces[i];
                int indexOfValueInParent2 = FindIndexInArray(valueToFindIndex, childPermutationPlaces);

                if (indexOfValueInParent2 == NOT_FOUND_CODE)
                {
                    // заменить из parent2.PermutationPlaces[i] с childPermutationPlaces
                    // FindChildIndex должен найти индекс, куда значение parent2.PermutationPlaces[i] будет вставлено , то есть:
                    int childIndex = FindChildIndex(valueToFindIndex, pivot, pivotPlusLength, parent2);
                    childPermutationPlaces[childIndex] = parent2.PermutationPlaces[i];
                }
            }

            #endregion Copy swath

            #region Fill Empty Array Elements

            for (int i = 0; i < childPermutationPlaces.Length; i++)
            {
                if (childPermutationPlaces[i] == NOT_FOUND_CODE)
                {
                    childPermutationPlaces[i] = parent2.PermutationPlaces[i];
                }
            }

            #endregion Fill Empty Array Elements

            PermutationPlaces = childPermutationPlaces;
        }

        private int FindChildIndex(
            int valueToFindIndex,
            int pivot,
            int pivotPlusLength,
            Individual parent2)
        {
            //Note the index of this value in Parent 2.
            int index = FindIndexInArray(valueToFindIndex, parent2.PermutationPlaces);

            //Locate the value, V, from parent 1 in this same position.
            int valueInParent1 = PermutationPlaces[index];

            //Locate this same value in parent 2.
            int indexToSwap = FindIndexInArray(valueInParent1, parent2.PermutationPlaces);

            //If the index of this value in Parent 2 is part of the original swath, go to step i. using this value.
            if (indexToSwap >= pivot && indexToSwap <= pivotPlusLength)
            {
                return FindChildIndex(parent2.PermutationPlaces[indexToSwap], pivot, pivotPlusLength, parent2);
            }

            return indexToSwap;
        }

        private int FindIndexInArray(int value, int[] array)
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

        public int NOT_FOUND_CODE = -1;

        /// <summary>
        /// The pivot is a range [1, Dimension - 2]
        /// so length is a range [1, Dimension - 3]
        /// </summary>
        /// <param name="pivot"></param>
        /// <returns></returns>
        private int GetRandomLength(int pivot)
        {
            int minlength = 1;
            int maxlength = MAX_PERMUTATION_PLACES_INDEX - pivot;
            return Randomizer.random.Next(minlength, maxlength);
        }

        /// <summary>
        /// Returns random value from first (array id = 0)
        /// to last (array id = Dimension - 1)
        /// </summary>
        /// <returns></returns>
        private int GetRandomPivot()
        {
            int randomPivot = Randomizer.random.Next(0, MAX_PERMUTATION_PLACES_INDEX);
            return randomPivot;
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