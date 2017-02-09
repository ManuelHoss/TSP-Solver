using System;
using System.Linq;
using TSPSolver.Model;
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

      private void DeleteButton_OnClicked(object sender, EventArgs e)
      {
         var button = (Button)sender;
         Address listitem = _viewModel.AddressList.FirstOrDefault(item => item.Id.ToString() == button.CommandParameter.ToString());
         if (listitem != null)
         {
            _viewModel.AddressList.Remove(listitem);
         }
      }

      private void EditButton_OnClicked(object sender, EventArgs e)
      {
         var button = (Button)sender;
         Address listitem = _viewModel.AddressList.FirstOrDefault(item => item.Id.ToString() == button.CommandParameter.ToString());
         if (listitem != null)
         {
            _viewModel.AddressList.Remove(listitem);
         }
      }
   }


}
