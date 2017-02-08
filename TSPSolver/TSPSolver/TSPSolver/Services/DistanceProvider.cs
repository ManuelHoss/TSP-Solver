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
         int i = 1;
         foreach (var address in addresses)
         {
            stringBuilder.Append($"{address.Street}+{address.Number}+{address.City}+{address.City}|");
            if (i == addresses.Count)
            {
               // Remove last "|"
               stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            i++;
         }
         stringBuilder.Append("&destinations=");
         i = 1;
         foreach (var address in addresses)
         {
            stringBuilder.Append($"{address.Street}+{address.Number}+{address.City}+{address.City}|");
            if (i == addresses.Count)
            {
               // Remove last "|"
               stringBuilder.Remove(stringBuilder.Length - 1, 1);
            }
            i++;
         }
         stringBuilder.Append("&language=de-DE&key=AIzaSyBQwOackntfrhkKycA-iq5TOeES3ykOkek");

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

      async Task<String> AccessTheWebAsync(String url)
      {
         HttpClient client = new HttpClient();
         
         Task<string> getStringTask = client.GetStringAsync(url);
         
         string urlContents = await getStringTask;
         
         return urlContents;
      }

   }
}
