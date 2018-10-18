using DataModel;
using Loader;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        /// <summary>
        /// number of places at TSP problem
        /// </summary>
        public int Dimension { get; set; }

        private readonly int NOT_FOUND_INDEX = -1;

        /// <summary>
        /// Number of population individuals
        /// </summary>
        private readonly int POPULATION_SIZE = 100;

        public Population()
        {
            #region Create data container and get all data from file

            DataLoader dataLoader = new DataLoader();
            DataContainer container = dataLoader.GetCreatedDataContainerFromFileAsync().Result;

            #endregion Create data container and get all data from file

            Dimension = container.Dimension;

            //first
            BestIndividual = Individuals[0];

            CreateNewRandomPopulation();
            //SaveBest();
        }

        public Individual BestIndividual
        { get; set; }

        public Dictionary<int, Individual> Individuals { get; set; }

        public double CountAverageCost()
        {
            int sumCost = 0;
            for (int i = 0; i < Individuals.Count; i++)
            {
                sumCost += Individuals[i].Fitness;
            }
            return sumCost / POPULATION_SIZE;
        }

        public void CountFitnessForTheEntirePopulation()
        {
            for (int i = 0; i < Individuals.Count; i++)
            {
                Individuals[i].CountFitness();
            }
        }

        public int FindWorstCost()
        {
            //starts searching from first id
            int worstCost = Individuals[0].Fitness;
            for (int i = 0; i < Individuals.Count; i++)
            {
                if (worstCost < Individuals[i].Fitness)
                {
                    worstCost = Individuals[i].Fitness;
                }
            }
            return worstCost;
        }

        public AverageCounter RunAlgorythmWithCounterCondition()
        {
            int counter = 0;

            AverageCounter averageCounter = new AverageCounter();
            while (GeneticAlgorithmParameters.StopConditionGenerationNumbers > counter)
            {
                CreateNextPopulationCircle();

                averageCounter.SaveData(counter++, BestIndividual.Fitness, CountAverageCost(), FindWorstCost());
            }

            return averageCounter;
        }

        public void SelectAndCross()
        {
            Individual individual1 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);
            Individual individual2 = GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);

            Cross(ref individual1, ref individual2);
        }

        private void CreateNewItemsPermutation(ref Individual firstIndividual, ref Individual secondIndividual)
        {
            throw new NotImplementedException();
        }

        private void CreateNewRandomPopulation()
        {
            #region Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            int[] defaultArray = new int[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                defaultArray[i] = i;
            }

            #endregion Create new defualt array {0,1,2,3,4,5, ... , dimension-1}

            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individuals[i].PermutationPlaces = Permutator.GetRandomPermutation(defaultArray);
            }
        }

        private void CreateNextPopulationCircle()
        {
            GetTournamentSelectionWinner(GeneticAlgorithmParameters.NumberOfTournamentParticipants);
            SelectAndCross(); // krzyrzowanie
            Mutate();
            SaveBest();
        }

        /// <summary>
        /// TODO
        ///  я не делаю ремонт, потому что я не делаю скрещивание на тех элементах, которых нет на новой таблице
        ///  зачем ломать , а потом чинить, если можно сразу не ломать ? :)
        /// </summary>
        /// <param name="pivot"></param>
        private void Cross(ref Individual firstIndividual, ref Individual secondIndividual)
        {
            UseCrossOperator(ref firstIndividual, ref secondIndividual);
            CreateNewItemsPermutation(ref firstIndividual, ref secondIndividual);
        }

        public void Mutate()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individuals[i].Mutate();
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
            firstIndividual.CrossWithPMXoperator(secondIndividual);
            secondIndividual.CrossWithPMXoperator(firstIndividual);
        }
    }
}