using System.Collections.Generic;
using TSPSolver.Interfaces;
using TSPSolver.Model;
using TSPSolver.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Distance = TSPSolver.Model.Distance;

namespace TSPSolver.Views
{
   public partial class BestRouteDetailView : ContentPage
   {
      private BestRouteDetailViewModel _viewModel;
      public BestRouteDetailView(Route bestRoute)
      {
         InitializeComponent();
         BindingContext = _viewModel = new BestRouteDetailViewModel(this, bestRoute);
         CreateMap(bestRoute);
      }

      private void CreateMap(Route bestRoute)
      {
         IGeocoder geoCoderService = DependencyService.Get<IGeocoder>();

         double latSum = 0;
         double longSum = 0;

         foreach (var address in bestRoute.Addresses)
         {
            List<double> approximateLocation = geoCoderService.GetGeoCoordinatesOfAddress(address.ToString()).Result;

            BestRouteMap.Pins.Add(new Pin()
            {
               Type = PinType.Place,
               Position = new Position(approximateLocation[0], approximateLocation[1]),
               Label = "custom pin",
               Address = address.ToString()
            });

            latSum += approximateLocation[0];
            longSum += approximateLocation[1];
         }

         BestRouteMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latSum / bestRoute.Addresses.Count, longSum / bestRoute.Addresses.Count), Xamarin.Forms.Maps.Distance.FromMeters(2000)));
      }
   }
}
