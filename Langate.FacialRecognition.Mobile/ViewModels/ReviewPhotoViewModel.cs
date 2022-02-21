using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class ReviewPhotoViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;

        #endregion
        public ReviewPhotoViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService
            )
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;

            RetakePhotoCommand = new MvxAsyncCommand(async () => { await CloseReviewPageAsync(false); });

            NextCommand = new MvxAsyncCommand(async () => { await CloseReviewPageAsync(true); });
        }

        #region Properties

        private LocalUserPhotoModel _userPhoto;
        public LocalUserPhotoModel UserPhoto
        {
            get => _userPhoto;
            set => SetProperty(ref _userPhoto, value);
        }

        private bool _isTextVisible;
        public bool IsTextVisible
        {
            get => _isTextVisible;
            set => SetProperty(ref _isTextVisible, value);
        }

        private bool _isUserFaceVisible;
        public bool IsUserFaceVisible
        {
            get => _isUserFaceVisible;
            set => SetProperty(ref _isUserFaceVisible, value);
        }

        public LocalUserDataModel _userDataModel { get; set; }

        #endregion

        #region MvxIteractions

        public MvxInteraction<LocalReviewPhotoModel> UserPhotoIteraction { get; set; } = new MvxInteraction<LocalReviewPhotoModel>();

        #endregion

        #region Commands


        public IMvxCommand RetakePhotoCommand { get; private set; }

        public IMvxCommand NextCommand { get; private set; }

        #endregion

        #region Override Methods

        public async override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.White;

            await InitUserData();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        protected async override Task TryBackPage()
        {
            //todo:add back if retake
            if (_userDataModel.IsRetaked)
            {
                if (UserPhoto.PageNumber > 0)
                {
                    await _localDataService.ChangeRetakedPhotoStatusAsync(UserPhoto.PageNumber - 1, false);
                    await InitUserData();
                    return;
                }

                //todo:added
                await _navigationService.Navigate<PersonalDataViewModel>();

                await base.TryBackPage();
                return;
            }

            if (UserPhoto != null && UserPhoto.IsFull)
            {
                await _localDataService.ClearPhotoAsync(UserPhoto.PageNumber);
            }
            await base.TryBackPage();
        }

        #endregion

        #region Private Methods

        private async Task InitUserData()
        {
            //todo:added
            _userDataModel = await _localDataService.GetUserDataAsync();
            if (_userDataModel.IsRetaked)
            {
                UserPhoto = await _localDataService.GetNextNeedRetakePhotoAsync();
            }
            if (!_userDataModel.IsRetaked)
            {
                UserPhoto = await _localDataService.GetCurrentPhotoAsync();
            }

            IsValidForm = true;

            IsTextVisible = UserPhoto.PageNumber == 0 ? true : false;
            IsUserFaceVisible = !IsTextVisible;

            UserPhotoIteraction.Raise(new LocalReviewPhotoModel(UserPhoto.Photo, IsUserFaceVisible));
        }

        private async Task CloseReviewPageAsync(bool isNextCommand)
        {
            using (_userDialogs.Loading())
            {
                if (_userDataModel.IsRetaked)
                {
                    await CloseIfRetakedAsync(isNextCommand);
                }
                if (!_userDataModel.IsRetaked)
                {
                    await CloseAsync(isNextCommand);
                }
            }
        }

        private async Task CloseIfRetakedAsync(bool isNextCommand)
        {
            if (isNextCommand && UserPhoto.PageNumber == Constants.MaxPhotoCount - 1)
            {
                await _navigationService.Navigate<ReviewDataViewModel>();
                return;
            }
            if (isNextCommand)
            {
                await _localDataService.ChangeRetakedPhotoStatusAsync(UserPhoto.PageNumber, true);
                await InitUserData();
                return;
            }
            await _localDataService.ClearPhotoAsync(UserPhoto.PageNumber);
            await _navigationService.Navigate<TakePhotoViewModel>();
            await base.TryBackPage();
        }

        private async Task CloseAsync(bool isNextCommand)
        {
            if (!isNextCommand)
            {
                await _localDataService.ClearPhotoAsync(UserPhoto.PageNumber);
            }

            if (isNextCommand && UserPhoto.PageNumber == Constants.MaxPhotoCount - 1)
            {
                await _navigationService.Navigate<ReviewDataViewModel>();
                return;
            }
            await base.TryBackPage();
        }

        #endregion
    }
}
