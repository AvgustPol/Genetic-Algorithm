using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing;
using GeneticAlgorithmLogic.Сommon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GeneticAlgorithmLogic
{
    public class ToFileLogger
    {
        private const string _csvFileType = ".csv";
        private const string _txtFileType = ".txt";

        private readonly string _fileName;
        private readonly string _folderPath = GlobalParameters.PathToResultFolder;

        private string Path => _folderPath + _fileName + _csvFileType;
        private string AnaliticPath => _folderPath + "Analitic " + _fileName + _csvFileType;
        private string ParametersPath => _folderPath + "Parameters " + _fileName + _txtFileType;

        public ToFileLogger(string fileName)
        {
            _fileName = fileName;

            DeleteOldData(Path);
            DeleteOldData(AnaliticPath);
            DeleteOldData(ParametersPath);
        }

        /// <summary>
        /// delete old data if exists
        /// </summary>
        /// <param name="path"></param>
        private void DeleteOldData(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void LogMetaheuristicToFile(MetaheuristicParameters metaheuristicParameters, MetaheuristicResult metaheuristicResult)
        {
            LogParameters(metaheuristicParameters);
            LogData(metaheuristicResult);
        }

        private void LogGlobalParameters()
        {
            File.AppendAllLines(ParametersPath,
                new[]
                {
                    $"Problem {GlobalParameters.FileNames}",
                    $"Ilość generacji dla każdego algorytmu {GlobalParameters.IntegerAlgorithmStopCondition}",
                    $"Ilość uruchomień dla każdego algorytmu {GlobalParameters.NumberOfRuns},"
                });
        }

        private void LogData(MetaheuristicResult metaheuristicResult)
        {
            File.AppendAllLines(Path,
                new[] {
                    $"Best Fitness" + "," +
                    $"Average Fitness" + "," +
                    $"Worst Fitness"
                });

            int generationsNumber = metaheuristicResult.Fitness.ListBest.Count;

            for (int i = 0; i < generationsNumber; i++)
            {
                TryLogList(metaheuristicResult.Fitness.ListBest, i);
                TryLogList(metaheuristicResult.Fitness.ListAverage, i);
                TryLogList(metaheuristicResult.Fitness.ListWorst, i);
            }
        }

        private void TryLogList(List<double> list, int i)
        {
            if (list.Count > 0)
            {
                File.AppendAllLines(Path,
                    new[] {
                        $"{SaveValue(list[i])}" + ","
                    });
            }
        }

        public void LogAnalytic(Tuple<double, double, double> analizeResult)
        {
            if (analizeResult != null)
            {
                File.AppendAllLines(AnaliticPath,
                new[] {
                    $"Max" + "," +
                    $"Avg" + "," +
                    $"Dev"
                });

                File.AppendAllLines(AnaliticPath,
                new[] {
                    $"{analizeResult.Item1}" + "," +
                    $"{analizeResult.Item2}" + "," +
                    $"{analizeResult.Item3}"
                });
            }
        }

        private void LogParameters(MetaheuristicParameters metaheuristicParameters)
        {
            LogGlobalParameters();

            #region Add empty line

            File.AppendAllLines(ParametersPath,
                    new[]
                    {
                        "" ,
                    });

            #endregion Add empty line

            if (metaheuristicParameters is GeneticAlgorithmParameters parameters)
            {
                File.AppendAllLines(ParametersPath,
                    new[]
                    {
                        $"Genetic Algorithm " ,
                        $"Liczba osobników w populacji: {parameters.PopulationSize}" ,
                        $"Liczba uczestników  w turnieju: {parameters.NumberOfTournamentParticipants}" ,
                        $"Krzyżowanie: {parameters.CrossProbability}%" ,
                        $"Mutacja: {parameters.MutationProbability}%" ,
                    });
            }
            else if (metaheuristicParameters is SimulatedAnnealingParameters simulatedAnnealingParameters)
            {
                File.AppendAllLines(ParametersPath,
                    new[]
                    {
                        "Simulated Annealing" ,
                        $"Początkowa temperatura T {simulatedAnnealingParameters.InitializeTemperature }" ,
                        $"Początkowa temperatura T {simulatedAnnealingParameters.NumberOfNeighbors }"
                    });
            }

            //if (metaheuristicParameters is TabuSearchParameters tabuSearchParameters)
            //{
            //    //File.AppendAllLines(ParametersPath,
            //    //    new[]
            //    //    {
            //    //        $"Tabu Search" ,
            //    //        $"Rozmiar  listy tabu {TabuSearchParameters.TabuListSize}" ,
            //    //        $"Liczba sąsiedzi w jednej generacji {TabuSearchParameters.NumberOfNeighbors}" ,
            //    //        $"Poziom sąsiedztwa 1" ,
            //    //    });
            //}
        }

        private string SaveValue(double value)
        {
            return Convert.ToString(value, CultureInfo.CurrentCulture);
        }
    }
}