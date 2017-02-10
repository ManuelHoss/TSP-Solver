using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace TSPSolver.CSV_Import
{
   public static class CsvHelper
   {
      public static async void ReadCsv()
      {
         FileData fileData = await CrossFilePicker.Current.PickFile();
         Stream stream = new MemoryStream(fileData.DataArray);
         
         using (var reader = new StreamReader(stream))
         {
            List<string> listA = new List<string>();
            List<string> listB = new List<string>();
            List<string> listC = new List<string>();
            List<string> listD = new List<string>();
            while (!reader.EndOfStream)
            {
               var line = reader.ReadLine();
               var values = line.Split(',');

               listA.Add(values[0]);
               listB.Add(values[1]);
               listC.Add(values[2]);
               listD.Add(values[3]);
            }
         }
      }
   }
}
