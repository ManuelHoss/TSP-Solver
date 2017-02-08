using System.Text;
using TSPSolver.Model;
using TSPSolver.ViewModels;
using Xamarin.Forms;

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
         StringBuilder stringBuilder = new StringBuilder();
         stringBuilder.Append("https://maps.googleapis.com/maps/api/directions/json?origin=");
         stringBuilder.Append($"{bestRoute.Addresses[0]}");
         stringBuilder.Append("&destination=");
         stringBuilder.Append($"{bestRoute.Addresses[bestRoute.Addresses.Count-1]}");
         stringBuilder.Append("&waypoints=");
         for (int i = 1; i < bestRoute.Addresses.Count-1; i++)
         {
            stringBuilder.Append($"{bestRoute.Addresses[i]}|");
         }
         //Remove last "|"
         stringBuilder.Remove(stringBuilder.Length - 1, 1);
         stringBuilder.Append($"&key={Constants.GoogleMapsApiKey}");

         //MapsWebView.Source = "http://maps.google.com/maps?" + "saddr=43.0054446,-87.9678884" + "&daddr=42.9257104,-88.0508355";
         MapsWebView.Source = stringBuilder.ToString();
      }
   }
}
