using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPSolver.Model
{
   public class Address
   {
      public string Street { get; set; }
      public string Number { get; set; }
      public string Zip { get; set; }
      public string City { get; set; }

      public override string ToString()
      {
         return $"{Street} {Number}, {Zip} {City}";
      }
   }
}
