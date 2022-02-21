using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Xamarin.Essentials;

namespace Langate.FacialRecognition.Mobile.Droid.Helpers
{
    public class KeyboardHelper
    {
        #region Variables

        private readonly View mChildOfContent;
        private int usableHeightPrevious;
        private FrameLayout.LayoutParams frameLayoutParams;

        #endregion

        #region Properties

        private int _bottomSpace { get; set; }
        private int _statusBarHeight { get; set; }

        #endregion

        #region Constructors

        private KeyboardHelper(Activity activity, IWindowManager windowManager)
        {
            var softButtonsHeight = GetSoftButtonsBarHeight(windowManager);
            GetStatusBarHeight(activity);
            CalculateBottomSpace(softButtonsHeight);

            var content = (FrameLayout)activity.FindViewById(Android.Resource.Id.Content);
            mChildOfContent = content.GetChildAt(0);
            var vto = mChildOfContent.ViewTreeObserver;
            vto.GlobalLayout += (sender, e) => PossiblyResizeChildOfContent(softButtonsHeight);
            frameLayoutParams = (FrameLayout.LayoutParams)mChildOfContent.LayoutParameters;
        }

        #endregion

        #region Static Methods

        public static void AssistActivity(Activity activity, IWindowManager windowManager)
        {
            new KeyboardHelper(activity, windowManager);
        }

        #endregion

        #region Private Methods

        private void PossiblyResizeChildOfContent(int softButtonsHeight)
        {
            var usableHeightNow = ComputeUsableHeight();
            if (usableHeightNow == usableHeightPrevious)
            {
                return;
            }
            var usableHeightSansKeyboard = mChildOfContent.RootView.Height - softButtonsHeight;
            var heightDifference = usableHeightSansKeyboard - usableHeightNow;
            if (heightDifference > (usableHeightSansKeyboard / 4))
            {
                frameLayoutParams.Height = usableHeightSansKeyboard - heightDifference + (softButtonsHeight / 2) + _bottomSpace;
            }
            if (heightDifference <= (usableHeightSansKeyboard / 4))
            {
                frameLayoutParams.Height = usableHeightSansKeyboard;
            }
            mChildOfContent.RequestLayout();
            usableHeightPrevious = usableHeightNow;
        }

        private int ComputeUsableHeight()
        {
            var rect = new Rect();
            mChildOfContent.GetWindowVisibleDisplayFrame(rect);
            return (rect.Bottom - rect.Top);
        }

        private int GetSoftButtonsBarHeight(IWindowManager windowManager)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                return 0;
            }

            var metrics = new DisplayMetrics();
            windowManager.DefaultDisplay.GetMetrics(metrics);
            int usableHeight = metrics.HeightPixels;

            _bottomSpace = (int)(Math.Round(95 * metrics.ScaledDensity));

            windowManager.DefaultDisplay.GetRealMetrics(metrics);
            int realHeight = metrics.HeightPixels;

            var result = realHeight > usableHeight ? realHeight - usableHeight : 0;
            return result;
        }

        private void GetStatusBarHeight(Activity activity)
        {
            _statusBarHeight = 0;
            int resourceId = activity.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                _statusBarHeight = activity.Resources.GetDimensionPixelSize(resourceId);
            }
        }

        private void CalculateBottomSpace(int softButtonsHeight)
        {
            if (softButtonsHeight > 0)
            {
                _bottomSpace = _bottomSpace - (softButtonsHeight - (int)(Math.Round(_statusBarHeight * 1.2)));
            }
        }

        #endregion
    }
}