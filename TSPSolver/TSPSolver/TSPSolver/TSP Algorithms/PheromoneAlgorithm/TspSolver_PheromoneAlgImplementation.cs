using System;
using System.Collections.Generic;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.PheromoneAlgorithm
{
   class TspSolver_PheromoneAlgImplementation : ITspSolver
   {
      public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, List<Address> addresses, Address depotAddress)
      {
         throw new NotImplementedException();
      }
   }
}
