using System;

namespace GeneticAlgorithmLogic.Сommon
{
    public static class EuclideanDistance
    {
        public static double FindEuclideanDistance(Place x, Place y)
        {
            double sum = Math.Pow((x.PozitionX - y.PozitionX), 2) + Math.Pow((x.PozitionY - y.PozitionY), 2);
            return Math.Sqrt(sum);
        }
    }
}