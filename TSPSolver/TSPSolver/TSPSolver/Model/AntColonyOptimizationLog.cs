using System;
using System.Collections.Generic;

namespace TSPSolver.Model
{
   public class AntColonyOptimizationLog
   {
      public List<Iteration> Iterations { get; set; }

      public double DistanceOfShortestRoute { get; set; }

      public Route BestRoute { get; set; }

      public TimeSpan EvaluationDuration { get; set; }
   }

   public class Iteration
   {
      public Route BestRoute { get; set; }
      public TimeSpan EvaluationDuration { get; set; }
   
   }
}
