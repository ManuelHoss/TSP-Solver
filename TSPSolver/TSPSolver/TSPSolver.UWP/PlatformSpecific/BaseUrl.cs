using TSPSolver.Interfaces;
using TSPSolver.UWP.PlatformSpecific;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace TSPSolver.UWP.PlatformSpecific
{
   public class BaseUrl : IBaseUrl
   {
      public string Get()
      {
         return "ms-appx-web:///";
      }
   }
}
