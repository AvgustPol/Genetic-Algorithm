using DataModel;
using System.Collections.Generic;

namespace GeneticAlgorithmLogic
{
    internal class AverageCounter
    {
        private const double _somethingWentWrongCode = -42.0;

        /// <summary>
        /// высчитывает среднее среди всех сохраненных финальных популяций
        /// для генерации номер generationNumber
        /// количество финальных популяций равно GlobalParameters.NumberOfRuns
        /// </summary>
        public static double CountAverageFitnessFor(List<MetaheuristicResult> allLoopsData, int generationNumber,
            string algorithmAndValueParameter)
        {
            double sum = 0;
            int counter = 0;

            foreach (var item in allLoopsData)
            {
                switch (algorithmAndValueParameter)
                {
                    case GlobalParameters.BestFitness:
                        sum += item.Fitness.ListBest[generationNumber];
                        break;

                    case GlobalParameters.AverageFitness:
                        sum += item.Fitness.ListAverage[generationNumber];
                        break;

                    case GlobalParameters.WorstFitness:
                        sum += item.Fitness.ListWorst[generationNumber];
                        break;

                    default:
                        return _somethingWentWrongCode;
                }

                counter++;
            }

            return counter > 0 ? sum / counter : 0;
        }
    }
}