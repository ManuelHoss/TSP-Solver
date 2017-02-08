using System.Collections.Generic;

namespace TSPSolver.Model
{
   public class Route
   {
      private List<Address> _addresses;
      private double _distance;
      private double _duration;

      public List<Address> Addresses
      {
         get { return _addresses; }
         set { _addresses = value; }
      }

      public double Distance
      {
         get { return _distance; }
         set { _distance = value; }
      }

      public double Duration
      {
         get { return _duration; }
         set { _duration = value; }
      }
   }
}
