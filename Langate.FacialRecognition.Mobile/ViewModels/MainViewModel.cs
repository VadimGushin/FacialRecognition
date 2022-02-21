using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Linq;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;
        private readonly IUserService _userService;

        #endregion
        public MainViewModel(IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService,
            IUserService userService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;
            _userService = userService;
            ShowFirstViewModelCommand = new MvxAsyncCommand(async () => await ShowFirstPageAsync());
        }

        #region Commands

        public IMvxCommand ShowFirstViewModelCommand { get; private set; }

        #endregion

        #region Override Methods

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        #endregion

        #region Private Methods

        private async Task ShowFirstPageAsync()
        {
            using (_userDialogs.Loading())
            {
                if (!CheckConnectionStatus())
                {
                    _userDialogs.Alert(Constants.InternetConnectionError);
                    await _navigationService.Navigate<StandaloneViewModel>();
                    return;
                }
                await Task.Delay(1000); //sometimes deppLink flow worked slowly then main app flow

                await _localDataService.InitDataAsync(); //was added
                _localDataService.Token = "0af35023-1fbd-47a0-bb32-5c8318535dc6";
                await GetInviteDataAsync();

                //var initResult = await _localDataService.InitDataAsync();
                //if (initResult)
                //{
                //    await GetInviteDataAsync();
                //    return;
                //}
                //if (_localDataService.UploadingState == Models.Enums.DataUploadingState.FirstStart)
                //{
                //    await _navigationService.Navigate<WelcomeViewModel>();
                //    return;
                //}
                //if (_localDataService.UploadingState == Models.Enums.DataUploadingState.Default
                //    || _localDataService.Token == Constants.DefaultToken)
                //{
                //    await _navigationService.Navigate<StandaloneViewModel>();
                //    return;
                //}
                //if (!_localDataService.UploadingResultModel.IsAgreementConfirm)
                //{
                //    await _navigationService.Navigate<UserAgreementViewModel>();
                //    return;
                //}
                //var userData = await _localDataService.GetUserDataAsync();
                //if (!userData.IsFull)
                //{
                //    await _localDataService.InitDataAsync(false);
                //    _localDataService.UploadingState = Models.Enums.DataUploadingState.SetData;
                //    await _navigationService.Navigate<PersonalDataViewModel>();
                //    return;
                //}
                //var userPhotos = await _localDataService.GetAllPhotosAsync();
                //if (userPhotos.Where(x => !x.IsFull).FirstOrDefault() != null)
                //{
                //    await _navigationService.Navigate<TakePhotoViewModel>();
                //    return;
                //}
                //await _navigationService.Navigate<ReviewDataViewModel>();
            }
        }

        private async Task GetInviteDataAsync()
        {
            var inviteData = await _userService.GetInviteModelAsync();
            if (inviteData != null && inviteData.Valid)
            {
                await _localDataService.SetInviteModelAsync(inviteData.Invite);
            }
            if (inviteData == null || !inviteData.Valid)
            {
                SaveErrorMessage(Constants.ApiGetInviteError, _localDataService.Token, inviteData?.Message);
                _userDialogs.Alert(Constants.InvalidTokenMessage);
                await _navigationService.Navigate<StandaloneViewModel>();
                return;
            }
            await _navigationService.Navigate<WelcomeViewModel>();
        }

        #endregion
    }
}
