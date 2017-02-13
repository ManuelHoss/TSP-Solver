using System;
using System.Text;

namespace TSPSolver.Model
{
   public class Address
   {
      public Guid Id { get; set; } = Guid.NewGuid();
      public string FormattedAddress { get; set; }

      public string FormattedAddressWithBreak
      {
         get
         {
            String str = FormattedAddress;
            String[] substrings = str.Split(',');
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(substrings[0]);
            stringBuilder.Append(",\n");
            for (int i = 1; i < substrings.Length; i++)
            {
               if (i > 1)
               {
                  stringBuilder.Append($", {substrings[i]}");
               }
               else
               {
                  stringBuilder.Append(substrings[i].Remove(0, 1));
               }
            }
            return stringBuilder.ToString();
         }
      }

      public bool IsDepotAddress { get; set; } = false;

      // Timewindows
      public DateTime TimeWindowsStart { get; set; }
      public DateTime TimeWindowsEnd { get; set; }
      public TimeSpan ArrivalTime { get; set; }

      public override string ToString()
      {
         return FormattedAddress;
      }
   }
}
