﻿using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        public readonly int PopulationSize;

        public Dictionary<int, Individual> Individuals { get; set; }

        public Population()
        {
            PopulationSize = GeneticAlgorithmParameters.PopulationSize;
        }

        public void CountFitnessForTheEntirePopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Individuals[i].CountFitness();
            }
        }

        /// <summary>
        /// For this problem
        /// Worst = smallest fitness
        /// </summary>
        /// <returns></returns>
        public double GetWorstFitness()
        {
            double smallestFitness = GetSmallestFitness();
            return smallestFitness;
        }

        private double GetSmallestFitness()
        {
            //starts searching from first id (Individuals[0])
            double smallestFitness = Individuals[0].Fitness;

            //so we can search from the second one id (i = 1)
            for (int i = 1; i < PopulationSize; i++)
            {
                if (smallestFitness > Individuals[i].Fitness)
                {
                    smallestFitness = Individuals[i].Fitness;
                }
            }
            return smallestFitness;
        }

        /// <summary>
        /// For this problem
        /// Best = biggest fitness
        /// </summary>
        /// <returns></returns>
        public double GetBestFitness()
        {
            double biggestFitness = GetBiggestFitness();
            return biggestFitness;
        }

        private double GetBiggestFitness()
        {
            //starts searching from first id (Individuals[0])
            double biggestFitness = Individuals[0].Fitness;

            //so we can search from the second one id (i = 1)
            for (int i = 1; i < PopulationSize; i++)
            {
                if (biggestFitness < Individuals[i].Fitness)
                {
                    biggestFitness = Individuals[i].Fitness;
                }
            }
            return biggestFitness;
        }

        public void Mutate()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Individuals[i].Mutate();
            }
        }

        public void SelectAndCross()
        {
            Dictionary<int, Individual> nextPopulation = new Dictionary<int, Individual>();

            while (nextPopulation.Count != PopulationSize)
            {
                int randomNumber = Randomizer.Random.Next(GeneticAlgorithmParameters.MaxProbability);
                if (GeneticAlgorithmParameters.CrossProbability > randomNumber)
                {
                    Individual parent1 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);
                    Individual parent2 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);

                    Individual child = Cross(parent1, parent2);
                    nextPopulation.Add(nextPopulation.Count, child);
                }
                else
                {
                    Individual winner = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);

                    nextPopulation.Add(nextPopulation.Count, winner);
                }
            }
            Individuals = nextPopulation;
        }

        private void CreateNewRandomPopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Individuals.Add(i, new Individual(CreateRandomIndividual()));
            }
        }

        public static int[] CreateRandomIndividual()
        {
            #region Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            int[] defaultArray = new int[GeneticAlgorithmParameters.Dimension];
            for (int i = 0; i < GeneticAlgorithmParameters.Dimension; i++)
            {
                defaultArray[i] = i;
            }

            #endregion Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            return Permutator.GetRandomPermutation(defaultArray);
        }

        public void CreatePopulationIndividuals()
        {
            Individuals = new Dictionary<int, Individual>(PopulationSize);
            CreateNewRandomPopulation();
        }

        private Individual Cross(Individual firstIndividual, Individual secondIndividual)
        {
            Individual child = CrossAndGetChild(firstIndividual, secondIndividual);

            child.CreateItems();
            child.CountFitness();

            return child;
        }

        /// <summary>
        /// Returns Random index at population permutation
        /// </summary>
        /// <returns></returns>
        private int GetRandomId()
        {
            return Randomizer.Random.Next(PopulationSize);
        }

        private Individual GetTournamentSelectionWinner(int numberOfTournamentParticipants)
        {
            int bestId = GetRandomId();
            numberOfTournamentParticipants--;
            for (int i = 0; i < numberOfTournamentParticipants; i++)
            {
                int randomId = GetRandomId();
                if (Individuals[randomId].Fitness > Individuals[bestId].Fitness)
                {
                    bestId = randomId;
                }
            }

            return Individuals[bestId];
        }

        private Individual CrossAndGetChild(Individual firstIndividual, Individual secondIndividual)
        {
            int[] firstIndividualCopy = (int[])firstIndividual.Places.Clone();
            int[] secondIndividualCopy = (int[])secondIndividual.Places.Clone();

            return new Individual()
            {
                Places = Permutator.CrossPermutations(firstIndividualCopy, secondIndividualCopy)
            };
        }

        /// <summary>
        /// Get average fitness from all population at current generation
        /// </summary>
        /// <param name="individuals">population individuals</param>
        /// <returns>double - average fitness from all population at current generation </returns>
        public double GetAverageFitness()
        {
            int counter = 0;
            double sumCost = 0;
            double itemFitness;
            for (int i = 0; i < Individuals.Count; i++)
            {
                itemFitness = Individuals[i].Fitness;
                sumCost += itemFitness;
                counter++;
            }
            return sumCost / counter;
        }
    }
}