using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPSolver.Model;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class BestRouteDetailViewModel : BaseViewModel
   {
      private List<Address> _addresses;
      private Dictionary<Address, Dictionary<Address, double>> _distanceMatrix;
      private Dictionary<Address, Dictionary<Address, double>> _durationMatrix;
      private TimeSpan _startTimeOfRoute;

      public BestRouteDetailViewModel(Page page, Route bestRoute) : base(page)
      {
         Addresses = bestRoute.Addresses.ToList();
         StartTimeOfRoute = DateTime.Now.TimeOfDay;
         DistanceMatrix = bestRoute.DistanceMatrix;
         DurationMatrix = bestRoute.DurationMatrix;
      }

      public List<Address> Addresses
      {
         get { return _addresses; }
         set { SetProperty(ref _addresses, value); }
      }

      public Dictionary<Address, Dictionary<Address, double>> DistanceMatrix
      {
         get { return _distanceMatrix; }
         set { SetProperty(ref _distanceMatrix, value); }
      }

      public Dictionary<Address, Dictionary<Address, double>> DurationMatrix
      {
         get { return _durationMatrix; }
         set { SetProperty(ref _durationMatrix, value); }
      }

      public TimeSpan StartTimeOfRoute
      {
         get { return _startTimeOfRoute; }
         set { SetProperty(ref _startTimeOfRoute, value); }
      }
   }
}
