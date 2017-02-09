﻿using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using TSPSolver.Interfaces;
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
         
         
         var json = JsonConvert.SerializeObject(bestRoute);
         MapsWebView.Eval($"initMap({json})");
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
         
         string baseUrl = DependencyService.Get<IBaseUrl>().Get();

         //var assembly = typeof(MapHtmlString).GetTypeInfo().Assembly;
         //Stream stream = assembly.GetManifestResourceStream("local.html");
         //StreamReader reader = new StreamReader(stream);
         //string htmlString = reader.ReadToEnd();

         var html = new UrlWebViewSource
         {
            Url = System.IO.Path.Combine(baseUrl, "local.html")
         };

         MapsWebView.Source = html;
      }
   }
}
