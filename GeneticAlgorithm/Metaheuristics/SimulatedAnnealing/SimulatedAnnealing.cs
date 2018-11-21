using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using System;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing
{
    public class SimulatedAnnealing : Metaheuristic
    {
        public static SimulatedAnnealingParameters SimulatedAnnealingParameters;

        public static int LowTemperatureCounter { get; set; }

        public static void TryAvoidLocalOptimum(ref Individual best, ref Individual tmpCandidate, double temperature)
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
            temperature *= 0.95;

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
            SimulatedAnnealingParameters = (SimulatedAnnealingParameters)algorithmParameters;
            _generationsCounter = 0;

            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();

            double currentTemperature = SimulatedAnnealingParameters.InitializeTemperature;
            Individual best = new Individual(Population.CreateRandomIndividual());

            List<int[]> neighbors;

            double bestAlgorithmFitness = best.Fitness;
            double bestNeighborFitness = best.Fitness;

            do
            {
                neighbors = NeighborsGenerator.GetNeighbors(best, SimulatedAnnealingParameters.NumberOfNeighbors);

                foreach (var neighborsRoad in neighbors)
                {
                    Individual neighbor = new Individual(neighborsRoad);
                    if (neighbor.Fitness > best.Fitness)
                    {
                        best = neighbor;
                        bestNeighborFitness = neighbor.Fitness;
                    }
                    else
                        //тут понижаем лушего!
                        TryAvoidLocalOptimum(ref best, ref neighbor, currentTemperature);
                }

                if (bestNeighborFitness > bestAlgorithmFitness)
                {
                    bestAlgorithmFitness = bestNeighborFitness;
                }

                metaheuristicResult.SaveBestFitnessForCurrentGeneration(bestNeighborFitness);
                metaheuristicResult.SaveAverageFitnessForCurrentGeneration(bestAlgorithmFitness);

                //TODO - change to separate method .
                metaheuristicResult.SaveWorstFitnessForCurrentGeneration(currentTemperature);

                DecreaseTemperature(ref currentTemperature, ++_generationsCounter);
                //if (currentTemperature < 0.5)
                //    break;
            } while (_algoritmStopCondition);

            return metaheuristicResult;
        }
    }
}