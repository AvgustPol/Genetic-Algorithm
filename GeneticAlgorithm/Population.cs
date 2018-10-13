using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    internal class Population
    {
        private Random random;

        private readonly int GENRATION_NUMBERS_STOP_CONDITION = 101; // количество итараций
                                                                     //readonly int TIMER_STOP_CONDITION = 5000; // время в милисикундах

        private readonly int POPULATION_SIZE = 100;
        private readonly int NUMBER_OF_TOURNAMENT_PARTICIPANTS = 5;

        private readonly int HYBRIDIZATION_PROBABILITY = 70; // x%
        private readonly int MUTATION_PROBABILITY = 1; // x%
        private readonly int MAX_PROBABILITY = 100; // 100%

        private readonly int NOT_FOUND_INDEX = -1;

        private List<Individual> individuals;
        public Individual BestIndividual { get; set; }
        public int Dimension { get; set; }

        public Population(int dimension)
        {
            random = new Random((int)DateTime.UtcNow.Ticks);
            Dimension = dimension;
            individuals = new List<Individual>(POPULATION_SIZE);
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                individuals.Add(new Individual());
            }
            //first
            BestIndividual = individuals.ElementAt(0);

            CreateNewRandomPopulation();
            CountCostForAllIndividuals();
            SaveBest();
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
                individuals.ElementAt(i).Permutation = Permutator.GetRandomPermutation(defaultArray);
            }
        }

        private void CountCostForAllIndividuals()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                individuals.ElementAt(i).Fitnes = CountFitnes(individuals.ElementAt(i).Permutation);
            }
        }

        private int CountFitnes(int[] permutation)
        {
            throw new NotImplementedException();
        }

        private void DoTournamentSelection()
        {
            List<Individual> tmpIndividuals = new List<Individual>(POPULATION_SIZE);

            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                int randomIndex1 = random.Next(POPULATION_SIZE - 1);
                int randomIndex2 = random.Next(POPULATION_SIZE - 1);
                while (randomIndex1 == randomIndex2)
                {
                    randomIndex2 = random.Next(POPULATION_SIZE - 1);
                }
                //we are looking for the smalles cost.
                if (individuals.ElementAt(randomIndex1).Fitnes > individuals.ElementAt(randomIndex2).Fitnes)
                {
                    tmpIndividuals.Add(individuals.ElementAt(randomIndex2));
                }
                else
                {
                    tmpIndividuals.Add(individuals.ElementAt(randomIndex1));
                }
            }
            individuals = tmpIndividuals;
        }

        private void DoMultiTournamentSelection(int participantsNumber)
        {
            List<Individual> tmpIndividuals = new List<Individual>(POPULATION_SIZE);

            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                #region Create random indexes

                int[] randomIndexes = new int[participantsNumber];
                for (int z = 0; z < participantsNumber; z++)
                {
                    randomIndexes[z] = random.Next(POPULATION_SIZE);
                }

                #endregion Create random indexes

                #region Create tmp list of individuals (by random indexes)

                List<Individual> tmpTournamentIndividuals = new List<Individual>();
                foreach (var item in randomIndexes)
                {
                    tmpTournamentIndividuals.Add((Individual)individuals.ElementAt(item).Clone());
                }

                #endregion Create tmp list of individuals (by random indexes)

                #region Find best in tmp list

                Individual tmpBest = tmpTournamentIndividuals.First();

                foreach (var item in tmpTournamentIndividuals)
                {
                    if (item.Fitnes < tmpBest.Fitnes)
                        tmpBest = item;
                }

                #endregion Find best in tmp list

                tmpIndividuals.Add(tmpBest);
            }
            individuals = new List<Individual>(tmpIndividuals);
        }

        public class RuletkaPobability
        {
            public int Index { get; set; }
            public decimal Probability { get; set; }

            public RuletkaPobability(int index, decimal probability)
            {
                Index = index;
                Probability = probability;
            }
        }

        private void DoRuletkaSelection()
        {
            List<Individual> tmpIndividuals = new List<Individual>(POPULATION_SIZE);
            List<RuletkaPobability> ruletkaList = new List<RuletkaPobability>();
            decimal costSum = 0;
            int counter = 0;
            foreach (var item in individuals)
            {
                costSum += Decimal.Parse(item.Fitnes.ToString());
            }
            foreach (var item in individuals)
            {
                ruletkaList.Add(new RuletkaPobability(counter++, (decimal)(item.Fitnes / costSum)));
            }

            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                bool found = false;
                while (!found)
                {
                    int randomProbability = random.Next(MAX_PROBABILITY);
                    int randomElement = random.Next(POPULATION_SIZE);
                    if (ruletkaList.ElementAt(randomElement).Probability > randomProbability)
                    {
                        found = true;
                        tmpIndividuals.Add(individuals.ElementAt(randomElement));
                    }
                }
            }

            individuals = tmpIndividuals;
        }

        public AverageCounter RunAlgorythmWithCounterCondition()
        {
            int counter = 0;

            AverageCounter averageCounter = new AverageCounter();
            while (GENRATION_NUMBERS_STOP_CONDITION > counter)
            {
                CreateNextPopulationCircle();

                averageCounter.SaveData(counter++, BestIndividual.Fitnes, CountAverageCost(), FindWorstCost());
            }

            return averageCounter;
        }

        private void CreateNextPopulationCircle()
        {
            //DoMultiTournamentSelection(NUMBER_OF_TOURNAMENT_PARTICIPANTS);
            DoRuletkaSelection();
            DoHybridization(); // krzyrzowa
            DoMutation();
            SaveBest();
        }

        private void DoHybridization()
        {
            #region Select pivot

            int pivotIndex = SelectRandomPivot();
            //or at middle :
            //int pivotIndex = Dimension / 2;

            #endregion Select pivot

            // я не делаю ремонт, потому что я не делаю скрещивание на тех элементах, которых нет на новой таблице
            // зачем ломать , а потом чинить, если можно сразу не ломать ? :)
            CreateNewIndividuals(pivotIndex);
        }

        private void CreateNewIndividuals(int pivot)
        {
            int halfPopulation = POPULATION_SIZE / 2;
            for (int i = 0; i < halfPopulation; i += 2)
            {
                int randomNumber = random.Next(MAX_PROBABILITY);
                if (HYBRIDIZATION_PROBABILITY > randomNumber)
                {
                    CreateNewPairIndividuals(pivot, individuals.ElementAt(i), individuals.ElementAt(i + 1));
                }

                #region Recount cost for changed permutation

                individuals.ElementAt(i).Fitnes = CostCounter.CountCost(individuals.ElementAt(i).Permutation);
                individuals.ElementAt(i + 1).Fitnes = CostCounter.CountCost(individuals.ElementAt(i + 1).Permutation);

                #endregion Recount cost for changed permutation
            }
        }

        private void CreateNewPairIndividuals(int indexTo, Individual firstIndividual, Individual secondIndividual)
        {
            for (int i = 0; i < indexTo; i++)
            {
                int secondIndividualPermutationElementAtCurrentIndexI = secondIndividual.Permutation.ElementAt(i);
                int[] hybridizationTmpArray = new int[indexTo];
                Array.Copy(firstIndividual.Permutation, hybridizationTmpArray, indexTo);
                if (WasHere(hybridizationTmpArray, secondIndividualPermutationElementAtCurrentIndexI))
                {
                    Permutator.Swap(firstIndividual.Permutation, i, FindThisNumberInArray(firstIndividual.Permutation, secondIndividualPermutationElementAtCurrentIndexI));
                    Permutator.SwapBeetweenArrays(firstIndividual.Permutation, secondIndividual.Permutation, i);
                }
            }
        }

        private int FindThisNumberInArray(int[] permutation, int value)
        {
            int premutationSize = permutation.Length;
            for (int i = 0; i < premutationSize; i++)
            {
                if (permutation[i] == value)
                {
                    return i;
                }
            }
            return NOT_FOUND_INDEX;
        }

        /// <summary>
        /// Checks , if value was in array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool WasHere(int[] array, int value)
        {
            foreach (var item in array)
            {
                if (item == value)
                    return true;
            }
            return false;
        }

        private int SelectRandomPivot()
        {
            int randomPivot = random.Next(1, Dimension - 2);
            return randomPivot;
        }

        private void DoMutation()
        {
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                Individual tmp = individuals.ElementAt(i);
                for (int j = 0; j < Dimension; j++)
                {
                    int randomNumber = random.Next(MAX_PROBABILITY);
                    if (MUTATION_PROBABILITY > randomNumber)
                    {
                        int randomIndex = random.Next(Dimension);
                        while (j == randomIndex)
                        {
                            randomIndex = random.Next(Dimension);
                        }
                        //MUTATE
                        Permutator.Swap(tmp.Permutation, j, randomIndex);
                        tmp.Fitnes = CostCounter.CountCost(tmp.Permutation);
                    }
                }
            }
        }

        private void SaveBest()
        {
            //check is best still best
            for (int i = 0; i < POPULATION_SIZE; i++)
            {
                if (BestIndividual.Fitnes > individuals.ElementAt(i).Fitnes)
                {
                    BestIndividual = (Individual)individuals.ElementAt(i).Clone();
                }
            }
        }

        public double CountAverageCost()
        {
            int sumCost = 0;
            foreach (var item in individuals)
            {
                sumCost += item.Fitnes;
            }
            return sumCost / POPULATION_SIZE;
        }

        public int FindWorstCost()
        {
            int worstCost = individuals.FirstOrDefault().Fitnes;
            foreach (var item in individuals)
            {
                if (worstCost < item.Fitnes)
                {
                    worstCost = item.Fitnes;
                }
            }
            return worstCost;
        }
    }
}