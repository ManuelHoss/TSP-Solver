using TSPSolver.Model;
using Xamarin.Forms;

namespace TSPSolver.Views
{
   public partial class BestRouteTabbedView
   {
      public BestRouteTabbedView(Route bestRoute)
      {
         this.Children.Add(new BestRouteDetailView(bestRoute));
         this.Children.Add(new NavigationPage(new BestRouteDetailView(bestRoute)) {Title = "Best Route Map."});
      }
   }
}
