using DataModel;
using Loader;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public class Population
    {
        private readonly DataContainer _container;
        private readonly DataLoader _dataLoader;
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

        public Population(int dimension)
        {
            Dimension = dimension;
            _random = new Random((int)DateTime.UtcNow.Ticks);
            _dataLoader = new DataLoader();
            _container = _dataLoader.GetCreatedDataContainerFromFileAsync().Result;

            //first
            BestIndividual = Individuals[0];

            CreateNewRandomPopulation();
            //SaveBest();
        }

        public Individual BestIndividual { get; set; }

        /// <summary>
        /// number of places at TSP problem
        /// </summary>
        public int Dimension { get; set; }

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
            while (GENRATION_NUMBERS_STOP_CONDITION > counter)
            {
                CreateNextPopulationCircle();

                averageCounter.SaveData(counter++, BestIndividual.Fitness, CountAverageCost(), FindWorstCost());
            }

            return averageCounter;
        }

        public void SelectAndCross()
        {
            Individual individual1 = GetTournamentSelectionWinner(NUMBER_OF_TOURNAMENT_PARTICIPANTS);
            Individual individual2 = GetTournamentSelectionWinner(NUMBER_OF_TOURNAMENT_PARTICIPANTS);

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
            GetTournamentSelectionWinner(NUMBER_OF_TOURNAMENT_PARTICIPANTS);
            SelectAndCross(); // krzyrzowanie
            DoMutation();
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

        private void DoMutation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individual tmp = Individuals[i];
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
                        Permutator.Swap(tmp.PermutationPlaces, j, randomIndex);

                        throw new NotImplementedException();

                        //tmp.Fitness = CostCounter.CountCost(tmp.PermutationPlaces);
                    }
                }
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
            return _random.Next(POPULATION_SIZE);
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

        /// <summary>
        /// надписывает
        /// </summary>
        /// <param name="leftPivot"></param>
        /// <param name="rightPivot"></param>
        /// <param name="firstIndividual"></param>
        /// <param name="secondIndividual"></param>
        private void UseCrossOperator(ref Individual firstIndividual, ref Individual secondIndividual)
        {
            var tmp1 = firstIndividual.PermutationPlaces;
            var tmp2 = secondIndividual.PermutationPlaces;

            firstIndividual.PMXoperator(secondIndividual);
            secondIndividual.PMXoperator(firstIndividual);
        }
    }
}