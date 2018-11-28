using DataModel;
using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Metaheuristics.SimulatedAnnealing;
using System;
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

            //DeleteOldData(Path);
            //DeleteOldData(AnaliticPath);
            //DeleteOldData(ParametersPath);
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
                    $"Ilość generacji dla każdego algorytmu {GlobalParameters.AlgorithmStopCondition}",
                    $"Ilość uruchomień dla każdego algorytmu {GlobalParameters.NumberOfRuns},"
                });
        }

        private void LogData(MetaheuristicResult metaheuristicResult)
        {
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
                        $"{SaveValue(metaheuristicResult._fitnessResult.ListBest[i])}" + "," +
                        $"{SaveValue(metaheuristicResult._fitnessResult.ListAverage[i])}" + "," +
                        $"{SaveValue(metaheuristicResult._fitnessResult.ListWorst[i])}"
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
                        $"Ilość osobników w populacji: {parameters.PopulationSize}" ,
                        $"Ilość uczestników  w turnieju: {parameters.NumberOfTournamentParticipants}" ,
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
            //    //        $"Ilość sąsiedzi w jednej generacji {TabuSearchParameters.NumberOfNeighbors}" ,
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