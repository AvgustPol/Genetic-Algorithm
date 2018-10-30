using DataModel;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    internal class AverageCounter
    {
        private const double _somethingWentWrongCode = -42.0;

        /// <summary>
        /// высчитывает среднее среди всех сохраненных финальных популяций
        /// для генерации номер generationNumber
        /// количество финальных популяций равно GlobalParameters.ExploringAlgorithmStopCondition
        /// </summary>
        /// <param name="list">All </param>
        /// <param name="generationNumber"></param>
        /// <param name="algorithmAndValueParameter"></param>
        /// <returns></returns>
        public static double CountAverageFitnessFor(List<AllGenerationsStatistics> list, int generationNumber, string algorithmAndValueParameter)
        {
            double sum = 0;
            int counter = 0;
            foreach (var item in list)
            {
                switch (algorithmAndValueParameter)
                {
                    case GlobalParameters.BestFitnessListGA:
                        sum += item.BestFitnessListGA[generationNumber];
                        break;

                    case GlobalParameters.WorstFitnessListGA:
                        sum += item.WorstFitnessListGA[generationNumber];
                        break;

                    case GlobalParameters.AverageFitnessListGA:
                        sum += item.AverageFitnessListGA[generationNumber];
                        break;

                    case GlobalParameters.BestFitnessListTS:
                        sum += item.BestFitnessListTS[generationNumber];
                        break;

                    case GlobalParameters.BestFitnessListSA:
                        sum += item.BestFitnessListSA[generationNumber];
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