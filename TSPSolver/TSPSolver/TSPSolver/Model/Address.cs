using System;

namespace TSPSolver.Model
{
   public class Address
   {
      public string Street { get; set; }
      public string Number { get; set; }
      public string Zip { get; set; }
      public string City { get; set; }

      // Timewindows
      public DateTime TimeWindowsStart { get; set; }
      public DateTime TimeWindowsEnd { get; set; }

      public override string ToString()
      {
         return $"{Street} {Number}, {Zip} {City}";
      }
   }
}
