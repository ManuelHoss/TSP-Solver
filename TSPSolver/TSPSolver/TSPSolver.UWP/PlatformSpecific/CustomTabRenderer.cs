using Windows.UI.Xaml.Markup;
using TSPSolver.Model;
using TSPSolver.UWP.PlatformSpecific;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(StyledTabbedPage), typeof(CustomTabRenderer))]

namespace TSPSolver.UWP.PlatformSpecific
{
   public class CustomTabRenderer : TabbedPageRenderer
   {
      public CustomTabRenderer()
      {
         this.ElementChanged += CustomTabRenderer_ElementChanged;

      }

      private void CustomTabRenderer_ElementChanged(object sender, VisualElementChangedEventArgs e)
      {
         Control.HeaderTemplate = GetStyledTitleTemplate();
      }

      private Windows.UI.Xaml.DataTemplate GetStyledTitleTemplate()
      {
         string dataTemplateXaml = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                <TextBlock
                    Text=""{Binding Title}""
                    FontFamily=""/Assets/Fonts/museosans-500.ttf#museosans-500""
                    FontSize =""25"" />
                  </DataTemplate>";

         return (Windows.UI.Xaml.DataTemplate)XamlReader.Load(dataTemplateXaml);
      }
   }
}