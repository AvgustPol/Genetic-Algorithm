namespace GeneticAlgorithm.Metaheuristics
{
    public abstract class Metaheuristic
    {
        /// <summary>
        /// Starts algorithm
        /// запускает алгоритм с параметрами</summary>
        /// и возразщает результат в<param name="algorithmParameters"></param>
        /// <returns></returns>
        public abstract object Run(object algorithmParameters);
    }
}