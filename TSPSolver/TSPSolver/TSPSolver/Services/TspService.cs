using System.Collections.Generic;
using System.Linq;
using TSPSolver.Model;

namespace TSPSolver.Services
{
   public class TspService
   {
      private Dictionary<Address, Dictionary<Address, double>> _distanceMatrix;
      private List<Address> _addresses;
      private DistanceProvider distanceProvider= new DistanceProvider();
      public TspService(List<Address> addresses)
      {
         _addresses = addresses;
         AdjacencyMatrix adjacencyMatrix = GetDistancesFromGoogleApi(_addresses);
         Dictionary<Address, Dictionary<Address, double>> distanceMatrix = parseAdjacencyMatrixToDistanceMatrix(adjacencyMatrix);
      }

      private AdjacencyMatrix GetDistancesFromGoogleApi(List<Address> addresses)
      {
         return distanceProvider.GetDistancesAsync(addresses).Result;
      }

      private Dictionary<Address, Dictionary<Address, double>> parseAdjacencyMatrixToDistanceMatrix(AdjacencyMatrix matrix)
      {
         Dictionary<Address, Dictionary<Address, double>> distancesMatrix = new Dictionary<Address, Dictionary<Address, double>>();

         for (int i = 0; i < _addresses.Count; i++)
         {
            Dictionary<Address, double> tempMatrix = new Dictionary<Address, double>();
            for (int j = 0; j < _addresses.Count; j++)
            {
               if (i != j)
               {
                  tempMatrix.Add(_addresses.ElementAt(j), matrix.Rows[i].elements[j].distance.value);
               }
            }
            distancesMatrix.Add(_addresses.ElementAt(i), tempMatrix);
         }
         return distancesMatrix;
      }
   }
}
