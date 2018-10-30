using DataModel;
using System;
using System.Globalization;
using System.IO;

namespace GeneticAlgorithm
{
    public class ToFileLogger
    {
        private string Path => _folderPath + _fileNameName;
        private readonly string _fileNameName;
        private readonly string _folderPath = GlobalParameters.PathToResultFolder;

        public ToFileLogger(string fileName)
        {
            _fileNameName = fileName;

            //delete old data if exists
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        public void LogToFile(GenerationsStatistics averageCounter)
        {
            File.AppendAllLines(Path,
                    new[] {
                        $"Problem {GlobalParameters.FileName}" ,
                        $"Ilość generacji dla każdego algorytmu {GlobalParameters.AlgorithmStopCondition}" ,
                        $"Ilość uruchomień dla każdego algorytmu {GlobalParameters.ExploringAlgorithmStopCondition}," ,

                        "Tabu Search" ,
                        $"Rozmiar  listy tabu {TabuSearchParameters.TabuListSize}" ,
                        $"Ilość sąsiedzi w jednej generacji {TabuSearchParameters.NumberOfNeighbors}" ,
                        $"Poziom sąsiedztwa 1" ,

                        "Genetic Algorithm" ,
                        $"Ilość osobników w populacji {GeneticAlgorithmParameters.PopulationSize}" ,
                        $"Ilość uczestników  w turnieju {GeneticAlgorithmParameters.NumberOfTournamentParticipants}" ,
                        $"Krzyżowanie  {GeneticAlgorithmParameters.CrossProbability}%" ,
                        $"Mutacja {GeneticAlgorithmParameters.MutationProbability}%" ,

                        "Simulated Annealing" ,
                        $"Początkowa temperatura T {SimulatedAnnealingParameters.InitializeTemperature }"
                            });

            File.AppendAllLines(Path,
                new[] {
                    $"Generation Number" + "," +

                    $"Best Fitness GA" + "," +
                    $"Average Fitness GA" + "," +
                    $"Worst Fitness GA" + "," +

                    $"Best Fitness TS" + "," +

                    $"Best Fitness SA"
                });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] {
                            $"{i + 1}," +
                            $"{ SaveValue(averageCounter.BestFitnessListGA[i])}" + "," +
                            $"{SaveValue(averageCounter.AverageFitnessListGA[i])}" + "," +
                            $"{SaveValue(averageCounter.WorstFitnessListGA[i])}" + "," +

                            $"{SaveValue(averageCounter.BestFitnessListTS[i])}" + "," +

                            $"{SaveValue(averageCounter.BestFitnessListSA[i])}"
                    });
            }
        }

        private string SaveValue(double value)
        {
            return Convert.ToString(value, CultureInfo.CurrentCulture);
        }
    }
}