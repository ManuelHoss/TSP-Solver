using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TSPSolver.Model;

namespace TSPSolver.Services
{
   public static class GoogleProvider
   {
      private static readonly HttpClient _client = new HttpClient();

      private static AdjacencyMatrix _adjacencyMatrix;
      private static GoogleMapsApiAddressResult _googleMapsApiAddressResult;

      public static async Task<AdjacencyMatrix> GetDistancesAsync(List<Address> addresses)
      {
         _adjacencyMatrix = new AdjacencyMatrix();
         
         StringBuilder uriString = new StringBuilder();
         uriString.Append("https://maps.googleapis.com/maps/api/distancematrix/json?");

         uriString.Append("origins=");
         // Add all addresses seperated by "|" as origin
         foreach (var address in addresses)
         {
            uriString.Append($"{address.FormattedAddress}|");
         }
         uriString.Remove(uriString.Length - 1, 1);
         uriString.Append("&destinations=");
         // Add all addresses seperated by "|" as destination
         foreach (var address in addresses)
         {
            uriString.Append($"{address.FormattedAddress}|");
         }
         uriString.Remove(uriString.Length - 1, 1);

         uriString.Append($"&language=de-DE&key={Constants.GoogleDistanceMatrixApiKey}");

         var uri = new Uri(uriString.ToString());

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

      public static async Task<GoogleMapsApiAddressResult> ValidateAddress(string addressToValidate)
      {
         bool addressIsValid = false;

         StringBuilder uriString = new StringBuilder();
         uriString.Append("https://maps.googleapis.com/maps/api/geocode/json?address=");
         uriString.Append(addressToValidate);

         var uri = new Uri(uriString.ToString());

         try
         {
            HttpResponseMessage response = _client.GetAsync(uri).Result;
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            _googleMapsApiAddressResult = JsonConvert.DeserializeObject<GoogleMapsApiAddressResult>(content);
         }
         catch (Exception ex)
         {
            Debug.WriteLine(@"ERROR {0}", ex.Message);
         }

         return _googleMapsApiAddressResult;
      }
   }
}
