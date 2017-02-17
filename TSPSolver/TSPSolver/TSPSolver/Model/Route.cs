using System.Collections.Generic;

namespace TSPSolver.Model
{
   public class Route
   {
      private List<Address> _addresses = new List<Address>();
      private double _distance = 0;
      private double _duration = 0;
      private Dictionary<Address, Dictionary<Address, double>> _distanceMatrix;
      private Dictionary<Address, Dictionary<Address, double>> _durationMatrix;
      private OptimizationAlgorithmLog _optimizationAlgorithmLog;

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

      public Dictionary<Address, Dictionary<Address, double>> DistanceMatrix
      {
         get { return _distanceMatrix; }
         set { _distanceMatrix = value; }
      }

      public Dictionary<Address, Dictionary<Address, double>> DurationMatrix
      {
         get { return _durationMatrix; }
         set { _durationMatrix = value; }
      }

      public OptimizationAlgorithmLog OptimizationAlgorithmLog
      {
         get { return _optimizationAlgorithmLog; }
         set { _optimizationAlgorithmLog = value; }
      }
   }
}
