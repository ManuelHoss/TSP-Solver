using System;
using System.Collections.Generic;
using PortableGeneticAlgorithm.Analytics;

namespace PortableGeneticAlgorithm.Interfaces
{
    public interface IAnalytics
    {
        Tuple<Analytics.Analytics.AggInfoType, double> EvaluateGeneration(List<Solution> jsonSolution);
    }
}