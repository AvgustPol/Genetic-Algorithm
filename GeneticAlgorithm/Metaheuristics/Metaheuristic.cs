using DataModel;

namespace GeneticAlgorithm.Metaheuristics
{
    public abstract class Metaheuristic
    {
        //public class AlgorithmRunParam
        //{
        //    public int AlgorithmStopCondition { get; set; }
        //    public int ExploringAlgorithmStopCondition { get; set; }
        //}

        protected bool _algoritmStopCondition => _generationsCounter < GlobalParameters.AlgorithmStopCondition;
        protected bool _totalStopCondition => _generationsCounter < GlobalParameters.NumberOfRuns;
        protected int _generationsCounter;

        /// <summary>
        /// Starts algorithm
        /// запускает алгоритм с параметрами</summary>
        /// и возразщает результат в<param name="algorithmParameters"></param>
        /// <returns></returns>
        public abstract object Run(object algorithmParameters);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="allAlgorithmsResult"></param>
        /// <returns></returns>
        protected abstract object CalculateAverageForAllRunsOfTheAlgorithm(object allAlgorithmsResult);
    }
}