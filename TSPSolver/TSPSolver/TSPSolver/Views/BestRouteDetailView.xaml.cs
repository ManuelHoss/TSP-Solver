using System.Text;
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
         InitializeComponent();
         BindingContext = _viewModel = new BestRouteDetailViewModel(this, bestRoute);
         CreateMap(bestRoute);
      } 

      private void CreateMap(Route bestRoute)
      {
         //StringBuilder stringBuilder = new StringBuilder();
         //stringBuilder.Append("https://maps.googleapis.com/maps/api/directions/json?origin=");
         //Boston,MA&destination=Concord,MA&waypoints=Charlestown,MA|Lexington,MA&key=YOUR_API_KEY

         MapsWebView.Source = "https://google.com";
      }
   }
}
