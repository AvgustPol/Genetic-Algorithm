using DataModel;
using System;
using System.Globalization;
using System.IO;

namespace GeneticAlgorithm
{
    public class ToFileLogger
    {
        private string Path => _folderPath + _fileName;
        private readonly string _fileName;
        private readonly string _folderPath = GlobalParameters.PathToResultFolder;

        public ToFileLogger(string fileName)
        {
            _fileName = fileName;

            //delete old data if exists
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        private void AddParametersData()
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
        }

        public void LogTSToFile(AllGenerationsStatistics averageCounter)
        {
            //AddParametersData();

            File.AppendAllLines(Path,
                new[] {
                    $"TS Best Fitness" + "," +

                    $"TS Best neighbor Fitness"
                });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] {
                        $"{SaveValue(averageCounter.BestFitnessListTS[i])}" + "," +
                        $"{SaveValue(averageCounter.BestNeighborFitnessListTS[i])}"
                    });
            }
        }

        internal void LogGAToFile(AllGenerationsStatistics averageCounter)
        {
            AddParametersData();

            File.AppendAllLines(Path,
                new[] {
                    $"GA Best Fitness" + "," +
                    $"GA Average Fitness" + "," +
                    $"GA Worst Fitness"
                });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] {
                        $"{SaveValue(averageCounter.BestFitnessListGA[i])}" + "," +
                        $"{SaveValue(averageCounter.AverageFitnessListGA[i])}" + "," +
                        $"{SaveValue(averageCounter.WorstFitnessListGA[i])}"
                    });
            }
        }

        public void LogSaToFile(AllGenerationsStatistics averageCounter)
        {
            //AddParametersData();

            File.AppendAllLines(Path,
                new[] {
                    $"SA Best Fitness" + "," +

                    $"SA Best neighbor Fitness"
                });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] {
                        $"{SaveValue(averageCounter.BestFitnessListSA[i])}" + "," +
                        $"{SaveValue(averageCounter.BestNeighborFitnessListSA[i])}"
                    });
            }
        }

        public void LogToFile(AllGenerationsStatistics averageCounter)
        {
            AddParametersData();

            File.AppendAllLines(Path,
                new[] {
                    $"Generation Number" + "," +

                    $"GA Best Fitness" + "," +
                    $"GA Average Fitness" + "," +
                    $"GA Worst Fitness" + "," +

                    $"TS Best Fitness" + "," +

                    $"SA Best Fitness"
                });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] {
                            $"{i + 1}," +
                            $"{SaveValue(averageCounter.BestFitnessListGA[i])}" + "," +
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