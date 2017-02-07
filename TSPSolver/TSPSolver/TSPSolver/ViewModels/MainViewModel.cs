using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TSPSolver.Model;
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

      public MainViewModel(Page page) : base(page)
      {
      }

      #region Properties

      #endregion //Properties

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

      #region Commands

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

      #endregion //Commands
   }
}
