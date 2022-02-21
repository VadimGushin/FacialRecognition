using Android.Content;
using Android.Views;
using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(CustomScrollViewRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Components
{
    public class CustomScrollViewRenderer : ScrollViewRenderer
    {
        public CustomScrollViewRenderer(Context context) : base(context)
        { 
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                if (!(e.NewElement as CustomScrollView).IsOverViewVisible)
                {
                    this.OverScrollMode = OverScrollMode.Never;
                }
            }
        }
    }
}