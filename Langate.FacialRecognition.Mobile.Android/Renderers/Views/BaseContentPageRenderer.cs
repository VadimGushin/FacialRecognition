using Android.Content;
using Android.Views;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Views;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Plugin.CurrentActivity;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MvxContentPage), typeof(BaseContentPageRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Views
{
    public class BaseContentPageRenderer : MvxPageRenderer
    {
        #region Variables

        private BaseViewModel _viewModel;

        #endregion

        public BaseContentPageRenderer(
            Context context)
            : base(context)
        {
        }

        #region Override methods

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                UpdateStatusBar();
            }

            _viewModel = (e.NewElement as MvxContentPage).ViewModel as BaseViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            if (_viewModel.StatusBarColor != StatusBarColor.Default
                /*&& _viewModel.StatusBarStyle != StatusBarStyle.Default*/)
            {
                //UpdateSafeAreaProperties(_viewModel.StatusBarStyle);
                UpdateStatusBar();
            }
        }

        #endregion

        #region Handlers

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Common.GetPropertyName(() => _viewModel.StatusBarColor)
                || e.PropertyName == Common.GetPropertyName(() => _viewModel.StatusBarStyle))
            {
                UpdateStatusBar();
            }
        }

        #endregion

        #region Protected methods

        protected virtual void UpdateStatusBar()
        {
            if (_viewModel != null)
            {
                //UpdateSafeAreaProperties(page.StatusBarStyle);
                UpdateStatusBarColor();
            }
        }

        protected virtual void UpdateSafeAreaProperties(StatusBarStyle style)
        {
            if (style == StatusBarStyle.Visible)
            {
                CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Visible;
            }
            if (style == StatusBarStyle.Invisible)
            {
                CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.Fullscreen;
            }
            if (style == StatusBarStyle.Transparent)
            {
                CrossCurrentActivity.Current.Activity.Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
            }
        }

        protected virtual void UpdateStatusBarColor()
        {
            //if (_viewModel.StatusBarStyle == StatusBarStyle.Default
            //    /*|| _viewModel.StatusBarStyle == StatusBarStyle.Visible*/)
            //{
            //    CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(color.ToAndroid());
            //}
            //CrossCurrentActivity.Current.Activity.Window.AddFlags(WindowManagerFlags.LayoutNoLimits);

            if (CrossCurrentActivity.Current.Activity == null)
            {
                return;
            }
            //CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
            //CrossCurrentActivity.Current.Activity.Window.ClearFlags( WindowManagerFlags.TranslucentStatus);
            //CrossCurrentActivity.Current.Activity.Window.AddFlags( WindowManagerFlags.DrawsSystemBarBackgrounds);
            var flags = CrossCurrentActivity.Current.Activity.Window.DecorView.WindowSystemUiVisibility;

            if (_viewModel.StatusBarColor == StatusBarColor.Black)
            {
                //CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(Android.Graphics.Color.Black);
                //CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            }
            if (_viewModel.StatusBarColor == StatusBarColor.White)
            {
                //CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LayoutStable;
                //CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(Android.Graphics.Color.Black);

                //CrossCurrentActivity.Current.Activity.Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                //CrossCurrentActivity.Current.Activity.Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);


                //CrossCurrentActivity.Current.Activity.Window.DecorView.SystemUiVisibility = StatusBarVisibility.;

            }
            if (_viewModel.StatusBarColor == StatusBarColor.Transparent ||
                _viewModel.StatusBarColor == StatusBarColor.Default)
            {
                //CrossCurrentActivity.Current.Activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            }
        }

        #endregion
    }
}