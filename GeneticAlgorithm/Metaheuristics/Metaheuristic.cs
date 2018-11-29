using GeneticAlgorithmLogic.Metaheuristics.Parameters;
using GeneticAlgorithmLogic.Сommon;

namespace GeneticAlgorithmLogic.Metaheuristics
{
    public abstract class Metaheuristic
    {
        protected bool ItIsTimeToStopAlg;

        public MetaheuristicParameters MetaheuristicParameters { get; set; }

        protected bool _algoritmStopCondition => _generationsCounter < GlobalParameters.IntegerAlgorithmStopCondition;
        protected int _generationsCounter;

        /// <summary>
        /// Starts algorithm
        /// запускает алгоритм с параметрами
        ///
        /// алгоритм работает до тех пор, пока N раз не поменяется фитнес.
        /// N =  GlobalParameters.NumberOfNonchangedFitness
        /// </summary>
        /// <param name="algorithmParameters"></param>
        /// <returns></returns>
        public abstract MetaheuristicResult Run(MetaheuristicParameters algorithmParameters);

        /// <summary>
        /// Starts algorithm
        /// запускает алгоритм с параметрами
        ///
        /// алгоритм работает generationsNumber
        /// </summary>
        /// <param name="algorithmParameters"></param>
        /// <param name="generationsNumber"></param>
        /// <returns></returns>
        public abstract MetaheuristicResult Run(MetaheuristicParameters algorithmParameters, int generationsNumber);
    }
}