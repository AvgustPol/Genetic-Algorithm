using DataModel;
using GeneticAlgorithm.Metaheuristics.SimulatedAnnealing;
using System;
using System.Globalization;
using System.IO;
using GeneticAlgorithm.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithm.Metaheuristics.TabuSearch;

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

        private void AddParametersData(GlobalParameters.AlgorithmType algorithmType)
        {
            File.AppendAllLines(Path,
                new[]
                {
                    $"Problem {GlobalParameters.FileName}",
                    $"Ilość generacji dla każdego algorytmu {GlobalParameters.AlgorithmStopCondition}",
                    $"Ilość uruchomień dla każdego algorytmu {GlobalParameters.ExploringAlgorithmStopCondition},"
                });

            switch (algorithmType)
            {
                case GlobalParameters.AlgorithmType.GA:
                    File.AppendAllLines(Path,
                        new[]
                        {
                            $"Genetic Algorithm" ,
                            $"Ilość osobników w populacji {GeneticAlgorithmParameters.PopulationSize}" ,
                            $"Ilość uczestników  w turnieju {GeneticAlgorithmParameters.NumberOfTournamentParticipants}" ,
                            $"Krzyżowanie  {GeneticAlgorithmParameters.CrossProbability}%" ,
                            $"Mutacja {GeneticAlgorithmParameters.MutationProbability}%" ,
                        });
                    break;

                case GlobalParameters.AlgorithmType.TS:
                    File.AppendAllLines(Path,
                        new[]
                        {
                            $"Tabu Search" ,
                            $"Rozmiar  listy tabu {TabuSearchParameters.TabuListSize}" ,
                            $"Ilość sąsiedzi w jednej generacji {TabuSearchParameters.NumberOfNeighbors}" ,
                            $"Poziom sąsiedztwa 1" ,
                        });
                    break;

                case GlobalParameters.AlgorithmType.SA:
                    File.AppendAllLines(Path,
                        new[]
                        {
                            "Simulated Annealing" ,
                            $"Początkowa temperatura T {SimulatedAnnealingParameters.InitializeTemperature }"
                        });
                    break;

                default:

                    break;
            }
        }

        public void LogTSToFile(LoopData<double> averageCounter)
        {
            File.AppendAllLines(Path,
                new[] {
                    //TODO #42
                    //сделать так, чтобы можно было написать
                    //
                    $"TS {LoopData<double>.TsDataType.Best} Fitness" + "," +

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

        internal void LogGAToFile(LoopData averageCounter)
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
                        $"{SaveValue(averageCounter.ListBest[i])}" + "," +
                        $"{SaveValue(averageCounter.ListOther[i])}" + "," +
                        $"{SaveValue(averageCounter.ListAvg[i])}"
                    });
            }
        }

        public void LogSaToFile(LoopData averageCounter)
        {
            AddParametersData();

            File.AppendAllLines(Path,
                new[] {
                    $"SA Best Fitness" + "," +

                    $"SA Best neighbor Fitness"
                });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] {
                        $"{SaveValue(averageCounter.SATemperature[i])}" + "," +
                        $"{SaveValue(averageCounter.BestFitnessListSA[i])}" + "," +
                        $"{SaveValue(averageCounter.BestNeighborFitnessListSA[i])}"
                    });
            }
        }

        public void LogToFile(GlobalParameters.AlgorithmType algorithmType, LoopData averageCounter)
        {
            AddParametersData(algorithmType);

            switch (algorithmType)
            {
                case GlobalParameters.AlgorithmType.GA:
                    algorithmResult = RunGA();
                    break;

                case GlobalParameters.AlgorithmType.SA:
                    algorithmResult = RunSA();
                    break;

                case GlobalParameters.AlgorithmType.TS:
                    algorithmResult = RunTS();
                    break;

                default:

                    break;
            }

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
                            $"{SaveValue(averageCounter.ListBest[i])}" + "," +
                            $"{SaveValue(averageCounter.ListOther[i])}" + "," +
                            $"{SaveValue(averageCounter.ListAvg[i])}" + "," +

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