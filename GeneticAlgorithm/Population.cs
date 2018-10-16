﻿using DataModel;
using Loader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class Population
    {
        private readonly Random _random;

        /// <summary>
        /// STOP_CONDITION
        /// Number of generations that will be generated before stop.
        /// </summary>
        private readonly int GENRATION_NUMBERS_STOP_CONDITION = 101;

        /// <summary>
        /// HYBRIDIZATION PROBABILITY
        /// (e. g. 70% )
        /// </summary>
        private readonly int HYBRIDIZATION_PROBABILITY = 70;

        /// <summary>
        /// MAX PROBABILITY = 100%
        /// </summary>
        private readonly int MAX_PROBABILITY = 100;

        /// <summary>
        /// MUTATION PROBABILITY (e. g. 1% )
        /// </summary>
        private readonly int MUTATION_PROBABILITY = 1;

        private readonly int NOT_FOUND_INDEX = -1;
        private readonly int NUMBER_OF_TOURNAMENT_PARTICIPANTS = 5;

        /// <summary>
        /// Number of population individuals
        /// </summary>
        private readonly int POPULATION_SIZE = 100;

        private readonly DataLoader _dataLoader;
        private readonly DataContainer _container;

        public Population(int dimension)
        {
            Dimension = dimension;
            _random = new Random((int)DateTime.UtcNow.Ticks);
            _dataLoader = new DataLoader();
            _container = _dataLoader.GetCreatedDataContainerFromFileAsync().Result;

            //first
            BestIndividual = Individuals.ElementAt(0);

            CreateNewRandomPopulation();
            CountCostForAllIndividuals();
            SaveBest();
        }

        private void CountCostForAllIndividuals()
        {
            throw new NotImplementedException();
        }

        public Individual BestIndividual { get; set; }

        /// <summary>
        /// number of places at TSP problem
        /// </summary>
        public int Dimension { get; set; }

        public List<Individual> Individuals { get; set; }

        public double CountAverageCost()
        {
            int sumCost = 0;
            foreach (var item in Individuals)
            {
                sumCost += item.Fitness;
            }
            return sumCost / POPULATION_SIZE;
        }

        public int FindWorstCost()
        {
            int worstCost = Individuals.FirstOrDefault().Fitness;
            foreach (var item in Individuals)
            {
                if (worstCost < item.Fitness)
                {
                    worstCost = item.Fitness;
                }
            }
            return worstCost;
        }

        public AverageCounter RunAlgorythmWithCounterCondition()
        {
            int counter = 0;

            AverageCounter averageCounter = new AverageCounter();
            while (GENRATION_NUMBERS_STOP_CONDITION > counter)
            {
                CreateNextPopulationCircle();

                averageCounter.SaveData(counter++, BestIndividual.Fitness, CountAverageCost(), FindWorstCost());
            }

            return averageCounter;
        }

        /// <summary>
        /// TODO
        ///  я не делаю ремонт, потому что я не делаю скрещивание на тех элементах, которых нет на новой таблице
        ///  зачем ломать , а потом чинить, если можно сразу не ломать ? :)
        /// </summary>
        /// <param name="pivot"></param>
        private void Cross(int pivot)
        {
            int halfPopulation = POPULATION_SIZE / 2;
            for (int i = 0; i < halfPopulation; i += 2)
            {
                int randomNumber = _random.Next(MAX_PROBABILITY);
                if (HYBRIDIZATION_PROBABILITY > randomNumber)
                {
                    CreateNewPairIndividuals(pivot, Individuals.ElementAt(i), Individuals.ElementAt(i + 1));
                }

                #region Recount cost for changed permutation

                //Individuals.ElementAt(i).Fitness = CostCounter.CountCost(Individuals.ElementAt(i).PermutationItems);
                //Individuals.ElementAt(i + 1).Fitness = CostCounter.CountCost(Individuals.ElementAt(i + 1).PermutationItems);

                #endregion Recount cost for changed permutation
            }
        }

        private void CreateNewPairIndividuals(int pivot, Individual firstIndividual, Individual secondIndividual)
        {
            //for (int i = 0; i < pivot; i++)
            //{
            //    int secondIndividualPermutationElementAtCurrentIndexI = secondIndividual.PermutationItems.ElementAt(i);
            //    int[] tmpArray = new int[pivot];
            //    Array.Copy(firstIndividual.PermutationItems, tmpArray, pivot);
            //    if (WasHere(tmpArray, secondIndividualPermutationElementAtCurrentIndexI))
            //    {
            //        Permutator.Swap(firstIndividual.PermutationItems, i, FindThisNumberInArray(firstIndividual.PermutationItems, secondIndividualPermutationElementAtCurrentIndexI));
            //        Permutator.SwapBeetweenArrays(firstIndividual.PermutationItems, secondIndividual.PermutationItems, i);
            //    }
            //}
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
                Individuals.ElementAt(i).PermutationItems = Permutator.GetRandomPermutation(defaultArray);
            }
        }

        private void CreateNextPopulationCircle()
        {
            GetTournamentSelectionWinner(NUMBER_OF_TOURNAMENT_PARTICIPANTS);
            SelectAndCross(); // krzyrzowanie
            DoMutation();
            SaveBest();
        }

        private void DoMutation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individual tmp = Individuals.ElementAt(i);
                for (int j = 0; j < Dimension; j++)
                {
                    int randomNumber = _random.Next(MAX_PROBABILITY);
                    if (MUTATION_PROBABILITY > randomNumber)
                    {
                        int randomIndex = _random.Next(Dimension);
                        while (j == randomIndex)
                        {
                            randomIndex = _random.Next(Dimension);
                        }
                        //MUTATE
                        Permutator.Swap(tmp.PermutationItems, j, randomIndex);

                        throw new NotImplementedException();

                        //tmp.Fitness = CostCounter.CountCost(tmp.PermutationItems);
                    }
                }
            }
        }

        public void SelectAndCross()
        {
            Individual individual1 = GetTournamentSelectionWinner(NUMBER_OF_TOURNAMENT_PARTICIPANTS);
            Individual individual2 = GetTournamentSelectionWinner(NUMBER_OF_TOURNAMENT_PARTICIPANTS);

            #region Select pivot

            int pivotIndex = GetRandomPivot();
            //or at middle :
            //int pivotIndex = Dimension / 2;

            #endregion Select pivot

            Cross(pivotIndex);
        }

        private Individual GetTournamentSelectionWinner(int numberOfTournamentParticipants)
        {
            int bestId = GetRandomId();
            numberOfTournamentParticipants--;
            for (int i = 0; i < numberOfTournamentParticipants; i++)
            {
                if (Individuals.ElementAt(GetRandomId()).Fitness > Individuals.ElementAt(bestId).Fitness)
                {
                    bestId = i;
                }
            }

            return Individuals.ElementAt(bestId);
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
            return _random.Next(POPULATION_SIZE);
        }

        private void SaveBest()
        {
            //check is best still best
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                if (BestIndividual.Fitness > Individuals.ElementAt(i).Fitness)
                {
                    BestIndividual = (Individual)Individuals.ElementAt(i).Clone();
                }
            }
        }

        /// <summary>
        /// Returns random value from 1 to Dimension - 2
        /// pivot can't be first (0) or last (Dimension - 2) index
        /// </summary>
        /// <returns></returns>
        private int GetRandomPivot()
        {
            int randomPivot = _random.Next(1, Dimension - 2);
            return randomPivot;
        }
    }
}