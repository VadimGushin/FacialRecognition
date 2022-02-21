using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Extensions;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using Langate.FacialRecognition.MobileApi.Model;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class PersonalDataViewModel : BaseViewModel
    {
        #region Services

        private readonly ILocalDataService _localDataService;

        #endregion

        public PersonalDataViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService)
            : base(navigationService, userDialogs)
        {
            _localDataService = localDataService;

            NextCommand = new MvxAsyncCommand(async () => { await SaveUserDataAsync(); });
            InitControls();
        }

        #region Properties

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

        private string _middleInitial;
        public string MiddleInitial
        {
            get => _middleInitial;
            set
            {
                SetProperty(ref _middleInitial, value);
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

        private int _lastNameValidLenght;
        public int LastNameValidLenght
        {
            get => _lastNameValidLenght;
            set => SetProperty(ref _lastNameValidLenght, value);
        }

        private int _firstNameValidLenght;
        public int FirstNameValidLenght
        {
            get => _firstNameValidLenght;
            set => SetProperty(ref _firstNameValidLenght, value);
        }

        private int _middleNameValidLenght;
        public int MiddleNameValidLenght
        {
            get => _middleNameValidLenght;
            set => SetProperty(ref _middleNameValidLenght, value);
        }

        private int _validIdValidLenght;
        public int ValidIdValidLenght
        {
            get => _validIdValidLenght;
            set => SetProperty(ref _validIdValidLenght, value);
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                ValidateForm();
            }
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
                ValidateForm();
            }
        }

        private MvxObservableCollection<string> _idTypes;
        public MvxObservableCollection<string> IdTypes
        {
            get => _idTypes;
            set => SetProperty(ref _idTypes, value);
        }

        private string _selectedIdType;
        public string SelectedIdType
        {
            get => _selectedIdType;
            set
            {
                SetProperty(ref _selectedIdType, value);
                ValidateForm();
            }
        }

        private string _validId;
        public string ValidId
        {
            get => _validId;
            set
            {
                SetProperty(ref _validId, value);
                ValidateForm();
            }
        }

        private bool _isUserDataRetaked { get; set; }

        #endregion

        #region Commands

        public IMvxCommand NextCommand { get; private set; }

        #endregion

        #region Override methods

        public async override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.White;
            await TryGetLocalData();
        }

        protected async override Task TryBackPage()
        {
            if (_isUserDataRetaked)
            {
                return;
            }
            await base.TryBackPage();
        }

        #endregion

        #region Private methods

        private void InitControls()
        {
            FirstNameValidLenght = 3;
            MiddleNameValidLenght = 1;
            LastNameValidLenght = 3;
            ValidIdValidLenght = 5;

            Genders = new MvxObservableCollection<string>(_localDataService.GetInviteModelAsync().Result.Gender.Values);
            IdTypes = new MvxObservableCollection<string>(_localDataService.GetInviteModelAsync().Result.IdType.Values);

            SelectedGender = string.Empty;
            SelectedIdType = string.Empty;

            ValidateForm();
        }

        private void ValidateForm()
        {
            if (FirstName.IsValid(Constants.EntryFirstLastNameValueKey, FirstNameValidLenght)
               && LastName.IsValid(Constants.EntryFirstLastNameValueKey, LastNameValidLenght)
               && MiddleInitial.IsValid(Constants.EntryMiddleNameValueKey, MiddleNameValidLenght)
               && SelectedDate.Year > Constants.MinValidYear && SelectedDate < DateTime.Now
               && !string.IsNullOrWhiteSpace(SelectedIdType)
               && !string.IsNullOrWhiteSpace(SelectedGender)
               && ValidId.IsValid(Constants.EntryIdValueKey, ValidIdValidLenght))
            {
                IsValidForm = true;
                return;
            }
            IsValidForm = false;
        }

        private async Task SaveUserDataAsync()
        {
            using (_userDialogs.Loading())
            {
                var userModel = new LocalUserDataModel();
                userModel.FirstName = FirstName;
                userModel.MiddleName = MiddleInitial;
                userModel.LastName = LastName;
                userModel.Gender = SelectedGender;
                userModel.IdType = (await _localDataService.GetInviteModelAsync()).IdType.FirstOrDefault(x => x.Value == SelectedIdType).Value;
                userModel.IdValue = ValidId;
                userModel.DateOfBirth = SelectedDate;
                userModel.IsFull = true;
                userModel.IsRetaked = _isUserDataRetaked;

                await _localDataService.SetUserDataAsync(userModel);

                if (!_isUserDataRetaked)
                {
                    await _navigationService.Navigate<TakePhotoViewModel>();
                }
                if (_isUserDataRetaked)
                {
                    await _navigationService.Navigate<ReviewPhotoViewModel>();
                }

                await base.TryBackPage();
            }
        }

        private async Task TryGetLocalData()
        {
            using (_userDialogs.Loading())
            {
                var localUserData = await _localDataService.GetUserDataAsync();
                if (localUserData != null
                    && (localUserData.IsFull || localUserData.IsRetaked))
                {
                    FirstName = localUserData.FirstName;
                    MiddleInitial = localUserData.MiddleName;
                    LastName = localUserData.LastName;
                    SelectedGender = localUserData.Gender;
                    SelectedIdType = localUserData.IdType;
                    ValidId = localUserData.IdValue;
                    SelectedDate = localUserData.DateOfBirth;
                    _isUserDataRetaked = localUserData.IsRetaked;
                }
            }
        }

        #endregion
    }
}
