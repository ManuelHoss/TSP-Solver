using System.Collections.Generic;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.Services
{
   public class TspSolver_MockImpl : ITspSolver
   {
      public Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, List<Address> addresses, Address depotAddress)
      {
         // Mock route
         List<Address> mockAddresses = new List<Address>();
         foreach (var distanceMatrixKey in distanceMatrix.Keys)
         {
            mockAddresses.Add(distanceMatrixKey);
         }
         double mockDistance = 0;
         foreach (var distanceMatrixValue in distanceMatrix.Values)
         {
            foreach (var value in distanceMatrixValue.Values)
            {
               mockDistance += value;
            }
         }
         return new Route() {Addresses = mockAddresses, Distance = mockDistance , Duration = 0};
      }
   }
}
