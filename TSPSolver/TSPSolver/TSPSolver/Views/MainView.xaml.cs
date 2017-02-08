using TSPSolver.ViewModels;
using Xamarin.Forms;

namespace TSPSolver.Views
{
   public partial class MainView : ContentPage
   {
      private readonly MainViewModel _viewModel;

      public MainView()
      {
         InitializeComponent();
         BindingContext = _viewModel = new MainViewModel(this);
         AddressListView.ItemsSource = _viewModel.AddressList;
      }
   }
}
