using System.Collections.ObjectModel;
using TSPSolver.Model;
using TSPSolver.ViewModels;
using Xamarin.Forms;

namespace TSPSolver.Views
{
   public partial class MainView : ContentPage
   {

      private MainViewModel _viewModel;

      public MainView()
      {
         InitializeComponent();
         BindingContext = _viewModel = new MainViewModel(this);
         btnAddButton.WidthRequest = btnAddButton.HeightRequest;
         AddressListView.ItemsSource = _viewModel.AddressList;
      }
   }
}
