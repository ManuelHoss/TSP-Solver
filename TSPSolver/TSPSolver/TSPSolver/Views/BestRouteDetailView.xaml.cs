using System;
using System.IO;
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
         var assembly = typeof(BestRouteDetailView).GetTypeInfo().Assembly;
         Stream stream = assembly.GetManifestResourceStream("TSPSolver.local.html");
         StreamReader reader = new StreamReader(stream);
         string htmlString = reader.ReadToEnd();
         
         int pos = htmlString.IndexOf("<select id=\"start\">\r\n") + 25;
         htmlString = htmlString.Insert(pos, $"<option value=\"{bestRoute.Addresses[0]}\">{bestRoute.Addresses[0]}</option>\r\n");


         pos = htmlString.IndexOf("<select id=\"end\">\r\n") + 23;
         htmlString = htmlString.Insert(pos, $"<option value=\"{bestRoute.Addresses[0]}\">{bestRoute.Addresses[0]}</option>\r\n");

         var html = new HtmlWebViewSource
         {
            Html = htmlString
         };

         MapsWebView.Source = html;
      }
   }
}
