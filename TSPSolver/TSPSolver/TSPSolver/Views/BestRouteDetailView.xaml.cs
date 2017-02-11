using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TSPSolver.Model;
using TSPSolver.ViewModels;
using Xamarin.Forms;

namespace TSPSolver.Views
{
   public partial class BestRouteDetailView : ContentPage
   {
      private BestRouteDetailViewModel _viewModel;
      public BestRouteDetailView(Route bestRoute, AntColonyOptimizationLog acoLog)
      {
         InitializeComponent();
         BindingContext = _viewModel = new BestRouteDetailViewModel(this, bestRoute);
         CreateMap(bestRoute);
         
         var json = JsonConvert.SerializeObject(bestRoute);

         if (acoLog != null)
         {
            CreateAcoLogStackLayout(acoLog);
         }
      }
      
      private void CreateMap(Route bestRoute)
      {
         var assembly = typeof(BestRouteDetailView).GetTypeInfo().Assembly;
         Stream stream = assembly.GetManifestResourceStream("TSPSolver.local.html");
         StreamReader reader = new StreamReader(stream);
         string htmlString = reader.ReadToEnd();
         
         int pos = htmlString.IndexOf("<select id=\"start\">\r\n") + 25;
         htmlString = htmlString.Insert(pos, $"<option value=\"{bestRoute.Addresses[0].FormattedAddress}\">{bestRoute.Addresses[0].FormattedAddress}</option>\r\n");

         for (int i = bestRoute.Addresses.Count - 1; i > 1; i--)
         {
            pos = htmlString.IndexOf("<select multiple id=\"waypoints\">\r\n") + 38;
            htmlString = htmlString.Insert(pos, $"<option value=\"{bestRoute.Addresses[i].FormattedAddress}\">{bestRoute.Addresses[i].FormattedAddress}</option>\r\n");
         }

         pos = htmlString.IndexOf("<select id=\"end\">\r\n") + 23;
         htmlString = htmlString.Insert(pos, $"<option value=\"{bestRoute.Addresses[0].FormattedAddress}\">{bestRoute.Addresses[0].FormattedAddress}</option>\r\n");

         var html = new HtmlWebViewSource
         {
            Html = htmlString
         };

         MapsWebView.Source = html;
      }

      private void CreateAcoLogStackLayout(AntColonyOptimizationLog acoLog)
      {
         StackLayout acoLogLayout = new StackLayout();
         acoLogLayout.BackgroundColor = Constants.DarkOrange;
         acoLogLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
         acoLogLayout.VerticalOptions = LayoutOptions.FillAndExpand;
         acoLogLayout.Margin = new Thickness(0, 12);
         acoLogLayout.Padding = new Thickness(12);
         acoLogLayout.Children.Add(new Label()
         {
            Text = $"ANT-COLONY-OPTIMIZATION",
            Font = Font.SystemFontOfSize(NamedSize.Large),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.White
         });
         acoLogLayout.Children.Add(new Label()
         {
            Text = $"Time spent to find Solution: \t{acoLog.EvaluationDuration} milliseconds",
            TextColor = Color.White
         });
         acoLogLayout.Children.Add(new Label()
         {
            Text = $"Number of iterations: \t\t{acoLog.Iterations.Count}",
            TextColor = Color.White
         });
         acoLogLayout.Children.Add(new Label()
         {
            Text = $"Shortest route distance: \t{acoLog.BestRoute.Distance} meters",
            VerticalOptions = LayoutOptions.EndAndExpand,
            Font = Font.SystemFontOfSize(NamedSize.Small),
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.White
         });

         OptimizationLogStackLayout.Children.Add(acoLogLayout);
      }
   }
}
