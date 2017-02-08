using Xamarin;

namespace TSPSolver.UWP
{
   public sealed partial class MainPage
   {
      public MainPage()
      {
         this.InitializeComponent();
         LoadApplication(new TSPSolver.App());
      }
   }
}
