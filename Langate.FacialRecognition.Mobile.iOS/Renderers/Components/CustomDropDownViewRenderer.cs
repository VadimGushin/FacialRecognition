using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Components;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDropDownView), typeof(CustomDropDownViewRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Components
{
    public class CustomDropDownViewRenderer : ViewRenderer
    {
        public CustomDropDownViewRenderer() : base()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
            }

            if (e.NewElement != null)
            {
                var view = e.NewElement as Dropdown;
                var children = view.Children;
                //children.Remove(children[1]);
            }
        }
    }
}