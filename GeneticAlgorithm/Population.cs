using System;
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

        public Individual BestIndividual { get; set; }
        public Dictionary<int, Individual> Individuals { get; set; }

        public Population()
        {
            CreatePopulationIndividuals();

            //first
            //BestIndividual = Individuals[0];

            //SaveBest();
        }

        public double GetAverageFitness()
        {
            double sumCost = Individuals[0].Fitness;
            for (int i = 1; i < Individuals.Count; i++)
            {
                sumCost += Individuals[i].Fitness;
            }
            return sumCost / POPULATION_SIZE;
        }

        public void CountFitnessForTheEntirePopulation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individuals[i].CountFitness();
            }
        }

        /// <summary>
        /// Worst = smallest fitness
        /// </summary>
        /// <returns></returns>
        public double GetWorstFitness()
        {
            //starts searching from first id
            double worstCost = Individuals[0].Fitness;
            for (int i = 1; i < Individuals.Count; i++)
            {
                if (Individuals[i].Fitness > worstCost)
                {
                    worstCost = Individuals[i].Fitness;
                }
            }
            return worstCost;
        }

        /// <summary>
        /// Best = biggest fitness
        /// </summary>
        /// <returns></returns>
        public double GetBestFitness()
        {
            double worstCost = Individuals[0].Fitness;

            for (int i = 1; i < Individuals.Count; i++)
            {
                if (Individuals[i].Fitness < worstCost)
                {
                    worstCost = Individuals[i].Fitness;
                }
            }
            return worstCost;
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

                averageCounter.SaveData(counter++, BestIndividual.Fitness, GetAverageFitness(), GetWorstFitness());
            }

            return averageCounter;
        }

        public void SelectAndCross()
        {
            Individual individual1 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);
            Individual individual2 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);

            Cross(individual1, individual2);
        }

        private void CreateNewItemsPermutation(ref Individual firstIndividual, ref Individual secondIndividual)
        {
            throw new NotImplementedException();
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
            SaveBest();
        }

        private void CreatePopulationIndividuals()
        {
            Individuals = new Dictionary<int, Individual>(POPULATION_SIZE);
            CreateNewRandomPopulation();
        }

        /// <summary>
        /// TODO
        ///  я не делаю ремонт, потому что я не делаю скрещивание на тех элементах, которых нет на новой таблице
        ///  зачем ломать , а потом чинить, если можно сразу не ломать ? :)
        /// </summary>
        /// <param name="pivot"></param>
        private void Cross(Individual firstIndividual, Individual secondIndividual)
        {
            int randomNumber = Randomizer.random.Next(GeneticAlgorithmParameters.MaxProbability);
            if (GeneticAlgorithmParameters.CrossProbability > randomNumber)
            {
                UseCrossOperator(ref firstIndividual, ref secondIndividual);
                //TODO finish
                //CreateNewItemsPermutation(ref firstIndividual, ref secondIndividual);
            }
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
                if (Individuals[GetRandomId()].Fitness > Individuals[bestId].Fitness)
                {
                    bestId = i;
                }
            }

            return Individuals[bestId];
        }

        private void SaveBest()
        {
            //check is best still best
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                if (BestIndividual.Fitness > Individuals[i].Fitness)
                {
                    BestIndividual = (Individual)Individuals[i].Clone();
                }
            }
        }

        private void UseCrossOperator(ref Individual firstIndividual, ref Individual secondIndividual)
        {
            Individual firstIndividualCopy = (Individual)firstIndividual.Clone();
            Individual secondIndividualCopy = (Individual)secondIndividual.Clone();

            firstIndividual.CrossWithPMXoperator(secondIndividualCopy);
            secondIndividual.CrossWithPMXoperator(firstIndividualCopy);
        }
    }
}