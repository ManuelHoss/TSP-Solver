using TSPSolver.Interfaces;
using TSPSolver.Windows.PlatformSpecific;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace TSPSolver.Windows.PlatformSpecific
{
   public class BaseUrl : IBaseUrl
   {
      public string Get()
      {
         return "ms-appx-web:///";
      }
   }
}
