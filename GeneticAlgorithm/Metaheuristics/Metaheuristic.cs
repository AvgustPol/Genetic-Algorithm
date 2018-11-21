using DataModel;
using GeneticAlgorithmLogic.Metaheuristics.Parameters;

namespace GeneticAlgorithmLogic.Metaheuristics
{
    public abstract class Metaheuristic
    {
        public MetaheuristicParameters MetaheuristicParameters { get; set; }

        protected bool _algoritmStopCondition => _generationsCounter < GlobalParameters.AlgorithmStopCondition;
        protected bool _totalStopCondition => _generationsCounter < GlobalParameters.NumberOfRuns;
        protected int _generationsCounter;

        /// <summary>
        /// Starts algorithm
        /// запускает алгоритм с параметрами </summary>
        /// и возразщает результат в<param name="algorithmParameters"></param>
        /// <returns></returns>
        public abstract MetaheuristicResult Run(MetaheuristicParameters algorithmParameters);

        /// <summary>
        /// запускает N раз алгоритм
        /// считает и возращает средний результат для каждой генерации
        ///
        /// "средний результат" - является поводом, чтобы судить о алгоритме (Efektywność/Skuteczność)
        /// </summary>
        /// <param name="algorithmParameters"></param>
        /// <returns></returns>
        //public MetaheuristicResult RunNTimes(MetaheuristicParameters algorithmParameters)
        //{
        //    List<MetaheuristicResult> allLoopsData = new List<MetaheuristicResult>(GlobalParameters.NumberOfRuns);
        //    for (int i = 0; i < GlobalParameters.NumberOfRuns; i++)
        //    {
        //        MetaheuristicResult metaheuristicResult = Run(algorithmParameters);
        //        allLoopsData.Add(metaheuristicResult);
        //    }

        //    MetaheuristicResult allMetaheuristicsAverage = CalculateAverageForAllRunsOfTheAlgorithm(allLoopsData);

        //    //TODO:
        //    //CountStandardDeviation
        //}

        ///// <summary>
        ///// TODO
        ///// </summary>
        ///// <param name="allAlgorithmsResult"></param>
        ///// <returns></returns>
        //protected abstract object CalculateAverageForAllRunsOfTheAlgorithm(object allAlgorithmsResult);
    }
}