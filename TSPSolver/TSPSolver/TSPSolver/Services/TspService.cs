using System.Collections.Generic;
using TSPSolver.Model;

namespace TSPSolver.Services
{
   public class TspService
   {
      private Dictionary<Address, Dictionary<Address, double>> _distanceMatrix;
      private DistanceProvider distanceProvider= new DistanceProvider();
      public TspService(List<Address> addresses)
      {
         AdjacencyMatrix matrix = distanceProvider.GetDistancesAsync(addresses).Result;
      }
   }
}
