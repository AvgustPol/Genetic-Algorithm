using System.Collections.Generic;

namespace GeneticAlgorithmLogic
{
    /// <summary>
    /// Результат работы алгоритма (метахеуристики)
    /// result (best solution ever)
    ///
    /// </summary>
    public class MetaheuristicResult
    {
        public class FitnessResult
        {
            public List<double> ListBest { get; set; } = new List<double>();
            public List<double> ListAverage { get; set; } = new List<double>();
            public List<double> ListWorst { get; set; } = new List<double>();
        }

        public class MetaheuristicIndicators
        {
            /// <summary>
            /// Effectiveness (pl. Efektywnosc ) - number of scanned points
            ///
            /// Ефективность можно мерять разными способами (например время работы) .
            /// Чтобы не привязывать себя к возможностям машины (компьюрета)
            /// я принимаю, что Ефективность = количество изученных точек (пунктов)
            ///
            /// Каждый osobnik - это punkt w przeszukiwanej przestszeni.
            /// Przestrzen (rozwiazan) - все возможные решения для данной (такой, которую мы решаем в данной программе) проблемы
            ///
            /// В теории, это количество подсчета фитнесса для индивида
            /// </summary>
            public int Effectiveness { get; set; }

            /// <summary>
            /// Efficiency (pl. Skutecznosc ) - best global fitness
            ///
            /// Лучшее найденное решение за все время работы алгоритма
            /// </summary>
            public double Efficiency { get; set; }

            /// <summary>
            /// Процесс сохранения ефективности:
            /// Суммируем количество изученых точек в данной генерации
            /// с суммарным количеством для всего алгоритма
            /// </summary>
            /// <param name="genetarionEffectiveness"></param>
            public void SaveEffectiveness(int genetarionEffectiveness)
            {
                Effectiveness += genetarionEffectiveness;
            }

            /// <summary>
            /// if new global fitness is better than old one
            /// save new one 
            /// </summary>
            /// <param name="newEffectiveness"></param>
            public void TrySaveEfficiency(double newEffectiveness)
            {
                if (IsBetter(Efficiency, newEffectiveness))
                {
                    Efficiency = newEffectiveness;
                }
            }

            private bool IsBetter(double oldEffectiveness, double newEffectiveness)
            {
                //For current implementation best fintess = greatest
                return oldEffectiveness < newEffectiveness;
            }
        }

        //TODO : _fitnessResult should be private ?
        public readonly FitnessResult _fitnessResult = new FitnessResult();

        //TODO : _metaheuristicIndicators should be private ?
        public readonly MetaheuristicIndicators _metaheuristicIndicators = new MetaheuristicIndicators();

        /// <summary>
        /// Saves Best fitness for current generation
        /// </summary>
        /// <param name="currentGenerationBestFitness"></param>
        public void SaveBestFitnessForCurrentGeneration(double currentGenerationBestFitness)
        {
            _fitnessResult.ListBest.Add(currentGenerationBestFitness);
        }

        /// <summary>
        /// Saves Average fitness for current generation
        /// </summary>
        /// <param name="currentGenerationBestFitness"></param>
        public void SaveAverageFitnessForCurrentGeneration(double currentGenerationAverageFitness)
        {
            _fitnessResult.ListAverage.Add(currentGenerationAverageFitness);
        }

        /// <summary>
        /// Saves Worst fitness for current generation
        /// </summary>
        /// <param name="currentGenerationBestFitness"></param>
        public void SaveWorstFitnessForCurrentGeneration(double currentGenerationWorstFitness)
        {
            _fitnessResult.ListWorst.Add(currentGenerationWorstFitness);
        }
    }
}