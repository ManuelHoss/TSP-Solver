using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TSPSolver.Model;
using TSPSolver.Views;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class BestRouteOverviewViewModel : BaseViewModel
   {
      private double _distance;
      private List<Address> _orderedAddresses = new List<Address>();
      private List<Route> _bestRoutes;
      private Route _bestRoute;

      public BestRouteOverviewViewModel(Page page, List<Route> bestRoutes) : base(page)
      {
         _bestRoutes = bestRoutes;
         _bestRoute = bestRoutes.FirstOrDefault(route => route.Distance == bestRoutes.Max(r => r.Distance));
         _orderedAddresses = _bestRoute.Addresses;
         _distance = _bestRoute.Distance;
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

      public Route BestRoute
      {
         get { return _bestRoute; }
         set { _bestRoute = value; }
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
               routeString.Append($"\t> {address.FormattedAddress}\n");
            }
         }

         return routeString.ToString();
      }

      #region Commands

      #region OnBestViewTappedCommand

      private Command _onBestRouteTappedCommand { get; set; }

      public ICommand OnBestRouteTappedCommand
      {
         get
         {
            return _onBestRouteTappedCommand ??
                   (_onBestRouteTappedCommand =
                      new Command(() =>
                      {
                         Page.Navigation.PushAsync(new BestRouteDetailView(_bestRoute));
                      }));
         }
      }

      #endregion //OnBestViewTappedCommand

      #endregion //Commands
   }
}
