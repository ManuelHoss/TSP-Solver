﻿using System.Collections.Generic;
using System.Linq;
using TSPSolver.Interfaces;
using TSPSolver.Model;
using TSPSolver.TSP_Algorithms.ACOOptimization;
using TSPSolver.TSP_Algorithms.DynamicProgramming;

namespace TSPSolver.Services
{
   public class TspService
   {
      #region Fields
      

      #endregion //Fields
      
      public TspService()
      {
      }

      public Route CalculateBestRoute(List<Address> addresses, Address depotAddress)
      {
         AdjacencyMatrix adjacencyMatrix = GetDistancesFromGoogleApi(addresses);
         Dictionary<Address, Dictionary<Address, double>> distanceMatrix = ParseAdjacencyMatrixToDistanceMatrix(adjacencyMatrix, addresses);
         Dictionary<Address, Dictionary<Address, double>> durationMatrix = ParseAdjacencyMatrixToDurationMatrix(adjacencyMatrix, addresses);

         // Create Solver and start calculation
         ITspSolver tspSolver = new TspSolver_PheromoneAlgImplementation();
         return tspSolver.CalculateShortestRoute(distanceMatrix, durationMatrix, addresses, depotAddress);
      }

      private AdjacencyMatrix GetDistancesFromGoogleApi(List<Address> addresses)
      {
         return GoogleProvider.GetDistancesAsync(addresses).Result;
      }

      private Dictionary<Address, Dictionary<Address, double>> ParseAdjacencyMatrixToDistanceMatrix(AdjacencyMatrix matrix, List<Address> addresses)
      {
         Dictionary<Address, Dictionary<Address, double>> distancesMatrix = new Dictionary<Address, Dictionary<Address, double>>();

         for (int i = 0; i < addresses.Count; i++)
         {
            Dictionary<Address, double> tempMatrix = new Dictionary<Address, double>();
            for (int j = 0; j < addresses.Count; j++)
            {
               if (i != j && matrix.Rows[i] != null && matrix.Rows[i].elements[j] != null)
               {
                  tempMatrix.Add(addresses.ElementAt(j), matrix.Rows[i].elements[j].distance.value);
               }
            }
            distancesMatrix.Add(addresses.ElementAt(i), tempMatrix);
         }
         return distancesMatrix;
      }

      private Dictionary<Address, Dictionary<Address, double>> ParseAdjacencyMatrixToDurationMatrix(AdjacencyMatrix matrix, List<Address> addresses)
      {
         Dictionary<Address, Dictionary<Address, double>> durationMatrix = new Dictionary<Address, Dictionary<Address, double>>();

         for (int i = 0; i < addresses.Count; i++)
         {
            Dictionary<Address, double> tempMatrix = new Dictionary<Address, double>();
            for (int j = 0; j < addresses.Count; j++)
            {
               if (i != j)
               {
                  tempMatrix.Add(addresses.ElementAt(j), matrix.Rows[i].elements[j].duration.value);
               }
            }
            durationMatrix.Add(addresses.ElementAt(i), tempMatrix);
         }
         return durationMatrix;
      }
   }
}
