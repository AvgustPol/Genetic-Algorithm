using System.Collections.Generic;

namespace GeneticAlgorithmLogic.Сommon
{
    public class GlobalParameters
    {
        /// <summary>
        /// указывает , сколько раз best fitness
        /// может не изменяться перед тем, как алгоритм перестанет работать
        /// </summary>
        public const int NumberOfNonchangedFitness = 2;

        /// <summary>
        /// STOP_CONDITION
        /// Number of generations that will be generated before stop.
        /// </summary>
        public const int IntegerAlgorithmStopCondition = 1000;

        /// <summary>
        /// STOP_CONDITION
        /// Number of runs algorcoreithm to exlore it
        /// </summary>
        public const int NumberOfRuns = 5;

        public const string PathToTestFolder = @"D:\7 semestr\Metaheurystyki\Data\ttp_student\";

        #region Source data

        //public static readonly List<string> FileNames = new List<string>() { "trivial_0.ttp", "easy_0.ttp" };

        public static readonly List<string> FileNames = new List<string>() { "easy_2.ttp", "easy_3.ttp", "medium_2.ttp", "medium_3.ttp", "hard_2.ttp" };

        #endregion Source data

        public static string PathToResultFolder = @"D:\7 semestr\Metaheurystyki\Data\Result\";

        public const string BestFitness = "Best Fitness ";
        public const string AverageFitness = "Average Fitness ";
        public const string WorstFitness = "Worst Fitness ";
    }
}