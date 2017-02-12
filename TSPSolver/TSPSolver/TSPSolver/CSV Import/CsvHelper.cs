using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using TSPSolver.Model;
using TSPSolver.Services;

namespace TSPSolver.CSV_Import
{
   public static class CsvHelper
   {
      public static async Task<List<Address>> ReadCsv()
      {
         List<Address> importAddresses = new List<Address>();

         FileData fileData = await CrossFilePicker.Current.PickFile();
         if (fileData != null)
         {
            Stream stream = new MemoryStream(fileData.DataArray);
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {

               while (!reader.EndOfStream)
               {
                  var line = reader.ReadLine();
                  var values = line.Split(',');

                  if (values.Length >= 4)
                  {
                     importAddresses.Add(new Address()
                     {
                        Street = values[0],
                        Number = values[1],
                        Zip = values[2],
                        City = values[3],
                     });
                  }
               }
            }
         }
         
         foreach (var address in importAddresses)
         {
            GoogleMapsApiAddressResult validationResult = GoogleProvider.ValidateAddress(address.ToString()).Result;

            if (validationResult.status == "OK")
            {
               address.Street = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("route")).long_name;
               address.Number = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("street_number")).long_name;
               address.City = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("locality")).long_name;
               address.Zip = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("postal_code")).long_name;
               address.FormattedAddress = validationResult.results[0].formatted_address;
            }
            else
            {
               throw new Exception($"Google Maps validation failed for address: {address}. Validation Status: {validationResult.status}. Change CSV file and try again.");
            }
         }

         return importAddresses;
      }
   }
}
