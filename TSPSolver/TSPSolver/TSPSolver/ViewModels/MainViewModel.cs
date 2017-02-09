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

      private TspService tspService;

      #region Constructor

      public MainViewModel(Page page) : base(page)
      {
         // Initialize Mock List
         _addressList.Add(new Address() {Street = "Kapuzinergasse", Number = "20", Zip = "86150", City = "Augsburg"});
         _addressList.Add(new Address() {Street = "Universitätsstraße", Number = "6a", Zip = "86159", City = "Augsburg"});
         _addressList.Add(new Address() {Street = "Bürgermeister-Ulrich-Straße", Number = "90", Zip = "86199", City = "Augsburg"});
         _addressList.Add(new Address() {Street = "Aubinger Str.", Number = "162", Zip = "81243", City = "München" });
         _addressList.Add(new Address() {Street = "Sommestraße", Number = "40", Zip = "86156", City = "Augsburg" });
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
                            _addressList.Add(new Address()
                            {
                               Id = Guid.NewGuid(),
                               Street = _street,
                               Number = _number,
                               Zip = _zip,
                               City = _city
                            });

                            // Cleanup for next entry
                            Street = "";
                            Number = "";
                            Zip = "";
                            City = "";
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
                         if (AddressList.Count > 1)
                         {
                            tspService = new TspService();
                            Route bestRoute = tspService.CalculateBestRoute(AddressList.ToList());
                            Page.Navigation.PushAsync(new BestRouteDetailView(bestRoute));
                         }
                      }));
         }
      }

      #endregion //CalculateBestRouteCommand
      
      #endregion //Commands
   }
}
