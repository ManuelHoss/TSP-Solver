using System.Collections.Generic;
using System.Threading.Tasks;

namespace TSPSolver.Interfaces
{
   public interface IGeocoder
   {
      Task<List<double>> GetGeoCoordinatesOfAddress(string address);
   }
}
