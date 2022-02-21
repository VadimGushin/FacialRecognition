using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;

namespace Langate.FacialRecognition.Mobile.Droid
{
    [Activity(
        Label = "FacialRecognition",
        MainLauncher = true,
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreen : MvxFormsSplashScreenActivity<Setup, CoreApp, App>
    {
        public SplashScreen()
            : base(Resource.Layout.Splash)
        {
        }

        protected override Task RunAppStartAsync(Bundle bundle)
        {
            StartActivity(typeof(RootActivity));
            return Task.CompletedTask;
        }

        public override void OnBackPressed()
        {
            //base.OnBackPressed();
        }
    }
}