using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using Langate.FacialRecognition.MobileApi.Model;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        #region Services

        private readonly IUserService _userService;
        private readonly ILocalDataService _localDataService;

        #endregion

        public WelcomeViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            IUserService userService,
            ILocalDataService localDataService)
            : base(navigationService, userDialogs)
        {
            _userService = userService;
            _localDataService = localDataService;
            NextCommand = new MvxAsyncCommand(OnNextClickedAsync);
        }

        #region Properties

        private string _doctorName;
        public string DoctorName
        {
            get => _doctorName;
            set => SetProperty(ref _doctorName, value);
        }

        private string _siteUrl;
        public string SiteUrl
        {
            get => _siteUrl;
            set => SetProperty(ref _siteUrl, value);
        }

        private string _subjectNumber;
        public string SubjectNumber
        {
            get => _subjectNumber;
            set => SetProperty(ref _subjectNumber, value);
        }

        #endregion

        #region Commands

        public IMvxCommand NextCommand { get; private set; }

        #endregion

        #region Overrides

        public async override void ViewAppearing()
        {
            //StatusBarStyle = Models.Enums.StatusBarStyle.Visible;
            StatusBarColor = Models.Enums.StatusBarColor.Transparent;

            base.ViewAppearing();

            var inviteData = await _localDataService.GetInviteModelAsync();

            if (inviteData.InviteId == 0)
            {
                inviteData = await TryGetInviteAsync();
            }

            _localDataService.UploadingState = Models.Enums.DataUploadingState.SetData;

            DoctorName = string.IsNullOrWhiteSpace(inviteData.Study) ? "Doctor" : inviteData.PI;
            SiteUrl = string.IsNullOrWhiteSpace(inviteData.Location) ? "site/location name" : inviteData.Location;
            SubjectNumber = string.IsNullOrWhiteSpace(inviteData.SubjectNumber) ? "123/456/789" : inviteData.SubjectNumber;

            IsValidForm = true;
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        protected async override Task TryBackPage()
        {
        }

        #endregion

        #region Private Methods

        private async Task<ValidateInviteTokenDto> TryGetInviteAsync()
        {
            using (_userDialogs.Loading())
            {
                var inviteData = await _userService.GetInviteModelAsync();
                if (!inviteData.Valid || inviteData.Invite.InviteId == 0)
                {
                    _localDataService.ClearStorage();
                    _userDialogs.Alert(Constants.InvalidTokenMessage);
                    await _navigationService.Navigate<StandaloneViewModel>();
                    await base.TryBackPage();
                }
                await _localDataService.SetInviteModelAsync(inviteData.Invite);
                return inviteData.Invite;
            }
        }

        private async Task OnNextClickedAsync()
        {
            await _navigationService.Navigate<UserAgreementViewModel>();
            base.TryBackPage();
        }

        #endregion
    }
}
