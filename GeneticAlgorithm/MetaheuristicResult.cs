using System.Collections.Generic;

namespace GeneticAlgorithmLogic
{
    /// <summary>
    /// ��������� ������ ��������� (��������������)
    /// result (best solution ever)
    ///
    /// </summary>
    /// <typeparam name="T">fitness datatype e.g. double</typeparam>
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
            /// Effectiveness (pl. Efektywnosc ) - best global fitness
            ///
            /// ������������ ����� ������ ������� ��������� (�������� ����� ������) .
            /// ����� �� ����������� ���� � ������������ ������ (����������)
            /// � ��������, ��� ������������ = ���������� ��������� ����� (�������)
            ///
            /// ������ osobnik - ��� punkt w przeszukiwanej przestszeni.
            /// Przestrzen (rozwiazan) - ��� ��������� ������� ��� ������ (�����, ������� �� ������ � ������ ���������) ��������
            ///
            /// � ������, ��� ���������� �������� �������� ��� ��������
            /// </summary>
            public int Effectiveness { get; set; }

            /// <summary>
            /// Efficiency (pl. Skutecznosc ) - number of scanned points
            ///
            /// ������ ��������� ������� �� ��� ����� ������ ���������
            /// </summary>
            public int Efficiency { get; set; }

            /// <summary>
            /// ������� ���������� ������������:
            /// ��������� ���������� �������� ����� � ������ ���������
            /// � ��������� ����������� ��� ����� ���������
            /// </summary>
            /// <param name="genetarionEfficiency"></param>
            public void SaveEffectiveness(int genetarionEffectiveness)
            {
                Effectiveness += genetarionEffectiveness;
            }

            /// <summary>
            /// if new global fitness is better than old one
            /// </summary>
            /// <param name="newEffectiveness"></param>
            public void TrySaveEfficiency(int newEffectiveness)
            {
                if (IsBetter(Efficiency, newEffectiveness))
                {
                    Effectiveness = newEffectiveness;
                }
            }

            private bool IsBetter(int oldEffectiveness, int newEffectiveness)
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