using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Resources.Strings;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class UploadingResultViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;
        private readonly IUserService _userService;

        #endregion

        #region Variables

        private DataUploadingState _uploadingState;
        private LocalUploadingResultTextModel _uploadingResultModel;

        #endregion

        public UploadingResultViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService,
            IUserService userService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;
            _userService = userService;

            ShowUrlCommand = new MvxAsyncCommand(async () => await ShowUrlAsync());
            RetakeCommand = new MvxAsyncCommand(async () => await TryRetakeDataAsync());
            InitControls();
        }

        #region Properties

        private string _mainText;
        public string MainText
        {
            get => _mainText;
            set => SetProperty(ref _mainText, value);
        }

        private ImageSource _mainImage;
        public ImageSource MainImage
        {
            get => _mainImage;
            set => SetProperty(ref _mainImage, value);
        }

        private ImageSource _bottomImage;
        public ImageSource BottomImage
        {
            get => _bottomImage;
            set => SetProperty(ref _bottomImage, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        private string _bottomText;
        public string BottomText
        {
            get => _bottomText;
            set => SetProperty(ref _bottomText, value);
        }

        private bool _isSuccededUploading;
        public bool IsSuccededUploading
        {
            get => _isSuccededUploading;
            set => SetProperty(ref _isSuccededUploading, value);
        }

        private bool _isWarningUploading;
        public bool IsWarningUploading
        {
            get => _isWarningUploading;
            set => SetProperty(ref _isWarningUploading, value);
        }

        private bool _isBottomTextVisible;
        public bool IsBottomTextVisible
        {
            get => _isBottomTextVisible;
            set => SetProperty(ref _isBottomTextVisible, value);
        }


        private bool _isUrlPresent;
        public bool IsUrlPresent
        {
            get => _isUrlPresent;
            set => SetProperty(ref _isUrlPresent, value);
        }

        private Xamarin.Forms.Color _bottomTextColor;
        public Xamarin.Forms.Color BottomTextColor
        {
            get => _bottomTextColor;
            set => SetProperty(ref _bottomTextColor, value);
        }

        #endregion

        #region Commands

        public IMvxCommand ShowUrlCommand { get; private set; }
        public IMvxCommand RetakeCommand { get; private set; }

        #endregion

        #region Override Methods

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.White;
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();

            using (_userDialogs.Loading())
            {
                SetImagesAndText();
                await ClearDataAsync();
            }
        }

        protected async override Task TryBackPage()
        {
            //return base.TryBackPage();
        }

        #endregion

        #region Private Methods

        private async Task InvalidateTokenAsync()
        {
            var invalidateResult = await _userService.InvalidateTokenAsync();
            if (invalidateResult is null || !invalidateResult.Valid)
            {
                SaveErrorMessage(Constants.ApiInvalidateTokenError, _localDataService.Token, invalidateResult?.Message);
                //_userDialogs.Toast(Constants.InvalidateTokenError);
            }
        }

        private void InitControls()
        {
            IsSuccededUploading = false;
            IsWarningUploading = false;
            IsBottomTextVisible = false;
            _uploadingResultModel = new LocalUploadingResultTextModel();
            _uploadingState = _localDataService.UploadingState;
        }

        private void SetImagesAndText()
        {
            _uploadingResultModel = _localDataService.UploadingResultTextModel;

            CheckAndSetDataUploadingState();
            //set image sources, colors and default text
            if (_uploadingResultModel?.State == 1)
            {
                MainImage = ImageSource.FromFile("img_success_uploading.png");
                MainText = Strings.SuccessfulUploading;
                Description = Strings.SuccessfullyPassedVerificationDescription;
                IsSuccededUploading = true;
                //Url = "https://google_test.com";
            }
            if (_uploadingResultModel?.State == 2)
            {
                MainImage = ImageSource.FromFile("img_warning_uploading.png");
                SetWarningControl();
            }
            if (_uploadingResultModel?.State == 3)
            {
                MainImage = ImageSource.FromFile("img_error_uploading.png");
                MainText = Strings.ErrorUploading;
                Description = Strings.ErrorPassedVerificationDescription;
                BottomText = Strings.ErrorUploadingBottomText;
                BottomTextColor = Constants.ErrorColor;
                BottomImage = ImageSource.FromFile("img_error_uploading_info.png");
                IsBottomTextVisible = true;
            }
            //if uploading result from server is valid, set text data from server
            if (!string.IsNullOrWhiteSpace(_uploadingResultModel?.Title)
                && !string.IsNullOrWhiteSpace(_uploadingResultModel?.Description))
            {
                MainText = _uploadingResultModel?.Title;
                Description = _uploadingResultModel?.Description;
                BottomText = _uploadingResultModel?.BottomText;
                Url = _uploadingResultModel?.Url;
            }
            IsUrlPresent = string.IsNullOrWhiteSpace(_uploadingResultModel?.Url) ? false : true;
        }

        private void CheckAndSetDataUploadingState()
        {
            //if flow was complete, get state from server
            if (_uploadingResultModel != null && _uploadingResultModel.State > 0)
            {
                return;
            }
            //if flow wasn't complete, get state by local state
            if (_uploadingState == DataUploadingState.Succeded)
            {
                _uploadingResultModel.State = 1;
            }
            if (_uploadingState == DataUploadingState.UncorrectlyPhoto
                || _uploadingState == DataUploadingState.SecondFlowInProgress
                || _uploadingState == DataUploadingState.SecondFlowUploadingError)
            {
                _uploadingResultModel.State = 2;
            }
            if (_uploadingState == DataUploadingState.Unsucceded
               || _uploadingState == DataUploadingState.Default)
            {
                _uploadingResultModel.State = 3; 
            }
        }

        private void SetWarningControl()
        {
            if (_localDataService.UploadingCount >= 3)
            {
                MainText = Strings.WarningLastUploading;
                Description = Strings.WarningLastPassedVerificationDescription;
                BottomText = Strings.WarningLastUploadingBottomText;
                BottomTextColor = Constants.WarningColor;
                BottomImage = ImageSource.FromFile("img_info.png");
                IsBottomTextVisible = true;
                return;
            }
            IsWarningUploading = true;
            MainText = Strings.WarningUploading;
            Description = Strings.WarningPassedVerificationDescription;
        }

        private async Task ShowUrlAsync()
        {
            await Browser.OpenAsync(Url, BrowserLaunchMode.SystemPreferred);
        }

        private async Task TryRetakeDataAsync()
        {
            _localDataService.UploadingCount += 1;

            var userData = await _localDataService.GetUserDataAsync();
            userData.IsRetaked = true;
            await _localDataService.SetUserDataAsync(userData);

            for (int i = 0; i < Constants.MaxPhotoCount; i++)
            {
                await _localDataService.ChangeRetakedPhotoStatusAsync(i, false);
            }

            await _navigationService.Navigate<PersonalDataViewModel>();
            await base.TryBackPage();
        }

        private async Task ClearDataAsync()
        {
            if (_uploadingResultModel.State == 2 && _localDataService.UploadingCount < 3)
            {
                return;
            }

            await InvalidateTokenAsync();
            //_localDataService.ClearStorage();
            _localDataService.Token = string.Empty;
        }

        #endregion
    }
}
