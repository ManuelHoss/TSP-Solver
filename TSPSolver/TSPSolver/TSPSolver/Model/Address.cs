using System;

namespace TSPSolver.Model
{
   public class Address
   {
      public Guid Id { get; set; } = Guid.NewGuid();
      public string Street { get; set; }
      public string Number { get; set; }
      public string Zip { get; set; }
      public string City { get; set; }
      public string FormattedAddress { get; set; }
      public bool IsDepotAddress { get; set; } = false;

      // Timewindows
      public DateTime TimeWindowsStart { get; set; }
      public DateTime TimeWindowsEnd { get; set; }
      public TimeSpan ArrivalTime { get; set; }

      public override string ToString()
      {
         return $"{Street} {Number}, {Zip} {City}";
      }
   }
}
