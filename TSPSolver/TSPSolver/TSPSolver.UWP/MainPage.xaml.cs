using Xamarin;

namespace TSPSolver.UWP
{
   public sealed partial class MainPage
   {
      public MainPage()
      {
         this.InitializeComponent();
         FormsMaps.Init("yCuJJ87Z75hk2C2Iy8fR~3Godq4K-ZNkQ5rXOQg_g0w~AubL0KgJZgd05w7Q20Tbmw1ApBfkdZrZahUqdM1LybwaGl3qTQ8zMhqwAzRm9YQh");
         LoadApplication(new TSPSolver.App());
      }
   }
}
