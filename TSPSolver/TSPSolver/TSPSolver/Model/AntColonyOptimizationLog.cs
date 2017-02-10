using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TSPSolver.Model
{
   public class AntColonyOptimizationLog
   {
      public List<Iteration> Iterations { get; set; }

      public double DistanceOfShortestRoute { get; set; }
   }

   public class Iteration
   {
      public Route BestRoute { get; set; }
      public TimeSpan EvaluationDuration { get; set; }
   
   }
}
