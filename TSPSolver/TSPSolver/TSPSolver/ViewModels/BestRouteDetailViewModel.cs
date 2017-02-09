using System.Collections.Generic;
using TSPSolver.Model;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class BestRouteDetailViewModel : BaseViewModel
   {
      private double _distance;
      private List<Address> _orderedAddresses;

      public BestRouteDetailViewModel(Page page, Route bestRoute) : base(page)
      {
         _orderedAddresses = bestRoute.Addresses;
         _distance = bestRoute.Distance;
      }

      public List<Address> OrderedAddresses
      {
         get { return _orderedAddresses; }
         set { SetProperty(ref _orderedAddresses, value); }
      }

      public double Distance
      {
         get { return _distance; }
         set { SetProperty(ref _distance, value); }
      }
   }
}
