﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using TSPSolver.CSV_Import;
using TSPSolver.Model;
using TSPSolver.ViewModels;
using Xamarin.Forms;

namespace TSPSolver.Views
{
   public partial class MainView : ContentPage
   {
      private readonly MainViewModel _viewModel;
      private Button _depotButton;
      public MainView()
      {
         InitializeComponent();
         BindingContext = _viewModel = new MainViewModel(this);
         AddressListView.ItemsSource = _viewModel.AddressList;
      }

      private void DeleteButton_OnClicked(object sender, EventArgs e)
      {
         var button = (Button)sender;
         Address listitem = _viewModel.AddressList.FirstOrDefault(item => item.Id.ToString() == button.CommandParameter.ToString());
         if (listitem != null)
         {
            if (listitem.IsDepotAddress)
            {
               DisplayAlert("Depot address can't be deleted!","" , "OK");
            }
            else
            {
               _viewModel.AddressList.Remove(listitem);
            }
         }
      }

      private void EditButton_OnClicked(object sender, EventArgs e)
      {
         var button = (Button)sender;
         Address listitem = _viewModel.AddressList.FirstOrDefault(item => item.Id.ToString() == button.CommandParameter.ToString());
         if (listitem != null)
         {

         }
      }

      private void SetDepotButton_OnClicked(object sender, EventArgs e)
      {
         if (_depotButton != null)
         {
            // Unselect old depot address
            _depotButton.Image = "Depot_Inactive_85.png";
            _viewModel.AddressList.FirstOrDefault(item => item.Id.ToString() == _depotButton.CommandParameter.ToString()).IsDepotAddress = false;
         }
         
         // Select new depot address
         _depotButton = (Button)sender;
         _depotButton.Image = "Depot_Active_85.png";
         _viewModel.AddressList.FirstOrDefault(item => item.Id.ToString() == _depotButton.CommandParameter.ToString()).IsDepotAddress = true;
      }

      private async void ReadCsvButton_OnClicked(object sender, EventArgs e)
      {
         _viewModel.AddressList =  new ObservableCollection<Address>(await CsvHelper.ReadCsv());
         AddressListView.ItemsSource = _viewModel.AddressList;
      }
   }


}
