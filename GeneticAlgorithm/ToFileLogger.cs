using DataModel;
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
                        $"Ilość uruchomień dla każdego algorytmu {GlobalParameters.ExploringAlgorythmStopCondition}," ,
                        $"Tabu Search" ,
                        $"Rozmiar  listy tabu {GlobalParameters.SameSize}" ,
                        $"Ilość sąsiedzi w jednej generacji {GlobalParameters.SameNumber}" ,
                        $"Poziom sąsiedztwa 1" ,
                        $"Genetic Algorithm" ,
                        $"Ilość osobników w populacji {GlobalParameters.SameSize}" ,
                        $"Ilość uczęstników w turnieju {GlobalParameters.SameNumber}" ,
                        $"Krzyżowanie  {GeneticAlgorithmParameters.CrossProbability}%" ,
                        $"Mutacja {GeneticAlgorithmParameters.MutationProbability}%"
                            });

            File.AppendAllLines(Path,
                new[] { $"Generation Number," +
                        $"Best Fitness GA ," +
                        $"Average Fitness GA," +
                        $"Worst Fitness GA, " +

                        $"Best Fitness TS", });

            for (int i = 0; i < GlobalParameters.AlgorithmStopCondition; i++)
            {
                File.AppendAllLines(Path,
                    new[] { $"{i + 1},{averageCounter.BestFitnessListGA[i]}," +
                            $"{averageCounter.AverageFitnessListGA[i]}," +
                            $"{averageCounter.WorstFitnessListGA[i]}," +

                            $"{averageCounter.BestFitnessListTS[i]}"});
            }
        }
    }
}