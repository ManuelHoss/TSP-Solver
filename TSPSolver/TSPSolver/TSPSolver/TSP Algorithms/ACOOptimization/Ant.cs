using System;
using System.Collections.Generic;
using System.Linq;
using TSPSolver.Model;

namespace TSPSolver.TSP_Algorithms.ACOOptimization
{
   public class Ant
   {
      public Route CurrentRoute { get; set; } = new Route();
      public Route BestRoute { get; set; }
      public List<Address> NotVisitedCities { get; set; }
      public List<Address> AllCities { get; set; }
      public Address StartCity { get; set; }
      public Address CurrentCity { get; set; }
      private TspSolver_PheromoneAlgImplementation _tspSolver;

      /// <summary>
      /// Constructor class for an Ant.
      /// </summary>
      /// <param name="startCity">City where the ant starts to search for a route.</param>
      /// <param name="addresses">All addresses the ant has to visit.</param>
      /// <param name="tspSolver"><see cref="TspSolver_PheromoneAlgImplementation"/> holds the distanceMatrix and pheromoneMatrix.</param>
      public Ant(Address startCity, List<Address> addresses, TspSolver_PheromoneAlgImplementation tspSolver)
      {
         StartCity = CurrentCity = startCity;
         CurrentRoute.Addresses.Add(StartCity);
         NotVisitedCities = addresses.ToList();
         _tspSolver = tspSolver;
         NotVisitedCities.Remove(startCity);
         AllCities = addresses.ToList();
         BestRoute = new Route() { Distance = double.MaxValue };
      }

      /// <summary>
      /// Searches for the route, which seems to be best, according to the current pheromone situation.
      /// </summary>
      /// <returns>A <see cref="Route"/> which seems to be best, according to the current pheromone situation</returns>
      public Route FindRoute()
      {
         ResetTempParameters();
         // Find new route according to current pheromone situation
         CurrentRoute = new Route() { Addresses = { StartCity}, Distance = 0, Duration = 0 };
         while (CurrentRoute.Addresses.Count < AllCities.Count)
         {
            MoveToNextCity();
         }

         // Check if new best route is found
         if (CurrentRoute.Distance < BestRoute.Distance)
         {
            BestRoute.Addresses = CurrentRoute.Addresses.ToList();
            BestRoute.Distance = CurrentRoute.Distance;
            BestRoute.Duration = CurrentRoute.Duration;
         }

         return CurrentRoute;
      }

      /// <summary>
      /// Chooses a city according to the current pheromone situation, moves to this city and removes it from notvisited cities.
      /// </summary>
      private void MoveToNextCity()
      {
         // Calculate probabilities to move to city x (https://en.wikipedia.org/wiki/Ant_colony_optimization_algorithms)
         Dictionary<Address, double> probabilities = new Dictionary<Address, double>();
         foreach (var notVisitedCity in NotVisitedCities)
         {
            probabilities[notVisitedCity] = CalculateProbabilityForCity(notVisitedCity);
         }

         // Chose city by roulette wheel selection (https://en.wikipedia.org/wiki/Fitness_proportionate_selection)
         RouletteWheelSelection selection = new RouletteWheelSelection(probabilities.Values.ToArray());
         Address chosenCity = NotVisitedCities[selection.spin()];

         // Update DistanceMatrix
         //CurrentRoute.Distance += _tspSolver.DistanceMatrix[CurrentCity][chosenCity];

         // "Move" to chosen city and remove it from NotVisitedCities
         CurrentCity = chosenCity;
         CurrentRoute.Addresses.Add(chosenCity);
         NotVisitedCities.Remove(chosenCity);
      }

      /// <summary>
      /// Calculates the probability to move to a specified city according to the current pheromone intensity.
      /// </summary>
      /// <param name="targetCity">City to move to.</param>
      /// <returns>Probability depending on pheromone situation to move to tagetCity</returns>
      private double CalculateProbabilityForCity(Address targetCity)
      {
         //double invertedDistance = 1 / _tspSolver.DistanceMatrix[CurrentCity][targetCity];
         double pheromoneIntensity = _tspSolver.PheromoneMatrix[CurrentCity][targetCity];

         //double numerator = Math.Pow(pheromoneIntensity, TspSolver_PheromoneAlgImplementation.PheromoneRelevance) * Math.Pow(invertedDistance, TspSolver_PheromoneAlgImplementation.DistanceRelevance);

         double denominator = 0;
         foreach (var notVisitedCity in NotVisitedCities)
         {
            //denominator += Math.Pow(1 / _tspSolver.DistanceMatrix[CurrentCity][notVisitedCity], TspSolver_PheromoneAlgImplementation.PheromoneRelevance)* Math.Pow(_tspSolver.PheromoneMatrix[CurrentCity][notVisitedCity], TspSolver_PheromoneAlgImplementation.DistanceRelevance);
         }

         return 0.0;
      }

      /// <summary>
      /// Resets the parameter of the ant, bevor starting a new route.
      /// </summary>
      private void ResetTempParameters()
      {
         CurrentCity = StartCity;
         CurrentRoute = new Route();
         NotVisitedCities = AllCities.ToList();
         NotVisitedCities.Remove(StartCity);
      }
   }
}
