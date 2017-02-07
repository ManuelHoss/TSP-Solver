using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPSolver.Model
{
   public class AdjacencyMatrix
   {
      public string Status { get; set; }
      public List<string> OriginAddresses { get; set; }
      public List<string> DestinationAddresses { get; set; }
      public List<Row> Rows { get; set; }
   }
}
