using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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

         _viewModel.PropertyChanged += ViewModelOnPropertyChanged; 
      }

      private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
      {
         if (propertyChangedEventArgs.PropertyName == "Addresses")
         {
            AddressListView.ItemsSource = _viewModel.Addresses.ToList();
         }
      }

      private void StartTimePicker_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == "Time")
         {
            _viewModel.UpdateArrivalTimesCommand.Execute(null);
         }
      }
   }
}
