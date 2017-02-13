using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
         BindingContext = _viewModel = new BestRouteDetailViewModel(this, bestRoute);
         InitializeComponent();
         _viewModel.StartTimeOfRoute = DateTime.Now.TimeOfDay;
         CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");
         StartTimePicker.Format = CultureInfo.DefaultThreadCurrentUICulture.DateTimeFormat.ShortTimePattern;
         
      }

      private void StartTimePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (StartTimePicker != null)
         {
            _viewModel.StartTimeOfRoute = StartTimePicker.Time;
            TimeSpan tmpTimeSpan = _viewModel.StartTimeOfRoute;
            _viewModel.Addresses[0].ArrivalTime = tmpTimeSpan;
            for (int i = 0; i < _viewModel.Addresses.Count - 1; i++)
            {
               tmpTimeSpan = tmpTimeSpan.Add(new TimeSpan(0, 0, (int)_viewModel.DurationMatrix[_viewModel.Addresses[i]][_viewModel.Addresses[i + 1]]));
               _viewModel.Addresses[i+1].ArrivalTime = tmpTimeSpan;
            }
            AddressListView.ItemsSource = _viewModel.Addresses;
         }
      }
   }
}
