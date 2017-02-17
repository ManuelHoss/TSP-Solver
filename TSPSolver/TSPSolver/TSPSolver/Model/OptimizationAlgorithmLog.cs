using System;
using System.Collections.Generic;

namespace TSPSolver.Model
{
   public class OptimizationAlgorithmLog
   {
      public string AlgorithmName { get; set; }

      public List<Iteration> Iterations { get; set; } = new List<Iteration>();

      public double DistanceOfShortestRoute { get; set; }

      public Route BestRoute { get; set; }

      public long EvaluationDuration { get; set; }
   }

   public class Iteration
   {
      public Route BestRoute { get; set; }
      public long EvaluationDuration { get; set; }
   }
}
