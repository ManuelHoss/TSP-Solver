using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TSPSolver.Model;

namespace TSPSolver.Services
{
   public class DistanceProvider
   {
      private readonly HttpClient _client = new HttpClient();

      private AdjacencyMatrix _adjacencyMatrix;

      public async Task<AdjacencyMatrix> GetDistancesAsync(List<Address> addresses)
      {
         _adjacencyMatrix = new AdjacencyMatrix();
         
         StringBuilder stringBuilder = new StringBuilder();
         stringBuilder.Append("https://maps.googleapis.com/maps/api/distancematrix/json?");

         stringBuilder.Append("origins=");
         // Add all addresses seperated by "|" as origin
         foreach (var address in addresses)
         {
            stringBuilder.Append($"{address.Street}+{address.Number}+{address.City}+{address.City}|");
         }
         stringBuilder.Remove(stringBuilder.Length - 1, 1);
         stringBuilder.Append("&destinations=");
         // Add all addresses seperated by "|" as destination
         foreach (var address in addresses)
         {
            stringBuilder.Append($"{address.Street}+{address.Number}+{address.City}+{address.City}|");
         }
         stringBuilder.Remove(stringBuilder.Length - 1, 1);

         stringBuilder.Append($"&language=de-DE&key={Constants.GoogleDistanceMatrixApiKey}");

         var uri = new Uri(stringBuilder.ToString());

         try
         {
            HttpResponseMessage response = _client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            _adjacencyMatrix = JsonConvert.DeserializeObject<AdjacencyMatrix>(content);
         }
         catch (Exception ex)
         {
            Debug.WriteLine(@"ERROR {0}", ex.Message);
         }

         return _adjacencyMatrix;
      }
   }
}
