using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Extensions;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using Langate.FacialRecognition.MobileApi.Model;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class ReviewDataViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;
        private readonly IUserService _userService;
        private readonly IPhotoUploadingService _photoUploadingService;
        private readonly IPlatformService _platformService;
        private readonly IOcrService _ocrService;

        #endregion

        #region Variables

        private const int _nameValidLenght = 3;
        private const int _middleNameValidLenght = 1;
        private const int _idValidLenght = 5;
        private bool _isRetakedWasComplete;

        #endregion

        public ReviewDataViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService,
            IUserService userService,
            IPhotoUploadingService photoUploadingService,
            IPlatformService platformService,
            IOcrService ocrService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;
            _userService = userService;
            _photoUploadingService = photoUploadingService;
            _platformService = platformService;
            _ocrService = ocrService;

            NextCommand = new MvxAsyncCommand(async () => { await OnNextClicked(); }); 
            RetakePhotoCommand = new MvxAsyncCommand(async () => { await RetakePhotoAsync(); });

            InitUserData();
        }

        #region Properties

        private string _buttonTitle;
        public string ButtonTitle
        {
            get => _buttonTitle;
            set
            {
                SetProperty(ref _buttonTitle, value);
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                SetProperty(ref _firstName, value);
                ValidateForm();
            }
        }

        private string _middleName;
        public string MiddleName
        {
            get => _middleName;
            set
            {
                SetProperty(ref _middleName, value);
                ValidateForm();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                SetProperty(ref _lastName, value);
                ValidateForm();
            }
        }

        private string _idValue;
        public string IdValue
        {
            get => _idValue;
            set
            {
                SetProperty(ref _idValue, value);
                ValidateForm();
            }
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set => SetProperty(ref _isErrorVisible, value);
        }

        private MvxObservableCollection<LocalPhotoModel> _photos;
        public MvxObservableCollection<LocalPhotoModel> Photos
        {
            get => _photos;
            set => SetProperty(ref _photos, value);
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private MvxObservableCollection<string> _genders;
        public MvxObservableCollection<string> Genders
        {
            get => _genders;
            set => SetProperty(ref _genders, value);
        }

        private string _selectedGender;
        public string SelectedGender
        {
            get => _selectedGender;
            set
            {
                SetProperty(ref _selectedGender, value);
            }
        }

        public string InitGender { get; set; }
        private string _idType { get; set; }
        private InviteResponseCreateRequestDto _apiUpdateResponseModel { get; set; }

        #endregion

        #region MvxIteractions

        public MvxInteraction<bool> ScrollIteraction { get; set; } = new MvxInteraction<bool>();

        #endregion

        #region Commands

        public IMvxCommand NextCommand { get; private set; }

        public IMvxCommand RetakePhotoCommand { get; private set; }

        #endregion

        #region Override Methods

        public async override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.White;
            Photos.Clear();
            var photos = await _localDataService.GetAllPhotosAsync();
            Photos.AddRange(photos.Select(x => new LocalPhotoModel { Photo = ImageSource.FromStream(() => new MemoryStream(x.Photo)) }));
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            StatusBarColor = Models.Enums.StatusBarColor.White;
            IsValidForm = true;
            IsErrorVisible = _localDataService.UploadingState == DataUploadingState.UncorrectlyId ? true : false;
            ButtonTitle = IsErrorVisible ? Resources.Strings.Strings.Confirm : Resources.Strings.Strings.Next;
        }

        protected async override Task TryBackPage()
        {
            if (_isRetakedWasComplete)
            {
                await _localDataService.ChangeRetakedPhotoStatusAsync(Constants.MaxPhotoCount - 1, false);
            }
            if (!_isRetakedWasComplete)
            {
                var userData = GetEditedUserDate();
                userData.IsRetaked = true;
                await _localDataService.SetUserDataAsync(userData);

                for (int i = 0; i < Constants.MaxPhotoCount - 1; i++)
                {
                    await _localDataService.ChangeRetakedPhotoStatusAsync(i, true);
                }
            }

            await base.TryBackPage();
        }

        #endregion

        #region Private Methods

        private void InitUserData()
        {
            var userData = _localDataService.GetUserDataAsync().Result;

            Photos = new MvxObservableCollection<LocalPhotoModel>();
            Genders = new MvxObservableCollection<string>(_localDataService.GetInviteModelAsync().Result.Gender.Values);

            SelectedGender = string.Empty;

            SelectedDate = userData.DateOfBirth;

            FirstName = userData.FirstName;
            LastName = userData.LastName;
            MiddleName = userData.MiddleName;
            InitGender = userData.Gender;
            IdValue = userData.IdValue;
            _idType = userData.IdType;
            _isRetakedWasComplete = userData.IsRetaked;

            _apiUpdateResponseModel = new InviteResponseCreateRequestDto();
        }

        private void ValidateForm()
        {
            if (FirstName.IsValid(Constants.EntryFirstLastNameValueKey, _nameValidLenght)
               && LastName.IsValid(Constants.EntryFirstLastNameValueKey, _nameValidLenght)
               && MiddleName.IsValid(Constants.EntryMiddleNameValueKey, _middleNameValidLenght)
               && !string.IsNullOrWhiteSpace(SelectedGender)
               && SelectedDate.Year > Constants.MinValidYear && SelectedDate < DateTime.Now
               && IdValue.IsValid(Constants.EntryIdValueKey, _idValidLenght))
            {
                IsValidForm = true;
                return;
            }
            IsValidForm = false;
        }

        private async Task OnNextClicked()
        {
            var userData = GetEditedUserDate();
            await _localDataService.SetUserDataAsync(userData);

            using (_userDialogs.Loading())
            {
                //IsErrorVisible = false;
                await TryUploadAndOcrGovernmentIdAsync(userData);
            }
        }

        private async Task RetakePhotoAsync()
        {
            var userData = GetEditedUserDate();
            userData.IsRetaked = true;
            await _localDataService.SetUserDataAsync(userData);

            for (int i = 0; i < Constants.MaxPhotoCount; i++)
            {
                await _localDataService.ChangeRetakedPhotoStatusAsync(i, false);
            }

            await _navigationService.Navigate<PersonalDataViewModel>();
            await base.TryBackPage();
        }

        private LocalUserDataModel GetEditedUserDate()
        {
            var userData = new LocalUserDataModel();
            userData.FirstName = FirstName;
            userData.LastName = LastName;
            userData.MiddleName = MiddleName;
            userData.Gender = SelectedGender;
            userData.DateOfBirth = SelectedDate;
            userData.IdValue = IdValue;
            userData.IdType = _idType;
            userData.IsFull = true;

            return userData;
        }

        #region Uploading Methods

        private async Task TryUploadAndOcrGovernmentIdAsync(LocalUserDataModel userData)
        {
            await CheckRetakedDataBeforeUploadingAsync(userData);
            _localDataService.UploadingState = Models.Enums.DataUploadingState.FirstFlowInProgress;
            if (!CheckConnectionStatus())
            {
                SaveErrorMessage(Constants.InternetConnectionError, _localDataService.Token, string.Empty);
                SetUnsuccededUploadingResult(DataUploadingState.FirstFlowUploadingError, Constants.InternetConnectionError);
                _userDialogs.Toast(Constants.InternetConnectionError);
                return;
            }
            await CreateInviteResponseAsync(userData);
            if (_localDataService.UploadingResultModel.ResponseId == 0)
            {
                SetUnsuccededUploadingResult(DataUploadingState.FirstFlowUploadingError, Constants.UploadingDataError);
                return;
            }
            var uploadIdResult = await UploadGovernmentIdAsync();
            if (!uploadIdResult)
            {
                SetUnsuccededUploadingResult(DataUploadingState.FirstFlowUploadingError, Constants.UploadingImageError);
                return;
            }
            var updateResponseResult = await UpdateResponseWithGovernmentIdAsync();
            if (!updateResponseResult)
            {
                SetUnsuccededUploadingResult(DataUploadingState.FirstFlowUploadingError, Constants.UploadingDataError);
                return;
            }
            //if user data isn't valid, continue workflow
            if (IsErrorVisible)
            {
                SetUnsuccededUploadingResult(DataUploadingState.UncorrectlyId);
                await _navigationService.Navigate<UploadingImagesViewModel>();
                await base.TryBackPage();
                return;
            }
            await RunOcrIdAsync();
            if (_localDataService.UploadingResultModel.OcrId == 0)
            {
                SetUnsuccededUploadingResult(DataUploadingState.FirstFlowUploadingError, Constants.UploadingDataError);
                return;
            }
            await RunOcrOfIdAsync(userData);
            if (_localDataService.UploadingResultModel.IsOcrOfComplete)
            {
                await _navigationService.Navigate<UploadingImagesViewModel>();
                await base.TryBackPage();
            }
            SetUnsuccededUploadingResult(DataUploadingState.UncorrectlyId);
        }

        private async Task CreateInviteResponseAsync(LocalUserDataModel userData)
        {
            if (userData == null)
            {
                userData = GetEditedUserDate();
            }
            if (_localDataService.UploadingResultModel.IsInviteResponseCreated)
            {
                return;
            }

            var inviteModel = await GetUnviteModelAsync();

            _apiUpdateResponseModel.InviteId = inviteModel.InviteId;
            _apiUpdateResponseModel.StudyId = inviteModel.StudyId;
            //_apiUpdateResponseModel.SubjectId = inviteModel.SubjectId;
            _apiUpdateResponseModel.SubjectNumber = inviteModel.SubjectNumber;
            _apiUpdateResponseModel.EuaId = _localDataService.UploadingResultModel.EuaId;
            _apiUpdateResponseModel.OsVersion = _platformService.GetOSVersion();

            _apiUpdateResponseModel.PartialIdentifiers = new PartialIdentifiersDto()
            {
                FirstName = userData.FirstName,
                MiddleName = userData.MiddleName,
                LastName = userData.LastName,
                DOB = userData.DateOfBirth.ToShortDateString(),
                Gender = inviteModel.Gender.FirstOrDefault(x => x.Value == userData.Gender).Key,
                IdNumber = userData.IdValue,
                IdType = inviteModel.IdType.FirstOrDefault(x => x.Value == userData.IdType).Key
            };

            var apiResponse = await _userService.CreateResponseAsync(_apiUpdateResponseModel);
            if (apiResponse == null || !apiResponse.Valid)
            {
                SaveErrorMessage(Constants.ApiCreateResponseError, _localDataService.Token, apiResponse?.Message);
                return;
            }
            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.ResponseId = apiResponse.InviteResponseId;
            uploadingModel.IsInviteResponseCreated = true;
            _localDataService.UploadingResultModel = uploadingModel;
        }

        private async Task<bool> UploadGovernmentIdAsync()
        {
            var photoModel = await _localDataService.GetPhotoAsync(0);
            if (photoModel.IsImageUploaded)
            {
                return true;
            }
            var uploadingResult = await _photoUploadingService.UploadPhotoAsync(0);
            if (uploadingResult == null || !uploadingResult.Valid)
            {
                SaveErrorMessage(Constants.ApiUploadPhotoError, _localDataService.Token, uploadingResult?.Message);
                return false;
            }
            await _localDataService.SetImageId(0, uploadingResult.ImageId);
            //_apiUpdateResponseModel.ImageDocumentId = uploadingResult.ImageId;
            return true;
        }

        private async Task<bool> UpdateResponseWithGovernmentIdAsync()
        {
            var governmentPhotoId = (await _localDataService.GetPhotoAsync(0)).ImageId;
            if (governmentPhotoId == 0 ||
                !_localDataService.UploadingResultModel.IsInviteResponseCreated
                || _localDataService.UploadingResultModel.ResponseId <= 0)
            {
                SaveErrorMessage(Constants.ApiUpdateResponseError, _localDataService.Token, "GovernmentPhotoId=0 | ResponseId=0");
                return false;
            }
            var requestModel = new InviteResponseUpdateDocumentRequestDto()
            {
                InviteResponseId = _localDataService.UploadingResultModel.ResponseId,
                ImageDocumentId = governmentPhotoId
            };
            var apiResponse = await _userService.UpdateResponseWithGovernmentIdAsync(requestModel);
            if (apiResponse == null || !apiResponse.Valid)
            {
                SaveErrorMessage(Constants.ApiUpdateResponseError, _localDataService.Token, apiResponse?.Message);
                return false;
            }
            return true;
        }

        private async Task RunOcrIdAsync()
        {
            if (_localDataService.UploadingResultModel.IsOcrComplete)
            {
                return;
            }
            var _documentId = (await _localDataService.GetPhotoAsync(0)).ImageId;
            var ocrResulr = await _ocrService.OcrGovernmentIdAsync(_documentId, _localDataService.UploadingResultModel.ResponseId);
            if (ocrResulr == null || !ocrResulr.Valid)
            {
                SaveErrorMessage(Constants.ApiOcrError, _localDataService.Token, ocrResulr?.Message);
                return;
            }
            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.OcrId = ocrResulr.OcrId;
            uploadingModel.IsOcrComplete = true;
            _localDataService.UploadingResultModel = uploadingModel;
        }

        private async Task RunOcrOfIdAsync(LocalUserDataModel userData)
        {
            if (_localDataService.UploadingResultModel.IsOcrOfComplete)
            {
                return;
            }
            var userRequestData = new OcrOfIdRequestDto()
            {
                FirstName = userData.FirstName,
                MiddleName = userData.MiddleName,
                LastName = userData.LastName,
                DOB = userData.DateOfBirth.ToShortDateString(),
                Gender = userData.Gender,
                IdNumber = userData.IdValue
            };
            var ocrApiResult = await _ocrService.OcrOfGovernmentIdAsync(userRequestData, _localDataService.UploadingResultModel.OcrId);
            if (ocrApiResult == null || !ocrApiResult.Valid)
            {
                SaveErrorMessage(Constants.ApiOcrOfError, _localDataService.Token, ocrApiResult?.Message);
                //_userDialogs.Toast(ocrApiResult.Message ?? Constants.UploadingDataError);
                return;
            }

            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.IsOcrOfComplete = true;
            _localDataService.UploadingResultModel = uploadingModel;
        }

        private async Task CheckRetakedDataBeforeUploadingAsync(LocalUserDataModel userData)
        {
            try
            {
                if (!_localDataService.UploadingResultModel.IsInviteResponseCreated)
                {
                    return;
                }

                var dataResponse = await _userService.GetUserPartialIdentifiersAsync(_localDataService.UploadingResultModel.ResponseId);
                var inviteModel = await GetUnviteModelAsync();
                var localGovernmentPhoto = await _localDataService.GetPhotoAsync(0);
                if (dataResponse == null
                    || !dataResponse.Valid
                    || dataResponse.PartialIdentifiers == null
                    || !dataResponse.PartialIdentifiers.FirstName.Equals(userData.FirstName)
                    || !dataResponse.PartialIdentifiers.LastName.Equals(userData.LastName)
                    || !dataResponse.PartialIdentifiers.MiddleName.Equals(userData.MiddleName)
                    || !dataResponse.PartialIdentifiers.Gender.Equals(inviteModel.Gender.FirstOrDefault(x => x.Value == userData.Gender).Key)
                    || !dataResponse.PartialIdentifiers.DOB.Equals(userData.DateOfBirth.TryFormat())
                    || !dataResponse.PartialIdentifiers.IdNumber.Equals(userData.IdValue)
                    || !dataResponse.PartialIdentifiers.IdType.Equals(inviteModel.IdType.FirstOrDefault(x => x.Value == userData.IdType).Key)
                    || dataResponse.ImageDocumentId != localGovernmentPhoto.ImageId)
                {
                    await ClearLocalUserResponseData();
                    return;
                }

                if (_localDataService.UploadingResultModel.IsOcrOfComplete)
                {
                    await _navigationService.Navigate<UploadingImagesViewModel>();
                    await base.TryBackPage();
                }
            }
            catch (Exception ex)
            {
                SaveErrorMessage(Constants.SomethingWrongError, _localDataService.Token, ex.Message);
                await ClearLocalUserResponseData();
            }

        }

        #endregion

        private void SetUnsuccededUploadingResult(DataUploadingState state, string userMessage = "")
        {
            _localDataService.UploadingState = state;
            if (state == DataUploadingState.FirstFlowUploadingError)
            {
                //_userDialogs.Alert(string.IsNullOrWhiteSpace(userMessage) ? Constants.SomethingWrongError : userMessage);
            }
            if (state == DataUploadingState.UncorrectlyId)
            {
                IsErrorVisible = true;
                ButtonTitle = Resources.Strings.Strings.Confirm;
                ScrollIteraction.Raise(true);
            }
            ValidateForm();
        }


        private async Task ClearLocalUserResponseData()
        {
            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.ResponseId = 0;
            uploadingModel.IsInviteResponseCreated = false;
            //uploadingModel.OcrId = 0;
            //uploadingModel.IsOcrComplete = false;
            //uploadingModel.IsOcrOfComplete = false;
            _localDataService.UploadingResultModel = uploadingModel;
            await _localDataService.SetImageId(0, 0, false);
        }

        private async Task<ValidateInviteTokenDto> GetUnviteModelAsync()
        {
            var inviteModel = await _localDataService.GetInviteModelAsync();
            if (inviteModel.InviteId == 0 || inviteModel.StudyId == 0)
            {
                inviteModel = (await _userService.GetInviteModelAsync()).Invite;
            }
            return inviteModel;
        }

        #endregion
    }
}
