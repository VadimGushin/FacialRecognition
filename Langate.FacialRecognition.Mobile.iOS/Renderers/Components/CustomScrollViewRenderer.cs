using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(CustomScrollViewRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Components
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.Bounces = (e.NewElement as CustomScrollView).IsOverViewVisible;
            }
        }
    }
}