using DataModel;
using GeneticAlgorithmLogic.Metaheuristics.GeneticAlgorithm;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using System;
using System.Globalization;
using System.IO;

namespace GeneticAlgorithmLogic
{
    public class ToFileLogger
    {
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

        private string Path => _folderPath + _fileName;

        public void LogMetaheuristicToFile(MetaheuristicParameters.MetaheuristicType metaheuristicType, MetaheuristicParameters metaheuristicParameters, MetaheuristicResult metaheuristicResult)
        {
            AddGlobalParameters();
            LogMetaheuristicParameters(metaheuristicParameters);
            LogData(metaheuristicResult);
        }

        private void AddGlobalParameters()
        {
            File.AppendAllLines(Path,
                new[]
                {
                    $"Problem {GlobalParameters.FileName}",
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

        private void LogMetaheuristicParameters(MetaheuristicParameters metaheuristicParameters)
        {
            if (metaheuristicParameters is GeneticAlgorithmParameters parameters)
            {
                File.AppendAllLines(Path,
                    new[]
                    {
                        $"Genetic Algorithm " ,
                        $"Ilość osobników w populacji: {parameters.PopulationSize}" ,
                        $"Ilość uczestników  w turnieju: {parameters.NumberOfTournamentParticipants}" ,
                        $"Krzyżowanie: {parameters.CrossProbability}%" ,
                        $"Mutacja: {parameters.MutationProbability}%" ,
                    });
            }

            //if (metaheuristicParameters is SimulatedAnnealingParameters simulatedAnnealingParameters)
            //{
            //    File.AppendAllLines(Path,
            //        new[]
            //        {
            //            "Simulated Annealing" ,
            //            $"Początkowa temperatura T {simulatedAnnealingParameters.InitializeTemperature }" ,
            //            $"Początkowa temperatura T {simulatedAnnealingParameters.NumberOfNeighbors }"
            //        });
            //}

            //if (metaheuristicParameters is TabuSearchParameters tabuSearchParameters)
            //{
            //    //File.AppendAllLines(Path,
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