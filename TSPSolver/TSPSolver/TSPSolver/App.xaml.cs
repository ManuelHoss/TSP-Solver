using System.Globalization;
using Plugin.FilePicker.Abstractions;
using TSPSolver.Views;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace TSPSolver
{
   public partial class App : Application
   {
      public App()
      {
         InitializeComponent();
         
         if (Device.OS == TargetPlatform.Android)
         {
            MainPage = new NavigationPage(new MainView_Mobile());
         }
         else
         {
            MainPage = new NavigationPage(new MainView());
         }
      }

      protected override void OnStart()
      {
         // Handle when your app starts
      }

      protected override void OnSleep()
      {
         // Handle when your app sleeps
      }

      protected override void OnResume()
      {
         // Handle when your app resumes
      }
   }
}
