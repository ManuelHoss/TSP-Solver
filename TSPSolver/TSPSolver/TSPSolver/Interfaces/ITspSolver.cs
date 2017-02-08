using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.Model;
using Xamarin.Forms;

namespace TSPSolver.Interfaces
{
   public interface ITspSolver
   {
      Route CalculateShortestRoute(Dictionary<Address, Dictionary<Address, double>> distanceMatrix);
   }
}
