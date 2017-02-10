using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
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

      private TspService _tspService;

      #region Constructor

      public MainViewModel(Page page) : base(page)
      {
         // Initialize Mock List
         _addressList.Add(new Address() {Street = "Kapuzinergasse", Number = "20", Zip = "86150", City = "Augsburg", Id = Guid.NewGuid(), FormattedAddress = "Kapuzinergasse 20, 86150 Augsburg, Deutschland"});
         _addressList.Add(new Address() {Street = "Universitätsstraße", Number = "6a", Zip = "86159", City = "Augsburg", Id = Guid.NewGuid(), FormattedAddress = "Universitätsstraße 6, 86159 Augsburg, Deutschland"});
         _addressList.Add(new Address() {Street = "Bürgermeister-Ulrich-Straße", Number = "90", Zip = "86199", City = "Augsburg", Id = Guid.NewGuid(), FormattedAddress = "Bürgermeister-Ulrich-Straße 90, 86199 Augsburg, Deutschland"});
         _addressList.Add(new Address() {Street = "Aubinger Str.", Number = "162", Zip = "81243", City = "München", Id = Guid.NewGuid(), FormattedAddress = "Aubinger Str. 162, 81243 München, Deutschland"});
         _addressList.Add(new Address() {Street = "Sommestraße", Number = "40", Zip = "86156", City = "Augsburg", Id = Guid.NewGuid(), FormattedAddress = "Sommestraße 40, 86156 Augsburg, Deutschland"});
      }

      #endregion //Constructor

      #region Properties

      public ObservableCollection<Address> AddressList
      {
         get { return _addressList; }
         set { SetProperty(ref _addressList, value); }
      }

      public string Street
      {
         get { return _street; }
         set { SetProperty(ref _street, value); }
      }

      public string Number
      {
         get { return _number; }
         set { SetProperty(ref _number, value); }
      }

      public string Zip
      {
         get { return _zip; }
         set { SetProperty(ref _zip, value); }
      }

      public string City
      {
         get { return _city; }
         set { SetProperty(ref _city, value); }
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
                      new Command(() =>
                      {
                         //Case of editing a existent Entry
                         if (!String.IsNullOrEmpty(_street)
                             & !String.IsNullOrEmpty(_number)
                             & !String.IsNullOrEmpty(_zip)
                             & !String.IsNullOrEmpty(_city))
                         {
                            Address addressToAdd = new Address() {Street = _street, Number = _number, Zip = _zip, City = _city};
                            GoogleMapsApiAddressResult validationResult = GoogleProvider.ValidateAddress(addressToAdd.ToString()).Result;

                            if (validationResult.status == "OK")
                            {
                               addressToAdd.Street = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("route")).long_name;
                               addressToAdd.Number = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("street_number")).long_name;
                               addressToAdd.City = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("locality")).long_name;
                               addressToAdd.Zip = validationResult.results[0].address_components.FirstOrDefault(item => item.types.Contains("postal_code")).long_name;
                               addressToAdd.FormattedAddress = validationResult.results[0].formatted_address;
                               _addressList.Add(addressToAdd);
                               // Cleanup for next entry
                               Street = "";
                               Number = "";
                               Zip = "";
                               City = "";
                            }
                            else
                            {
                               Page.DisplayAlert("Address not valid!", "Address not found in Google Maps. Please verfy correctness and try again.", "OK");
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
                      new Command(() =>
                      {
                         //Case of editing a existent Entry
                         if (AddressList.Count(item => item.IsDepotAddress) < 1)
                         {
                            Page.DisplayAlert("No depot address chosen!", "Choose a address as your depot address by clicking the transporter icon on one of the address entries!", "OK");
                         }
                         else if (AddressList.Count > 1)
                         {
                            _tspService = new TspService();
                            Route bestRoute = _tspService.CalculateBestRoute(AddressList.ToList(), AddressList.FirstOrDefault(address => address.IsDepotAddress));
                            Page.Navigation.PushAsync(new BestRouteTabbedView(bestRoute));
                         }
                      }));
         }
      }

      #endregion //CalculateBestRouteCommand

      #endregion //Commands
   }
}
