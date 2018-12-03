using System;
using System.Collections.Generic;
using GeneticAlgorithmLogic.Individuals;

namespace GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm
{
    public class Population
    {
        public readonly int PopulationSize;

        public Dictionary<int, IndividualTspKnp> Individuals { get; set; }

        public Population()
        {
            PopulationSize = GeneticAlgorithm.GeneticAlgorithmParameters.PopulationSize;
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

        public int Mutate()
        {
            int mutationCounter = 0;
            for (int i = 0; i < PopulationSize; i++)
            {
                if (Individuals[i].Mutate())
                {
                    mutationCounter++;
                }
            }
            return mutationCounter;
        }

        public int SelectAndCross()
        {
            int counter = 0;
            Dictionary<int, IndividualTspKnp> nextPopulation = new Dictionary<int, IndividualTspKnp>();

            while (nextPopulation.Count != PopulationSize)
            {
                int randomNumber = Randomizer.Random.Next(GeneticAlgorithmParameters.MaxProbability);
                if (GeneticAlgorithm.GeneticAlgorithmParameters.CrossProbability > randomNumber)
                {
                    IndividualTspKnp parent1 = GetTournamentSelectionWinner(GeneticAlgorithm.GeneticAlgorithmParameters.NumberOfTournamentParticipants);
                    IndividualTspKnp parent2 = GetTournamentSelectionWinner(GeneticAlgorithm.GeneticAlgorithmParameters.NumberOfTournamentParticipants);

                    IndividualTspKnp child = Cross(parent1, parent2);
                    nextPopulation.Add(nextPopulation.Count, child);
                    counter++;
                }
                else
                {
                    IndividualTspKnp winner = GetTournamentSelectionWinner(GeneticAlgorithm.GeneticAlgorithmParameters.NumberOfTournamentParticipants);

                    nextPopulation.Add(nextPopulation.Count, winner);
                }
            }
            Individuals = nextPopulation;
            return counter;
        }

        private void CreateNewRandomPopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Individuals.Add(i, new IndividualTspKnp(CreateRandomIndividual()));
            }
        }

        public static int[] CreateRandomIndividual()
        {
            #region Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            int[] defaultArray = new int[AlgorithmCoreParameters.Dimension];
            for (int i = 0; i < AlgorithmCoreParameters.Dimension; i++)
            {
                defaultArray[i] = i;
            }

            #endregion Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            return Permutator.GetRandomPermutation(defaultArray);
        }

        public void CreatePopulationIndividuals()
        {
            Individuals = new Dictionary<int, IndividualTspKnp>(PopulationSize);
            CreateNewRandomPopulation();
        }

        private IndividualTspKnp Cross(IndividualTspKnp firstIndividualTspKnp, IndividualTspKnp secondIndividualTspKnp)
        {
            IndividualTspKnp child = CrossAndGetChild(firstIndividualTspKnp, secondIndividualTspKnp);

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

        private IndividualTspKnp GetTournamentSelectionWinner(int numberOfTournamentParticipants)
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

        private IndividualTspKnp CrossAndGetChild(IndividualTspKnp firstIndividualTspKnp, IndividualTspKnp secondIndividualTspKnp)
        {
            int[] firstIndividualCopy = (int[])firstIndividualTspKnp.Places.Clone();
            int[] secondIndividualCopy = (int[])secondIndividualTspKnp.Places.Clone();

            return new IndividualTspKnp()
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
                itemFitness = Convert.ToDouble(Individuals[i].Fitness);
                sumCost = sumCost + itemFitness;
                counter++;
            }

            double result = sumCost / counter;
            return result;
        }
    }
}