﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TSPSolver.CSV_Import;
using TSPSolver.Model;
using TSPSolver.Services;
using TSPSolver.Views;
using Xamarin.Forms;

namespace TSPSolver.ViewModels
{
   public class MainViewModel : BaseViewModel
   {
      private ObservableCollection<Address> _addressList = new ObservableCollection<Address>();
      private string _street;
      private string _number;
      private string _zip;
      private string _city;
      private string _inputAddress;

      private TspService _tspService;
      private bool _useAcoAlgorithm;
      private bool _useDynamicProgrammingAlgorithm;
      private bool _useGeneticProgrammingAlgorithm;

      #region Constructor

      public MainViewModel(Page page) : base(page)
      {
         _useAcoAlgorithm = true;
         _useDynamicProgrammingAlgorithm = true;
         _useGeneticProgrammingAlgorithm = true;

         // Initialize Mock List
         _addressList.Add(new Address() {Id = Guid.NewGuid(), FormattedAddress = "Kapuzinergasse 20, 86150 Augsburg, Deutschland"});
         _addressList.Add(new Address() {Id = Guid.NewGuid(), FormattedAddress = "Universitätsstraße 6, 86159 Augsburg, Deutschland"});
         _addressList.Add(new Address() {Id = Guid.NewGuid(), FormattedAddress = "Bürgermeister-Ulrich-Straße 90, 86199 Augsburg, Deutschland"});
         _addressList.Add(new Address() {Id = Guid.NewGuid(), FormattedAddress = "Aubinger Str. 162, 81243 München, Deutschland"});
         _addressList.Add(new Address() {Id = Guid.NewGuid(), FormattedAddress = "Sommestraße 40, 86156 Augsburg, Deutschland"});
      }

      #endregion //Constructor

      #region Properties

      public string InputAddress
      {
         get { return _inputAddress; }
         set { SetProperty(ref _inputAddress, value); }
      }

      public ObservableCollection<Address> AddressList
      {
         get { return _addressList; }
         set { SetProperty(ref _addressList, value); }
      }

      public bool UseAcoAlgorithm
      {
         get { return _useAcoAlgorithm; }
         set { SetProperty(ref _useAcoAlgorithm, value); }
      }

      public bool UseDynamicProgrammingAlgorithm
      {
         get { return _useDynamicProgrammingAlgorithm; }
         set { SetProperty(ref _useDynamicProgrammingAlgorithm, value); }
      }

      public bool UseGeneticProgrammingAlgorithm
      {
         get { return _useGeneticProgrammingAlgorithm; }
         set { SetProperty(ref _useGeneticProgrammingAlgorithm, value); }
      }

      #endregion //Properties

      #region Commands

      #region AddAddressToListCommand

      // Add address to list
      private Command _addAddressToListCommand { get; set; }

      public ICommand AddAddressToListCommand
      {
         get
         {
            return _addAddressToListCommand ??
                   (_addAddressToListCommand =
                      new Command(async () =>
                      {
                         //Case of editing a existent Entry
                         if (!String.IsNullOrEmpty(_inputAddress))
                         {
                            IsBusy = true;
                            try
                            {
                               GoogleMapsApiAddressResult validationResult = await GoogleProvider.ValidateAddress(InputAddress);

                               if (validationResult.status == "OK")
                               {
                                  Address address = new Address();
                                  address.FormattedAddress = validationResult.results[0].formatted_address;
                                  var response = await Page.DisplayAlert("VALIDATION", $"Validate the address to add:\n{address.FormattedAddress}." , "ADD", "CANCEL");
                                  if (response)
                                  {
                                     AddressList.Add(address);
                                     InputAddress = "";
                                  }
                               }
                               else if(validationResult.status == "ZERO_RESULTS")
                               {
                                  IsBusy = false;
                                  await Page.DisplayAlert("Address not valid!", "Address not found in Google Maps. Please verfy correctness and try again.", "OK");
                               }
                            }
                            catch (Exception ex)
                            {
                               IsBusy = false;
                               await Page.DisplayAlert("ERROR", $"Exception: {ex}", "OK");
                            }
                            finally
                            {
                               IsBusy = false;
                            }
                         }
                      }));
         }
      }

      #endregion //AddAddressToListCommand

      #region CalculateBestRouteCommand

      private Command _calculateBestRouteCommand { get; set; }

      public ICommand CalculateBestRouteCommand
      {
         get
         {
            return _calculateBestRouteCommand ??
                   (_calculateBestRouteCommand =
                      new Command(async () =>
                      {
                         IsBusy = true;
                         //Case of editing a existent Entry
                         if (AddressList.Count(item => item.IsDepotAddress) < 1)
                         {
                            IsBusy = false;
                            await Page.DisplayAlert("No depot address chosen!", "Choose a address as your depot address by clicking the transporter icon on one of the address entries!", "OK");
                         }
                         else if (AddressList.Count > 1)
                         {
                            try
                            {
                               _tspService = new TspService();
                               List<AlgorithmType> algorithmTypes = new List<AlgorithmType>();
                               if(UseAcoAlgorithm) { algorithmTypes.Add(AlgorithmType.AntColonyOptimization);}
                               if(UseDynamicProgrammingAlgorithm) { algorithmTypes.Add(AlgorithmType.DynamicProgramming);}
                               if(UseGeneticProgrammingAlgorithm) { algorithmTypes.Add(AlgorithmType.GeneticAlgorithm);}
                               List<Route> bestRoutes = await _tspService.CalculateBestRoute(AddressList.ToList(), AddressList.FirstOrDefault(address => address.IsDepotAddress), algorithmTypes);
                               IsBusy = false;
                               await Page.Navigation.PushAsync(new BestRouteOverviewView(bestRoutes));
                            }
                            catch (Exception ex)
                            {
                               await Page.DisplayAlert("ERROR", "Problems calculating best route", "Ok");
                            }
                            finally
                            {
                               IsBusy = false;
                            }
                            
                         }
                      }));
         }
      }

      #endregion //CalculateBestRouteCommand

      #region ImportCsvCommand

      private Command _importCsvCommand { get; set; }

      public ICommand ImportCsvCommand
      {
         get
         {
            return _importCsvCommand ??
                   (_importCsvCommand =
                      new Command(async () =>
                      {
                         List<Address> inputListView = new List<Address>(await CsvHelper.ReadCsv());

                         if (inputListView.Any())
                         {
                            AddressList = new ObservableCollection<Address>(inputListView);
                         }
                         else
                         {
                            await Page.DisplayAlert("Import Error", "A problem occured while importing the selected CSV-file. Make sure the file is valid and try agail.", "OK");
                         }
                      }));
         }
      }

      #endregion //ImportCsvCommand

      #endregion //Commands
   }
}
