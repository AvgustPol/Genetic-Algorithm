﻿#define Generator
//#define TspKnp
//#undef Generator
#undef TspKnp

using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Сommon;
using GeneticAlgorithmLogic.Сommon.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing
{
    public class SimulatedAnnealing : Metaheuristic
    {
        public static SimulatedAnnealingParameters SimulatedAnnealingParameters;

        public static int LowTemperatureCounter { get; set; }

        public static void TryAvoidLocalOptimum(ref Individual best, ref IndividualTspKnp tmpCandidate, double temperature)
        {
            //отнимаем tmpCandidate - best, потому что для данной проблемы best = biggest
            double expValue = Math.Exp((tmpCandidate.Fitness - best.Fitness) / temperature);
            double randomValue = Randomizer.Random.NextDouble();

            //if random[0, 1] < exp{ (f(Vn)– f(Vc))/ T}
            if (randomValue < expValue)
            {
                best = tmpCandidate;
            }
        }

        public static void DecreaseTemperature(ref double temperature, int generationsCounter)
        {
            temperature *= 0.995;

            //golden ratio
            //temperature *= 0.61803398875;

            //if (temperature < 0.5)
            //{
            //    if (LowTemperatureCounter > 500)
            //    {
            //        temperature = SimulatedAnnealingParameters.InitializeTemperature;
            //        LowTemperatureCounter = 0;
            //    }
            //    temperature *= 0.99;
            //    LowTemperatureCounter++;
            //}
        }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters)
        {
            ItIsTimeToStopAlg = false;
            int nonChangeFitnessCounter = 0;

            SimulatedAnnealingParameters = (SimulatedAnnealingParameters)algorithmParameters;

            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();

            double currentTemperature = SimulatedAnnealingParameters.InitializeTemperature;
            Individual best;
#if Generator
            best = new IndividualHybryd();
#endif
#if TspKnp
            best = new IndividualTspKnp(IndividualTspKnp.CreateRandomIndividual()); ;
#endif

            List<int[]> neighbors;

            double bestAlgorithmFitness = best.Fitness;
            double averageNeighborFitness = best.Fitness;

            List<double> neighborsFitness = new List<double>(SimulatedAnnealingParameters.NumberOfNeighbors);

            do
            {
                neighbors = best.GetNeighbors(SimulatedAnnealingParameters.NumberOfNeighbors);

                foreach (var neighborsRoad in neighbors)
                {
#if Generator
                    IndividualHybryd neighbor = new IndividualHybryd(neighborsRoad);
#endif
#if TspKnp
                    IndividualTspKnp neighbor = new IndividualTspKnp(neighborsRoad);
#endif

                    #region Save to neighbors fitness list

                    neighborsFitness.Add(neighbor.Fitness);

                    #endregion Save to neighbors fitness list

                    if (neighbor.Fitness > best.Fitness)
                    {
                        best = neighbor;
                    }
                    else
                        //тут понижаем лушего!
                        TryAvoidLocalOptimum(ref best, ref neighbor, currentTemperature);
                }

                averageNeighborFitness = neighborsFitness.Average();
                neighborsFitness.Clear();

                bestAlgorithmFitness = best.Fitness;

                metaheuristicResult.SaveBestFitnessForCurrentGeneration(bestAlgorithmFitness);

                DecreaseTemperature(ref currentTemperature, ++_generationsCounter);

                if (metaheuristicResult.Fitness.ListBest.Count > 1)
                {
                    int currentGenerationFitness = metaheuristicResult.Fitness.ListBest.Count - 1;
                    int previousGenerationFitness = metaheuristicResult.Fitness.ListBest.Count - 2;

                    if (metaheuristicResult.Fitness.ListBest.ElementAt(currentGenerationFitness) == metaheuristicResult.Fitness.ListBest.ElementAt(previousGenerationFitness))
                    {
                        nonChangeFitnessCounter++;
                        if (nonChangeFitnessCounter > GlobalParameters.NumberOfNonchangedFitness)
                        {
                            ItIsTimeToStopAlg = true;
                        }
                    }
                    else
                    {
                        nonChangeFitnessCounter = 0;
                    }
                }
            } while (!ItIsTimeToStopAlg);

            return metaheuristicResult;
        }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters, int generationsNumber)
        {
            SimulatedAnnealingParameters = (SimulatedAnnealingParameters)algorithmParameters;
            _generationsCounter = 0;

            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();

            double currentTemperature = SimulatedAnnealingParameters.InitializeTemperature;
            Individual best;
#if Generator
            best = new IndividualHybryd(); ;
#endif
#if TspKnp
            best = new IndividualTspKnp(Population.CreateRandomIndividual());
#endif

            List<int[]> neighbors;

            double bestAlgorithmFitness = best.Fitness;
            double averageNeighborFitness = best.Fitness;

            List<double> neighborsFitness = new List<double>(SimulatedAnnealingParameters.NumberOfNeighbors);

            do
            {
                neighbors = best.GetNeighbors(SimulatedAnnealingParameters.NumberOfNeighbors);

                foreach (var neighborsRoad in neighbors)
                {
                    IndividualTspKnp neighbor = new IndividualTspKnp(neighborsRoad);

                    #region Save to neighbors fitness list

                    neighborsFitness.Add(neighbor.Fitness);

                    #endregion Save to neighbors fitness list

                    if (neighbor.Fitness > best.Fitness)
                    {
                        best = neighbor;
                    }
                    else
                        //тут понижаем лушего!
                        TryAvoidLocalOptimum(ref best, ref neighbor, currentTemperature);
                }

                averageNeighborFitness = neighborsFitness.Average();
                neighborsFitness.Clear();

                bestAlgorithmFitness = best.Fitness;

                metaheuristicResult.SaveBestFitnessForCurrentGeneration(bestAlgorithmFitness);
                metaheuristicResult.SaveAverageFitnessForCurrentGeneration(averageNeighborFitness);

                //TODO - change to separate method
                // because method "SaveWorstFitnessForCurrentGeneration" must save WorstFitness, not currentTemperature
                metaheuristicResult.SaveWorstFitnessForCurrentGeneration(currentTemperature);

                DecreaseTemperature(ref currentTemperature, ++_generationsCounter);
                //if (currentTemperature < 0.5)
                //    break;
            } while (_algoritmStopCondition);

            return metaheuristicResult;
        }
    }
}