using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using TSPSolver.Model;
using TSPSolver.ViewModels;
using Xamarin.Forms;

namespace TSPSolver.Views
{
   public partial class BestRouteOverviewView : ContentPage
   {
      private BestRouteOverviewViewModel _viewModel;
      public BestRouteOverviewView(List<Route> bestRoutes)
      {
         InitializeComponent();
         BindingContext = _viewModel = new BestRouteOverviewViewModel(this, bestRoutes);
         
         CreateMap(_viewModel.BestRoute);

         if (bestRoutes != null)
         {
            foreach (var bestRoute in bestRoutes)
            {
               if (bestRoute.OptimizationAlgorithmLog != null)
               {
                  CreateAcoLogStackLayout(bestRoute.OptimizationAlgorithmLog);
               }
            }
         }
      }
      
      private void CreateMap(Route bestRoute)
      {
         var assembly = typeof(BestRouteOverviewView).GetTypeInfo().Assembly;
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

      private void CreateAcoLogStackLayout(OptimizationAlgorithmLog algorithmLog)
      {
         StackLayout algorithmLogLayout = new StackLayout();
         algorithmLogLayout.BackgroundColor = Constants.DarkOrange;
         algorithmLogLayout.HorizontalOptions = LayoutOptions.StartAndExpand;
         algorithmLogLayout.VerticalOptions = LayoutOptions.FillAndExpand;
         algorithmLogLayout.Margin = new Thickness(0, 12);
         algorithmLogLayout.Padding = new Thickness(12);
         algorithmLogLayout.Children.Add(new Label()
         {
            Text = $"{algorithmLog.AlgorithmName}",
            Font = Font.SystemFontOfSize(NamedSize.Large),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.White
         });
         algorithmLogLayout.Children.Add(new Label()
         {
            Text = $"Time spent to find Solution: \t{algorithmLog.EvaluationDuration} milliseconds",
            TextColor = Color.White
         });
         algorithmLogLayout.Children.Add(new Label()
         {
            Text = $"Number of iterations: \t\t{algorithmLog.Iterations.Count}",
            TextColor = Color.White
         });
         algorithmLogLayout.Children.Add(new Label()
         {
            Text = $"Shortest route distance: \t{algorithmLog.BestRoute.Distance} meters",
            VerticalOptions = LayoutOptions.EndAndExpand,
            Font = Font.SystemFontOfSize(NamedSize.Small),
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.White
         });

         OptimizationLogStackLayout.Children.Add(algorithmLogLayout);
      }
   }
}
