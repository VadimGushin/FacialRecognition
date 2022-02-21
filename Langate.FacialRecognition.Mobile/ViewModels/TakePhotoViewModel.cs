using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.Plugin.PictureChooser;
using MvvmCross.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class TakePhotoViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;
        private readonly IImageResizeService _imageResizeService;

        #endregion

        public TakePhotoViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService,
            IImageResizeService imageResizeService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;
            _imageResizeService = imageResizeService;

            IsFirstShowing = true;

            CreatePhotoCommand = new MvxAsyncCommand(async () => { /*await CreatePhotoAsync();*/ _userDialogs.ShowLoading(); });
            ChangeCameraCommand = new MvxAsyncCommand(async () => { /*await ChangeCameraCommandAsync();*/ });
            GetImageFromLibraryCommand = new MvxAsyncCommand(async () => { await GetImageFromLibraryAsync(); });

            IsUserFaceImageVisible = true;

            UserPhoto = _localDataService.GetNextPhotoAsync().Result;
        }

        #region Properties

        private bool IsFirstShowing { get; set; }

        private LocalUserPhotoModel _userPhoto;
        public LocalUserPhotoModel UserPhoto
        {
            get => _userPhoto;
            set => SetProperty(ref _userPhoto, value);
        }

        private bool _isUserFaceImageVisible;
        public bool IsUserFaceImageVisible
        {
            get => _isUserFaceImageVisible;
            set => SetProperty(ref _isUserFaceImageVisible, value);
        }

        public bool _isUserDataRetaked { get; set; }

        #endregion

        #region MvxIteractions

        public MvxInteraction<LocalUserPhotoModel> LayoutIteraction { get; set; } = new MvxInteraction<LocalUserPhotoModel>();

        #endregion

        #region Commands

        public IMvxCommand CreatePhotoCommand { get; private set; }

        public IMvxCommand ChangeCameraCommand { get; private set; }

        public IMvxCommand GetImageFromLibraryCommand { get; private set; }

        #endregion

        #region Override methods

        public async override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.Black;

            //todo: added
            _isUserDataRetaked = (await _localDataService.GetUserDataAsync()).IsRetaked;
            if (_isUserDataRetaked)
            {
                //var currentNeedRetakePhoto = await _localDataService.GetNextNeedRetakePhotoAsync();
                //if (currentNeedRetakePhoto != null)
                //{
                //    UserPhoto = currentNeedRetakePhoto;
                //    return;
                //}

                UserPhoto = await _localDataService.GetNextNeedRetakePhotoAsync();

                //await _navigationService.Navigate<ReviewDataViewModel>();
                //await _navigationService.Close(this);
            }
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();

            LayoutIteraction.Raise(UserPhoto);

            if (IsFirstShowing)
            {
                IsFirstShowing = false;
                return;
            }

            //todo: added
            if (_isUserDataRetaked)
            {
                return;
            }

            using (_userDialogs.Loading())
            {
                var userPhoto = await _localDataService.GetNextPhotoAsync();
                if (userPhoto.PageNumber < UserPhoto.PageNumber)
                {
                    return;
                }
                if (UserPhoto.PageNumber < Constants.MaxPhotoCount - 1
                    || (UserPhoto.PageNumber == Constants.MaxPhotoCount - 1 && !userPhoto.IsFull))
                {
                    UserPhoto = userPhoto;
                    LayoutIteraction.Raise(UserPhoto);

                    return;
                }
                await _navigationService.Navigate<ReviewDataViewModel>();
            }

        }

        protected async override Task TryBackPage()
        {
            using (_userDialogs.Loading())
            {
                //todo: added
                if (_isUserDataRetaked)
                {
                    return;
                }

                if (UserPhoto.PageNumber > 0)
                {
                    await _localDataService.ClearPhotoAsync(UserPhoto.PageNumber);
                    await _navigationService.Navigate<ReviewPhotoViewModel>();
                    UserPhoto = await _localDataService.GetCurrentPhotoAsync();
                    LayoutIteraction.Raise(UserPhoto);
                    return;
                }
                if (UserPhoto.PageNumber == 0)
                {
                    await _localDataService.ClearPhotoAsync(UserPhoto.PageNumber);
                    //await base.TryBackPage();
                    await _navigationService.Navigate<PersonalDataViewModel>();
                    await base.TryBackPage();
                }
            }
        }

        #endregion

        #region Public Methods

        public async Task CreatePhotoAsync()
        {
            await _localDataService.SetPhotoAsync(UserPhoto.PageNumber, UserPhoto.Photo);
            //_userDialogs.HideLoading();
            await _navigationService.Navigate<ReviewPhotoViewModel>();
            if (_isUserDataRetaked)
            {
                //_userDialogs.Toast("close takr photo page");
                await base.TryBackPage();
            }
            _userDialogs.HideLoading();
        }

        public async Task ShowErrorMessageAsync(string errorText)
        {
            _userDialogs.HideLoading();
            await _userDialogs.AlertAsync(errorText);
        }

        #endregion

        #region Private methods

        private void ChangeCameraCommandAsync()
        {
            _userDialogs.Toast(Constants.ChangeCamera);
        }

        private async Task GetImageFromLibraryAsync()
        {
            var task = Mvx.IoCProvider.Resolve<IMvxPictureChooserTask>();
            try
            {
                task.ChoosePictureFromLibrary(1500, 90,
                stream =>
                {
                    SetImageFromLibrary(stream);
                },
                () => { });
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"TakePhotoViewModel.GetImageFromLibraryAsync() error: {ex.Message}");
                _userDialogs.Toast(Constants.ImageLibraryErrorText);
            }
        }

        private void SetImageFromLibrary(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                var imageBytes = memoryStream.ToArray();

                var resizedByteArrayImage = _imageResizeService.ResizeImage(imageBytes, UserPhoto.PageNumber);

                UserPhoto.Photo = resizedByteArrayImage;
                InvokeOnMainThread(async () =>
                {
                    LayoutIteraction.Raise(null);
                    await CreatePhotoAsync();
                });
            }
        }

        #endregion
    }
}
