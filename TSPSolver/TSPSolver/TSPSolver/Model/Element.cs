using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPSolver.Model
{
   public class Element
   {
      public string status { get; set; }
      public Duration duration { get; set; }
      public Distance distance { get; set; }
   }
}
