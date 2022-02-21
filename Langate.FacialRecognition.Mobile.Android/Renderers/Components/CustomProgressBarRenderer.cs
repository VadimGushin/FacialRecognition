using Android.Content;
using Android.Support.V4.Content;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Components;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ProgressBar), typeof(CustomProgressBarRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Components
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        #region Variables

        private readonly Context _context;

        #endregion

        public CustomProgressBarRenderer(Context context) : base(context)
        {
            _context = context;
        }

        #region Override Methods

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var progressBar = Control as Android.Widget.ProgressBar;
                var draw = ContextCompat.GetDrawable(_context, Resource.Drawable.progress_bar_color);
                progressBar.ProgressDrawable = draw;
            }
        }

        #endregion
    }
}