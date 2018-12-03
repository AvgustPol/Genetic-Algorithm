using GeneticAlgorithmLogic.Metaheuristics.Parameters;

namespace GeneticAlgorithmLogic.Metaheuristics.Generator
{
    internal class Generator : Metaheuristic
    {
        public Metaheuristic Metaheuristic { get; set; }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters)
        {
            throw new System.NotImplementedException();
        }

        public override MetaheuristicResult Run(MetaheuristicParameters algorithmParameters, int generationsNumber)
        {
            throw new System.NotImplementedException();
        }
    }
}