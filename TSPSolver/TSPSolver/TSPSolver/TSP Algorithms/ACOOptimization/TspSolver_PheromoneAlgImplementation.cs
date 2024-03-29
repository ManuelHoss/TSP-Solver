﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.ACOOptimization
{
   public class TspSolver_PheromoneAlgImplementation : ITspSolver
   {
      public const int NumberOfAnts = 5;
      public const int Iterations = 200;
      public const double PheromoneRelevance = 0.3;
      public const double DistanceRelevance = 0.3;
      public const double EvapourationRate = 0.001;

      public OptimizationAlgorithmLog AlgorithmLog { get; } = new OptimizationAlgorithmLog() {AlgorithmName = "Ant Colony Optimization"};

      private List<Ant> _ants = new List<Ant>();
      private Dictionary<Address, Dictionary<Address, double>> _adjacencyMatrix;
      private Dictionary<Address, Dictionary<Address, double>> _pheromoneMatrix;
      private Route _bestRoute = new Route() { Distance = Double.MaxValue };
      
      public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> adjacencyMatrix, List<Address> addresses, Address depotAddress)
      {
         _adjacencyMatrix = adjacencyMatrix;
         // Initialize pheromone matrix with 1
         InitializePheromoneMatrix(addresses);

         for (int i = 0; i < NumberOfAnts; i++)
         {
            _ants.Add(new Ant(depotAddress, addresses, this));
         }

         StartOptimization();

         return BestRoute;
      }

      private void InitializePheromoneMatrix(List<Address> addresses)
      {
         _pheromoneMatrix = new Dictionary<Address, Dictionary<Address, double>>();
         for (int i = 0; i < addresses.Count; i++)
         {
            Dictionary<Address, double> tempDictionary = new Dictionary<Address, double>();
            for (int j = 0; j < addresses.Count; j++)
            {
               if (i != j)
               {
                  tempDictionary.Add(addresses.ElementAt(j), 1);
               }
            }
            _pheromoneMatrix.Add(addresses.ElementAt(i), tempDictionary);
         }
      }

      public Dictionary<Address, Dictionary<Address, double>> AdjacencyMatrix
      {
         get { return _adjacencyMatrix; }
         set { _adjacencyMatrix = value; }
      }

      public Dictionary<Address, Dictionary<Address, double>> PheromoneMatrix
      {
         get { return _pheromoneMatrix; }
         set { _pheromoneMatrix = value; }
      }

      public Route BestRoute
      {
         get { return _bestRoute; }
         set { _bestRoute = value; }
      }

      public void StartOptimization()
      {
         var acoStopwatch = Stopwatch.StartNew();
         for (int i = 0; i < Iterations; i++)
         {
            var iterationStopWatch = Stopwatch.StartNew();
            foreach (var ant in _ants)
            {
               Route route = ant.FindRoute();
               if (ant.BestRoute.Distance < BestRoute.Distance)
               {
                  BestRoute = ant.BestRoute;
               }
               Debug.WriteLine($"Best route from ant {ant}: {ant.BestRoute} - with length: {ant.BestRoute.Distance}");
               UpdatePheromoneMatrix(route);
            }
            AlgorithmLog.Iterations.Add(new Iteration() { BestRoute = BestRoute, EvaluationDuration = iterationStopWatch.ElapsedMilliseconds});
         }
         acoStopwatch.Stop();
         AlgorithmLog.EvaluationDuration = acoStopwatch.ElapsedMilliseconds;
         AlgorithmLog.BestRoute = BestRoute;
         BestRoute.OptimizationAlgorithmLog = AlgorithmLog;
      }

      private void UpdatePheromoneMatrix(Route route)
      {
         // Every route looses pheromone intesity in every iteration
         foreach (var key in PheromoneMatrix.Keys.ToList())
         {
            foreach (var innerKey in PheromoneMatrix[key].Keys.ToList())
            {
               PheromoneMatrix[key][innerKey] = (1 - EvapourationRate) * PheromoneMatrix[key][innerKey];
            }
         }

         // All routes that are used, are covered by new pheromones
         for (int i = 0; i < route.Addresses.Count - 1; i++)
         {
            PheromoneMatrix[route.Addresses[i]][route.Addresses[i + 1]] += 1 / route.Distance;
         }
      }
   }
}