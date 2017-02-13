using System.Collections.Generic;
using TSPSolver.Model;

namespace TSPSolver.Interfaces
{
   public interface ITspSolver
   {
      Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> adjacencyMatrix, List<Address> addresses, Address depotAddress);
   }
}
