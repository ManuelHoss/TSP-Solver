using Project.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(FlatButtonRenderer))]
namespace Project.Droid
{
   public class FlatButtonRenderer : ButtonRenderer
   {
      protected override void OnDraw(Android.Graphics.Canvas canvas)
      {
         base.OnDraw(canvas);
         Elevation = 0;
      }
   }
}