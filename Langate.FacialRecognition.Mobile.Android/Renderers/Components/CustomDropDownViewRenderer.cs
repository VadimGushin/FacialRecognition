using Android.App;
using Android.Content;
using Android.Views.InputMethods;
using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Components;
using Plugin.CurrentActivity;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDropDownView), typeof(CustomDropDownViewRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Components
{
    public class CustomDropDownViewRenderer : ViewRenderer
    {
        #region Variables

        private Context _context;
        private readonly Activity _activity = CrossCurrentActivity.Current.Activity;

        #endregion

        public CustomDropDownViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        #region Override Methods

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
            }

            if (e.NewElement != null)
            {
                var view = e.NewElement as CustomDropDownView;
                var children = view.Children;

                view.Margin = new Thickness(0, 0, 0, 0);
                view.Padding = new Thickness(0, 0, 0, 0);

                view.OnMenuShowed += (sender, arg) =>
                {
                    TryCloseKeyboard();
                };
            }
        }

        #endregion

        #region Private Methods

        private void TryCloseKeyboard()
        {
            InputMethodManager inputManager = (InputMethodManager)_activity.GetSystemService(Context.InputMethodService);
            if (_activity != null && _activity.CurrentFocus != null)
            {
                inputManager.HideSoftInputFromWindow(_activity.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }
        }

        #endregion
    }
}