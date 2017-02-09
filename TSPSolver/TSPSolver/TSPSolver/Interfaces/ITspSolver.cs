using System.Collections.Generic;
using TSPSolver.Model;

namespace TSPSolver.Interfaces
{
   public interface ITspSolver
   {
      Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix, Dictionary<Address, Dictionary<Address, double>> durationMatrix, List<Address> addresses, Address depotAddress);
   }
}
