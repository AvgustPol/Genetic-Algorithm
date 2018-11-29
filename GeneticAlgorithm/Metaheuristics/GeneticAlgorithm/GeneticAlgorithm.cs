using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Сommon;
using System.Linq;

namespace GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm
{
    public class GeneticAlgorithm : Metaheuristic
    {
        public static GeneticAlgorithmParameters GeneticAlgorithmParameters;

        public Population Population { get; set; }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters, int generationsNumber)
        {
            GeneticAlgorithmParameters = (GeneticAlgorithmParameters)algorithmParameters;
            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();

            CreatePopulation();

            SaveEffectiveness(metaheuristicResult, AlgorithmCoreParameters.Dimension);

            for (_generationsCounter = 0; _algoritmStopCondition; _generationsCounter++)
            {
                SaveEffectiveness(metaheuristicResult, SelectAndCross());
                SaveEffectiveness(metaheuristicResult, Mutate());
                CountFitness();
                SaveEffectiveness(metaheuristicResult, AlgorithmCoreParameters.Dimension);

                SaveFitnessData(metaheuristicResult);

                SaveEfficiency(metaheuristicResult, metaheuristicResult.Fitness.ListBest.Last());
            }

            return metaheuristicResult;
        }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters)
        {
            int nonChangeFitnessCounter = 0;

            GeneticAlgorithmParameters = (GeneticAlgorithmParameters)algorithmParameters;
            MetaheuristicResult metaheuristicResult = new MetaheuristicResult();

            CreatePopulation();

            SaveEffectiveness(metaheuristicResult, AlgorithmCoreParameters.Dimension);

            ItIsTimeToStopAlg = false; /// ! ! !
            while (!ItIsTimeToStopAlg)
            {
                SaveEffectiveness(metaheuristicResult, SelectAndCross());
                SaveEffectiveness(metaheuristicResult, Mutate());
                CountFitness();
                SaveEffectiveness(metaheuristicResult, AlgorithmCoreParameters.Dimension);

                SaveFitnessData(metaheuristicResult);

                SaveEfficiency(metaheuristicResult, metaheuristicResult.Fitness.ListBest.Last());

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
            }

            return metaheuristicResult;
        }

        private void SaveEfficiency(MetaheuristicResult metaheuristicResult, double fitness)
        {
            metaheuristicResult._metaheuristicIndicators.Efficiency = fitness;
        }

        private static void SaveEffectiveness(MetaheuristicResult metaheuristicResult, int fitnessCounts)
        {
            metaheuristicResult._metaheuristicIndicators.Effectiveness = +fitnessCounts;
        }

        public void SaveFitnessData(MetaheuristicResult metaheuristicResult)
        {
            metaheuristicResult.SaveBestFitnessForCurrentGeneration(Population.GetBestFitness());
            metaheuristicResult.SaveAverageFitnessForCurrentGeneration(Population.GetAverageFitness());
            metaheuristicResult.SaveWorstFitnessForCurrentGeneration(Population.GetWorstFitness());
        }

        private void CreatePopulation()
        {
            Population = new Population();
            Population.CreatePopulationIndividuals();
        }

        /// <summary>
        /// Counts fitness for each element in a population
        /// </summary>
        /// <param name="population"></param>
        private void CountFitness()
        {
            Population.CountFitnessForTheEntirePopulation();
        }

        private int Mutate()
        {
            return Population.Mutate();
        }

        private int SelectAndCross()
        {
            return Population.SelectAndCross();
        }

        //protected override object CalculateAverageForAllRunsOfTheAlgorithm(object allAlgorithmsResult)
        //{
        //    MetaheuristicResult allMetaheuristicsAverage = new MetaheuristicResult();

        //    #region Get GA MetaheuristicResult

        //    double averageBestFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.BestFitness);
        //    double averageAverageFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.AverageFitness);
        //    double averageWorstFitnessGA = AverageCounter.CountAverageFitnessFor(dataList, _generationsCounter, GlobalParameters.WorstFitness);

        //    #endregion Get GA MetaheuristicResult

        //    #region Save GA

        //    allMetaheuristicsAverage.SaveData(averageBestFitnessGA);
        //    allMetaheuristicsAverage.SaveAverageFitnessForGA(averageAverageFitnessGA);
        //    allMetaheuristicsAverage.SaveWorstFitnessForGA(averageWorstFitnessGA);

        //    #endregion Save GA

        //    return allMetaheuristicsAverage;
        //}
    }
}