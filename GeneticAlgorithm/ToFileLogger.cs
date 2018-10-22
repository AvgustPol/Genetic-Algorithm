using DataModel;
using System.IO;

namespace GeneticAlgorithm
{
    public class ToFileLogger
    {
        private string Path => _folderPath + _fileName;
        private readonly string _folderPath = @"D:\7 semestr\Metaheurystyki\Data\Result\";
        private readonly string _fileName;

        public ToFileLogger(string fileName)
        {
            _fileName = fileName;

            //delete old data if exists
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
        }

        public void LogToFile(GenerationsStatistics averageCounter)
        {
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