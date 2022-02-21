using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class UserAgreementViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;
        private readonly IPlatformService _platformService;
        private readonly IUserService _userService;

        #endregion

        public UserAgreementViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService,
            IPlatformService platformService,
            IUserService userService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;
            _platformService = platformService;
            _userService = userService;

            NextCommand = new MvxAsyncCommand(ShowNextPageAsync);
            InitControls();
        }

        #region Properties

        private string _acceptedText;
        public string AcceptedText
        {
            get => _acceptedText;
            set => SetProperty(ref _acceptedText, value);
        }

        private bool _isAccepted;
        public bool IsAccepted
        {
            get => _isAccepted;
            set 
            {
                IsValidForm = !string.IsNullOrWhiteSpace(AcceptedText) ? value : false;
                SetProperty(ref _isAccepted, value);
            }
        }

        private int _agreementId { get; set; }

        #endregion

        #region Commands

        public IMvxCommand NextCommand { get; private set; }

        #endregion

        #region Override methods

        public async override void ViewAppearing()
        {
            StatusBarColor = Models.Enums.StatusBarColor.White;
            IsValidForm = IsAccepted;
            base.ViewAppearing();
            using (_userDialogs.Loading())
            {
                if (!CheckConnectionStatus())
                {
                    //IsValidForm = false;
                    return;
                }
                var agreementModel = await _userService.GetAgreementAsync();
                if (agreementModel.Valid)
                {
                    AcceptedText = agreementModel.Message;
                    _agreementId = agreementModel.EuaId;
                    //IsValidForm = true;
                    //SetLocalData();
                    //IsAccepted = agreementModel.Skip;
                }
                if (agreementModel == null || !agreementModel.Valid)
                {
                    //await _navigationService.Navigate<StandaloneViewModel>();
                    _userDialogs.Alert(Constants.AgreementResponseError);
                    SaveErrorMessage(Constants.ApiGetAgreementError, _localDataService.Token, agreementModel?.Message);
                    //IsValidForm = false;
                }
            }
        }

        protected async override Task TryBackPage()
        {
            await _navigationService.Navigate<WelcomeViewModel>();
            base.TryBackPage();
        }

        #endregion

        #region Private methods

        private void InitControls()
        {
            //TODO: temp text
            //AcceptedText = Constants.DefaultAgreementText;
            IsAccepted = false;
        }

        private async Task ShowNextPageAsync()
        {
            SetLocalData();
            _localDataService.UploadingState = Models.Enums.DataUploadingState.SetData;
            //await _localDataService.SetUserAgreementAsync(_agreementId);
            await _navigationService.Navigate<PersonalDataViewModel>();
            base.TryBackPage();
        }

        private void SetLocalData()
        {
            var uploadModel = _localDataService.UploadingResultModel;
            uploadModel.IsAgreementConfirm = true;
            uploadModel.EuaId = _agreementId;
            _localDataService.UploadingResultModel = uploadModel;
        }

        #endregion
    }
}
