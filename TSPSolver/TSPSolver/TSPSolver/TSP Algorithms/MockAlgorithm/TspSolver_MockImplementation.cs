﻿using System;
using System.Collections.Generic;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.MockAlgorithm
{
   public class TspSolver_MockImplementation : ITspSolver
   {
      public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> adjacencyMatrix, List<Address> addresses, Address depotAddress)
      {
         throw new NotImplementedException();
      }
   }
}
