using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Views;
using Acr.UserDialogs;
using Plugin.CurrentActivity;
using Android.Runtime;
using Android;
using Android.Support.V4.App;
using Xamarin.Forms.Platform.Android.AppLinks;
using Firebase;
using Android.Content;
using Firebase.DynamicLinks;
using Android.Gms.Tasks;
using Java.Lang;
using MvvmCross;
using MvvmCross.Navigation;
using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.Heplers;
using Android.Views;
using Langate.FacialRecognition.Mobile.Droid.Helpers;
using Xamarin.Essentials;

namespace Langate.FacialRecognition.Mobile.Droid
{
    [Activity(
        Label = "FacialRecognition.Mobile.Droid",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTask)]
    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                       AutoVerify = true,
                       Categories = new[]
                       {
                            Android.Content.Intent.CategoryDefault,
                            Android.Content.Intent.CategoryBrowsable
                       },
                       DataScheme = "http",
                       DataPathPrefix = "/root",
                       DataHost = "vctvirtual.net")]
    public class RootActivity : MvxFormsAppCompatActivity, IOnSuccessListener, IOnFailureListener
    {
        #region Properties

        internal static RootActivity Instance { get; private set; }
        private bool _isStarted = false;

        #endregion

        #region Override Methods

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, bundle);
            Plugin.InputKit.Platforms.Droid.Config.Init(this, bundle);

            Instance = this;

            base.OnCreate(bundle);

            AndroidAppLinks.Init(this);
            InitFirebaseApp();

            TryRequestPermissions();

            Window.SetSoftInputMode(SoftInput.AdjustResize);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                KeyboardHelper.AssistActivity(this, WindowManager);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected async override void OnStart()
        {
            base.OnStart();

            TryStartContinueSubmission();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var link = intent?.DataString;
            if (!string.IsNullOrWhiteSpace(link))
            {
                TrySaveToken(intent);
            }
        }

        #endregion

        #region Implementation

        public void OnSuccess(Object result)
        {
            if (result == null || !(result is PendingDynamicLinkData))
            {
                return;
            }
            var link = Intent?.DataString;
            if (!string.IsNullOrWhiteSpace(link))
            {
                TrySaveToken(Intent);
            }
        }

        public void OnFailure(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"RootActivity.OnFailure(), error: {ex.Message}");
            Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<StandaloneViewModel>();
        }

        #endregion

        #region Private Methods

        private void InitFirebaseApp()
        {
            var options = new FirebaseOptions.Builder()
                            .SetApplicationId("vctvirtual-d7012")
                            .SetApiKey("AIzaSyBAwDOThx1V9IeRZFNBXatzn9RD5xq8xnA")
                            .SetProjectId("vctvirtual-d7012")
                            .Build();

            FirebaseApp.InitializeApp(this, options);

            FirebaseDynamicLinks.Instance.GetDynamicLink(Intent)
                .AddOnSuccessListener(this, this)
                .AddOnFailureListener(this, this);
        }

        private void TrySaveToken(Intent intent)
        {
            var link = intent?.DataString;
            var localService = Mvx.IoCProvider.Resolve<ILocalDataService>();
            if (!string.IsNullOrWhiteSpace(localService.Token) && !localService.Token.Equals(Constants.DefaultToken))
            {
                //return;
                localService.ClearStorage();
            }
            var startIndex = link.IndexOf("token=");
            var token = intent?.DataString.Substring(startIndex + 6, link.Length - startIndex - 6);
            localService.InitDataAsync();
            localService.Token = token;
            localService.UploadingState = Models.Enums.DataUploadingState.FirstStart;
        }

        public void TryRequestPermissions()
        {
            if (CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted ||
                CheckSelfPermission(Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted ||
                CheckSelfPermission(Manifest.Permission.Camera) != (int)Permission.Granted ||
                CheckSelfPermission(Manifest.Permission.ReadPhoneState) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(
                    this,
                    new string[]
                    {
                            Manifest.Permission.WriteExternalStorage,
                            Manifest.Permission.ReadExternalStorage,
                            Manifest.Permission.Camera,
                            Manifest.Permission.ReadPhoneState,
                    }, 1);
            }
        }

        #endregion

        private async void TryStartContinueSubmission()
        {
            if (!_isStarted)
            {
                _isStarted = true;
                return;
            }

            var _localDataService = Mvx.IoCProvider.Resolve<ILocalDataService>();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet
                && _localDataService != null
                && !string.IsNullOrWhiteSpace(_localDataService.Token)
                && !_localDataService.Token.Equals(Constants.DefaultToken)
                && _localDataService.UploadingState != Models.Enums.DataUploadingState.SecondFlowInProgress
                && _localDataService.UploadingState != Models.Enums.DataUploadingState.SecondFlowUploadingError
                && _localDataService.UploadingState != Models.Enums.DataUploadingState.SecondFlowUploadingError)
            {
                var _navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();
                await _navigationService.Navigate<ContinueSubmissionViewModel>();
            }
        }
    }
}