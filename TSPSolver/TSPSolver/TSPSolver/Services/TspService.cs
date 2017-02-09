using System.Collections.Generic;
using System.Linq;
using TSPSolver.Interfaces;
using TSPSolver.Model;

namespace TSPSolver.Services
{
   public class TspService
   {
      #region Fields

      private readonly DistanceProvider _distanceProvider;

      #endregion //Fields
      
      public TspService()
      {
         _distanceProvider = new DistanceProvider();
      }

      public Route CalculateBestRoute(List<Address> addresses)
      {
         ITspSolver tspSolver = new TspSolver_MockImpl();
         AdjacencyMatrix adjacencyMatrix = GetDistancesFromGoogleApi(addresses);
         Dictionary<Address, Dictionary<Address, double>> distanceMatrix = ParseAdjacencyMatrixToDistanceMatrix(adjacencyMatrix, addresses);
         return tspSolver.CalculateShortestRoute(distanceMatrix, addresses, addresses[0]);
      }

      private AdjacencyMatrix GetDistancesFromGoogleApi(List<Address> addresses)
      {
         return _distanceProvider.GetDistancesAsync(addresses).Result;
      }

      private Dictionary<Address, Dictionary<Address, double>> ParseAdjacencyMatrixToDistanceMatrix(AdjacencyMatrix matrix, List<Address> addresses)
      {
         Dictionary<Address, Dictionary<Address, double>> distancesMatrix = new Dictionary<Address, Dictionary<Address, double>>();

         for (int i = 0; i < addresses.Count; i++)
         {
            Dictionary<Address, double> tempMatrix = new Dictionary<Address, double>();
            for (int j = 0; j < addresses.Count; j++)
            {
               if (i != j)
               {
                  tempMatrix.Add(addresses.ElementAt(j), matrix.Rows[i].elements[j].distance.value);
               }
            }
            distancesMatrix.Add(addresses.ElementAt(i), tempMatrix);
         }
         return distancesMatrix;
      }
   }
}
