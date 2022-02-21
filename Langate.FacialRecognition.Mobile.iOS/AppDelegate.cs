using Acr.UserDialogs;
using Firebase.DynamicLinks;
using Foundation;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels;
using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.Navigation;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;

namespace Langate.FacialRecognition.Mobile.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : MvxFormsApplicationDelegate<Setup, CoreApp, App>
    {
        #region Variables

        private bool _isFirstStarted = false;

        #endregion

        #region Overrides

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Firebase.Core.App.Configure();
            var finishedLaunchingResult = base.FinishedLaunching(app, options);
            _isFirstStarted = true;
            return finishedLaunchingResult;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var dynamicLink = DynamicLinks.SharedInstance.FromCustomSchemeUrl(url);
            //Mvx.IoCProvider.Resolve<IUserDialogs>().Alert($"DynamicLink from OpenUrl1, link={dynamicLink.Url}");
            if (dynamicLink != null)
            {
                //Mvx.IoCProvider.Resolve<IUserDialogs>().Alert($"DynamicLink from OpenUrl1, dynamicLink={dynamicLink.Url}");
                return true;
            }

            //return base.OpenUrl(app, url, options);
            return false;
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            var dynamicLink = DynamicLinks.SharedInstance.FromCustomSchemeUrl(url);
            //Mvx.IoCProvider.Resolve<IUserDialogs>().Alert($"DynamicLink from OpenUrl2: {dynamicLink?.Url?.ToString()}");
            //return base.OpenUrl(application, url, sourceApplication, annotation);
            return true;
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            //Mvx.IoCProvider.Resolve<IUserDialogs>().Alert($"ContinueUserActivity, absStr={userActivity.WebPageUrl.AbsoluteString}");
            DynamicLinks.SharedInstance.HandleUniversalLink(userActivity.WebPageUrl, (dynamicLink, error) =>
            {
                //Mvx.IoCProvider.Resolve<IUserDialogs>().Alert($"DynamicLink from ContinueUserActivity.1: {dynamicLink?.Url?.ToString()}");
            });
            if (userActivity.WebPageUrl != null && userActivity.WebPageUrl.AbsoluteString.Contains("token="))
            {
                HandleDeepLink(userActivity.WebPageUrl.AbsoluteString);
            }

            return true;
        }

        public async override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);
            await TryStartContinueSubmission();
        }

        #endregion

        #region Private Methods

        private void HandleDeepLink(string link)
        {
            var localService = Mvx.IoCProvider.Resolve<ILocalDataService>();
            if (!string.IsNullOrWhiteSpace(localService.Token) && !localService.Token.Equals(Constants.DefaultToken))
            {
                //return;
                localService.ClearStorage();
            }
            var startIndex = link.IndexOf("token=");
            var endIndex = link.IndexOf("&apn=");
            var token = link.Substring(startIndex + 6, endIndex - startIndex - 6);
            localService.InitDataAsync();
            localService.Token = token;
            localService.UploadingState = Models.Enums.DataUploadingState.FirstStart;

            //Mvx.IoCProvider.Resolve<IUserDialogs>().Alert($"DynamicLink from HandleDeepLink, token={token}");
            Task.Run(async() =>
            { 
                await TryStartContinueSubmission();
            });           
        }

        private async Task TryStartContinueSubmission()
        {
            if (!_isFirstStarted)
            {
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

        #endregion
    }
}
