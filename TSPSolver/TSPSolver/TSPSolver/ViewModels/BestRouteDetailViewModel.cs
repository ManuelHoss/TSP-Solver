using System.Collections.Generic;
using System.Text;
using TSPSolver.Model;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class BestRouteDetailViewModel : BaseViewModel
   {
      private double _distance;
      private List<Address> _orderedAddresses = new List<Address>();

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

      public string OrderedAddressesString => CreateRouteString(OrderedAddresses);

      public double Distance
      {
         get { return _distance; }
         set { SetProperty(ref _distance, value); }
      }

      public string CreateRouteString(List<Address> addresses)
      {
         StringBuilder routeString = new StringBuilder();
         foreach (var address in addresses)
         {
            if (address.IsDepotAddress)
            {
               routeString.Append($"Depot:\t{address.FormattedAddress}\n");
            }
            else
            {
               routeString.Append($"\t--> {address.FormattedAddress}\n");
            }
         }

         return routeString.ToString();
      }
   }
}
