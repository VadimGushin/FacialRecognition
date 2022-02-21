using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using Langate.FacialRecognition.MobileApi.Model;
using MvvmCross.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class UploadingImagesViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;
        private readonly IPhotoUploadingService _photoUploadingService;
        private readonly IUserService _userService;
        private readonly IPlatformService _platformService;
        private readonly IOcrService _ocrService;

        #endregion

        public UploadingImagesViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService,
            IPhotoUploadingService photoUploadingService,
            IUserService userService,
            IPlatformService platformService,
            IOcrService ocrService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;
            _photoUploadingService = photoUploadingService;
            _userService = userService;
            _platformService = platformService;
            _ocrService = ocrService;

            InitControls();
        }

        #region Properties

        private List<LocalUserPhotoModel> _photos { get; set; }
        private InviteResponseUpdateImagesRequestDto _apiUpdateResponseModel { get; set; }
        private int _progress { get; set; }

        private string _progressText;
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

        private double _progressValue;
        public double ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        private bool _isFrontalFaceUpdated { get; set; }

        #endregion

        #region Override Methodds

        protected async override Task TryBackPage()
        {
            //return base.TryBackPage();
        }

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.White;
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();

            Task.Run(async () => 
            { 
                await UploadUserDataAsync();
            });            
        }

        #endregion

        #region Private Methods

        #region Uploading Methods

        private async Task UploadUserDataAsync()
        {
            _localDataService.UploadingState = Models.Enums.DataUploadingState.SecondFlowInProgress;
            if (!CheckConnectionStatus())
            {
                SaveErrorMessage(Constants.InternetConnectionError, _localDataService.Token, string.Empty);
                await SetUnsuccededUploadingResult(DataUploadingState.SecondFlowUploadingError, Constants.InternetConnectionError);
                _userDialogs.Toast(Constants.InternetConnectionError);
                return;
            }
            var imageUploadingResult = await UploadImagesAsync();
            if (!imageUploadingResult)
            {
                await SetUnsuccededUploadingResult(DataUploadingState.SecondFlowUploadingError, Constants.UploadingImageError);
                return;
            }
            var updateResponseResult = await UpdateInviteResponseAsync();
            if (!updateResponseResult)
            {
                await SetUnsuccededUploadingResult(DataUploadingState.SecondFlowUploadingError, Constants.UploadingDataError);
                return;
            }
            //todo ProcessImage
            var processImageResult = await ProcessUserImageAsync();
            if (!processImageResult)
            {
                await SetUnsuccededUploadingResult(DataUploadingState.UncorrectlyPhoto, Constants.UploadingDataError);
                return;
            }
            await RecognizeImageAsync();
            if (!_localDataService.UploadingResultModel.IsRecognizeComplete)
            {
                await SetUnsuccededUploadingResult(DataUploadingState.UncorrectlyPhoto);
                return;
            }
            await ProvideDecisionApiServiceAsync();
            if (!_localDataService.UploadingResultModel.IsDecisionComplete)
            {
                await SetUnsuccededUploadingResult(DataUploadingState.Unsucceded);
                return;
            }

            _localDataService.UploadingState = Models.Enums.DataUploadingState.Succeded;
            //await _localDataService.SetUploadingResultAsync(true);
            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.IsUploadingComplete = true;
            _localDataService.UploadingResultModel = uploadingModel;
            //UpdateProgressValue();
            await _navigationService.Navigate<UploadingResultViewModel>();
            await base.TryBackPage();
        }

        private async Task<bool> UploadImagesAsync()
        {
            for (int i = 1; i < Constants.MaxPhotoCount; i++)
            {
                var photoModel = await _localDataService.GetPhotoAsync(i);
                CheckFrontalFaceUploading(photoModel.PhotoType, photoModel.IsImageUploaded);
                if (photoModel.IsImageUploaded)
                {
                    UpdateProgressValue(2);
                    continue;
                }
                var uploadingResult = await _photoUploadingService.UploadPhotoAsync(i);
                if (uploadingResult == null || !uploadingResult.Valid)
                {
                    SaveErrorMessage(Constants.ApiUploadPhotoError, _localDataService.Token, uploadingResult?.Message);
                    await SetUnsuccededUploadingResult(DataUploadingState.SecondFlowUploadingError, Constants.UploadingImageError);
                    return false;
                }
                await _localDataService.SetImageId(i, uploadingResult.ImageId);
                _localDataService.UploadingResultModel.IsRecognizeComplete = false;
                _localDataService.UploadingResultModel.IsDecisionComplete = false;
                UpdateProgressValue(2);
            }
            return true;
        }

        private async Task SetImagesIdToApiResponseAsync()
        {
            var photos = await _localDataService.GetAllPhotosAsync();
            _apiUpdateResponseModel.ImageFrontalId = photos.Where(x => x.PhotoType == ImageType.FrontalFace).FirstOrDefault().ImageId;
            _apiUpdateResponseModel.ImageLeftId = photos.Where(x => x.PhotoType == ImageType.FaceFromLeft).FirstOrDefault().ImageId;
            _apiUpdateResponseModel.ImageRightId = photos.Where(x => x.PhotoType == ImageType.FaceFromRight).FirstOrDefault().ImageId;
        }

        private async Task<bool> UpdateInviteResponseAsync()
        {
            await SetImagesIdToApiResponseAsync();
            if (_apiUpdateResponseModel.ImageFrontalId == 0
                || _apiUpdateResponseModel.ImageLeftId == 0
                || _apiUpdateResponseModel.ImageRightId == 0
                || !_localDataService.UploadingResultModel.IsInviteResponseCreated
                || _localDataService.UploadingResultModel.ResponseId <= 0)
            {
                SaveErrorMessage(Constants.ApiUpdateResponseError, _localDataService.Token, "somePhotoId=0 | responseId<=0");
                return false;
            }

            _apiUpdateResponseModel.InviteResponseId = _localDataService.UploadingResultModel.ResponseId;

            var apiResponse = await _userService.UpdateResponseWithUserFaceIdAsync(_apiUpdateResponseModel);
            UpdateProgressValue(1);
            if (apiResponse == null || !apiResponse.Valid)
            {
                SaveErrorMessage(Constants.ApiUpdateResponseError, _localDataService.Token, apiResponse?.Message);
                return false;
            }
            return true;
        }

        private async Task<bool> ProcessUserImageAsync()
        {
            if (!_isFrontalFaceUpdated && _localDataService.UploadingResultModel.IsRecognizeComplete)
            {
                UpdateProgressValue(1);
                return true;
            }
            var _frontalFaceId = (await _localDataService.GetPhotoAsync(1)).ImageId;
            var processResult = await _userService.ProcessUserPhotoAsync(_frontalFaceId);
            UpdateProgressValue(1);
            if (!processResult.Valid)
            {
                SaveErrorMessage(Constants.ApiProcessImageError, _localDataService.Token, processResult?.Message);
                //_userDialogs.Toast(processResult.Message);
                return false;
            }
            return true;
        }

        private async Task RecognizeImageAsync()
        {
            if (!_isFrontalFaceUpdated && _localDataService.UploadingResultModel.IsRecognizeComplete)
            {
                UpdateProgressValue(1);
                SetLocalRecognizeResult();
                return;
            }
            var _frontalFaceId = (await _localDataService.GetPhotoAsync(1)).ImageId;
            var recognizeResult = await _userService.RecognizeFaceAsync(_frontalFaceId, _localDataService.UploadingResultModel.ResponseId);
            UpdateProgressValue(1);
            if (!recognizeResult.Valid)
            {
                SaveErrorMessage(Constants.ApiRecognizeFaceError, _localDataService.Token, recognizeResult?.Message);
                //_userDialogs.Toast(recognizeResult.Message);
                return;
            }
            SetLocalRecognizeResult();
        }

        private async Task ProvideDecisionApiServiceAsync()
        {
            if (_localDataService.UploadingResultModel.IsDecisionComplete)
            {
                UpdateProgressValue(1);
                return;
            }
            var decisionResult = await _userService.ProvideDecisionApiServiceAsync(_localDataService.UploadingResultModel.ResponseId, _localDataService.UploadingResultModel.OcrId);
            UpdateProgressValue(1);
            if (decisionResult == null || !decisionResult.Valid)
            {
                SaveErrorMessage(Constants.ApiDecisionError, _localDataService.Token, decisionResult?.Message);
                return;
            }
            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.IsDecisionComplete = true;
            _localDataService.UploadingResultModel = uploadingModel;

            _localDataService.UploadingResultTextModel = new LocalUploadingResultTextModel(
                decisionResult.Title,
                decisionResult.Message,
                decisionResult.BottomLine,
                decisionResult.Url,
                decisionResult.State);
        }

        #endregion

        private async Task SetUnsuccededUploadingResult(DataUploadingState state, string userMessage = "")
        {
            _localDataService.UploadingState = state;
            if (state == DataUploadingState.SecondFlowUploadingError)
            {
                //_userDialogs.Alert(string.IsNullOrWhiteSpace(userMessage) ? Constants.SomethingWrongError : userMessage);              
            }
            //if (state == DataUploadingState.UncorrectlyPhoto || state == DataUploadingState.Unsucceded)
            //{
            //    await _navigationService.Navigate<UploadingResultViewModel>();
            //}
            await _navigationService.Navigate<UploadingResultViewModel>();
            await base.TryBackPage();
        }

        private void InitControls()
        {
            ProgressText = "0%";
            _progress = 0;
            _isFrontalFaceUpdated = false;
            _apiUpdateResponseModel = new InviteResponseUpdateImagesRequestDto();
        }

        private void UpdateProgressValue(int progress = 0)
        {
            InvokeOnMainThread(() =>
            {
                _progress += progress;
                ProgressText = $"{_progress * 10}%";
                ProgressValue = (double)((double)_progress / 10);
            });
        }

        private void CheckFrontalFaceUploading(ImageType type, bool isUploaded)
        {
            if (type == ImageType.FrontalFace && !isUploaded)
            {
                _isFrontalFaceUpdated = true;
            }
        }

        private void SetLocalRecognizeResult(bool result = true)
        {
            var uploadingModel = _localDataService.UploadingResultModel;
            uploadingModel.IsRecognizeComplete = result;
            _localDataService.UploadingResultModel = uploadingModel;
        }

        #endregion
    }
}
