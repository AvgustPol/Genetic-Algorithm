using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        private readonly int NOT_FOUND_INDEX = -1;

        /// <summary>
        /// Number of population individuals
        /// </summary>
        private readonly int POPULATION_SIZE = 100;

        //public Individual BestIndividual { get; set; }
        public Dictionary<int, Individual> Individuals { get; set; }

        public Population()
        {
            //SaveBest();
        }

        public void CountFitnessForTheEntirePopulation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individuals[i].CountFitness();
            }
        }

        public double GetAverageFitness()
        {
            double sumCost = Individuals[0].Fitness;
            for (int i = 1; i < POPULATION_SIZE; i++)
            {
                sumCost += Individuals[i].Fitness;
            }
            return sumCost / POPULATION_SIZE;
        }

        /// <summary>
        /// For this problem
        /// Worst = smallest fitness
        /// </summary>
        /// <returns></returns>
        public double GetWorstFitness()
        {
            return GetSmallestFitness();
        }

        private double GetSmallestFitness()
        {
            //starts searching from first id (Individuals[0])
            double smallestFitness = Individuals[0].Fitness;

            //so we can search from the second one id (i = 1)
            for (int i = 1; i < POPULATION_SIZE; i++)
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
            return GetBiggestFitness();
        }

        private double GetBiggestFitness()
        {
            //starts searching from first id (Individuals[0])
            double biggestFitness = Individuals[0].Fitness;

            //so we can search from the second one id (i = 1)
            for (int i = 1; i < POPULATION_SIZE; i++)
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
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individuals[i].Mutate();
            }
        }

        public AverageCounter RunAlgorythmWithCounterCondition()
        {
            int counter = 0;

            AverageCounter averageCounter = new AverageCounter();
            while (GeneticAlgorithmParameters.StopConditionGenerationNumbers > counter)
            {
                CreateNextPopulationCircle();

                //averageCounter.SaveData(counter++, BestIndividual.Fitness, GetAverageFitness(), GetWorstFitness());
                averageCounter.SaveData(counter++, GetBestFitness(), GetAverageFitness(), GetWorstFitness());
            }

            return averageCounter;
        }

        public void SelectAndCross()
        {
            Dictionary<int, Individual> nextPopulation = new Dictionary<int, Individual>();

            while (nextPopulation.Count != POPULATION_SIZE)
            {
                int randomNumber = Randomizer.random.Next(GeneticAlgorithmParameters.MaxProbability);
                if (GeneticAlgorithmParameters.CrossProbability > randomNumber)
                {
                    Individual parent1 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);
                    Individual parent2 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);

                    Individual child = Cross(parent1, parent2);
                    nextPopulation.Add(nextPopulation.Count, child);
                }
                else
                {
                    //int doubledNumberOfTournamentParticipants =
                    //    GeneticAlgorithmParameters.NumberOfTournamentParticipants * 2;
                    //Individual winner = GetTournamentSelectionWinner(doubledNumberOfTournamentParticipants);

                    Individual winner = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);

                    nextPopulation.Add(nextPopulation.Count, winner);
                }
            }
            Individuals = nextPopulation;
        }

        private void CreateNewRandomPopulation()
        {
            #region Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            int[] defaultArray = new int[GeneticAlgorithmParameters.Dimension];
            for (int i = 0; i < GeneticAlgorithmParameters.Dimension; i++)
            {
                defaultArray[i] = i;
            }

            #endregion Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individuals.Add(i, new Individual()
                {
                    PermutationPlaces = Permutator.GetRandomPermutation(defaultArray)
                });
            }
        }

        private void CreateNextPopulationCircle()
        {
            GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);
            SelectAndCross(); // krzyrzowanie
            Mutate();
        }

        public void CreatePopulationIndividuals()
        {
            Individuals = new Dictionary<int, Individual>(POPULATION_SIZE);
            CreateNewRandomPopulation();
        }

        private Individual Cross(Individual firstIndividual, Individual secondIndividual)
        {
            Individual child = CrossAndGetChild(firstIndividual, secondIndividual);

            //TODO finish
            //CreateNewItemsPermutation(child);

            return child;
        }

        private int FindThisNumberInArray(int[] permutation, int value)
        {
            int permutationLength = permutation.Length;
            for (int i = 0; i < permutationLength; i++)
            {
                if (permutation[i] == value)
                {
                    return i;
                }
            }
            return NOT_FOUND_INDEX;
        }

        /// <summary>
        /// Returns random index at population permutation
        /// </summary>
        /// <returns></returns>
        private int GetRandomId()
        {
            return Randomizer.random.Next(POPULATION_SIZE);
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
            int[] firstIndividualCopy = (int[])firstIndividual.PermutationPlaces.Clone();
            int[] secondIndividualCopy = (int[])secondIndividual.PermutationPlaces.Clone();

            return new Individual()
            {
                PermutationPlaces = Permutator.CrossPermutations(firstIndividualCopy, secondIndividualCopy)
            };
        }
    }
}