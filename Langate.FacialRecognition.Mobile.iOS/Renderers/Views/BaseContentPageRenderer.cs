using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Views;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using Foundation;
using MvvmCross.Forms.Platforms.Ios.Views;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using Plugin.DeviceInfo;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

[assembly: ExportRenderer(typeof(MvxContentPage), typeof(BaseContentPageRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Views
{
    public class BaseContentPageRenderer : MvxPageRenderer
    {
        //Use _viewModel.StatusBarStyle to change visibility

        #region Variables

        private readonly nint STATUS_BAR_TAG = 987654321;
        private UIView _statusBar = default(UIView);
        private BaseViewModel _viewModel;

        #endregion

        #region Overrides methods

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _viewModel = (e.NewElement as MvxContentPage).ViewModel as BaseViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            if (_viewModel.StatusBarColor != StatusBarColor.Default
                /*&& _viewModel.StatusBarStyle != StatusBarStyle.Default*/)
            {
                //UpdateSafeAreaProperties(_viewModel.StatusBarStyle);
                UpdateStatusBarColor(_viewModel.StatusBarColor);
            }
        }


        #region Handlers

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Common.GetPropertyName(() => _viewModel.StatusBarColor)
                || e.PropertyName == Common.GetPropertyName(() => _viewModel.StatusBarStyle))
            {
                UpdateStatusBarColor(_viewModel.StatusBarColor);
            }
        }

        #endregion

        #endregion

        #region Protected methods

        protected virtual void UpdateStatusBar()
        {
            //UpdateSafeAreaProperties(_viewModel.StatusBarStyle);
            UpdateStatusBarColor(_viewModel.StatusBarColor);
        }

        protected virtual void UpdateSafeAreaProperties(StatusBarStyle style)
        {
            if (style == StatusBarStyle.Visible)
            {
                ShowStatusBarGivenSafeArea(true, 20);
            }
            if (style == StatusBarStyle.Invisible)
            {
                UIApplication.SharedApplication.StatusBarHidden = true;
            }
            if (style == StatusBarStyle.Transparent)
            {
                ShowStatusBarGivenSafeArea(false, 0);
            }
        }

        protected virtual async void UpdateStatusBarColor(StatusBarColor color)
        {
            if (CrossDeviceInfo.Current.VersionNumber.Major >= 13)
            {
                await GetOrCreateStatusBar();
            }
            if (CrossDeviceInfo.Current.VersionNumber.Major < 13)
            {
                _statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            }

            if (_statusBar != null &&
                _statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")) &&
                (_viewModel.StatusBarStyle == StatusBarStyle.Default ||
                _viewModel.StatusBarStyle == StatusBarStyle.Visible))
            {
                SetStatusBarColor(color);
            }
        }

        #endregion

        #region Private methods       

        private async Task GetOrCreateStatusBar()
        {
            // HACK: In native code MakeKeyAndVisible method takes time to init KeyWindow, need to wait
            if (UIApplication.SharedApplication.KeyWindow == null)
            {
                await Task.Delay(200);
            }

            _statusBar = UIApplication.SharedApplication.KeyWindow?.ViewWithTag(STATUS_BAR_TAG);

            if (_statusBar == null)
            {
                _statusBar = AddStatusBar();
            }
            else
            {
                _statusBar.RemoveFromSuperview();
                _statusBar = AddStatusBar();
            }
        }

        private void ShowStatusBarGivenSafeArea(bool safeAreaValue, double topValue)
        {
            var page = Element as ContentPage;

            UIApplication.SharedApplication.StatusBarHidden = false;

            page.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(safeAreaValue);

            if (CrossDeviceInfo.Current.VersionNumber.Major <= 10)
            {
                page.Padding = new Thickness(0, topValue, 0, 0);
            }
        }

        private UIView AddStatusBar()
        {
            var statusBarView = new UIView(UIApplication.SharedApplication.StatusBarFrame);

            statusBarView.Tag = STATUS_BAR_TAG;

            UIApplication.SharedApplication.KeyWindow?.AddSubview(statusBarView);

            return statusBarView;
        }

        private void SetStatusBarColor(StatusBarColor color)
        {
            //Use _statusBar.BackgroundColor to change background
            if (color == StatusBarColor.White)
            {
                //_statusBar.BackgroundColor = UIColor.White;
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, true);
            }
            if (color == StatusBarColor.Black)
            {
                //_statusBar.BackgroundColor = UIColor.Black;
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
            }
            if (color == StatusBarColor.Transparent || color == StatusBarColor.Default)
            {
                //_statusBar.BackgroundColor = UIColor.Clear;
                UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
            }
        }

        #endregion
    }
}