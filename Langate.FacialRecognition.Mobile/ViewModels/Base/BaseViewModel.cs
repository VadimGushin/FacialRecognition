using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Models.Page;
using Langate.FacialRecognition.Mobile.Resources.Strings;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Binding.Extensions;
using MvvmCross.Commands;
using MvvmCross.Forms.Views;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.ViewModels.Base
{
    public abstract class BaseViewModel : MvxViewModel
    {
        #region Services
        protected readonly IMvxNavigationService _navigationService;
        protected readonly IUserDialogs _userDialogs;
        #endregion

        public BaseViewModel(IMvxNavigationService navigationService,
            IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;

            BackCommand = new MvxAsyncCommand(async () => await TryBackPage());
        }

        /// <summary>
        /// Gets the internationalized string at the given <paramref name="index"/>, which is the key of the resource.
        /// </summary>
        /// <param name="index">Index key of the string from the resources of internationalized strings.</param>
        public string this[string index] => Strings.ResourceManager.GetString(index);

        #region Properties

        /// <summary>
        /// Shows the page validation state
        /// </summary>       
        private bool _isValidForm;
        public bool IsValidForm
        {
            get => _isValidForm;
            set => SetProperty(ref _isValidForm, value);
        }

        /// <summary>
        /// Set style of native status bar
        /// </summary>
        private StatusBarStyle _statusBarStyle;
        public StatusBarStyle StatusBarStyle
        {
            get { return _statusBarStyle; }
            set { SetProperty(ref _statusBarStyle, value); }
        }

        /// <summary>
        /// Set color of native status bar
        /// </summary>
        private StatusBarColor _statusBarColor;
        public StatusBarColor StatusBarColor
        {
            get { return _statusBarColor; }
            set { SetProperty(ref _statusBarColor, value); }
        }

        /// <summary>
        /// Device internet connection state
        /// </summary>
        private InternetConnectionState _connectionState;
        public InternetConnectionState ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }

        #endregion

        #region Commands

        public virtual IMvxCommand BackCommand { get; private set; }

        #endregion

        #region VirtualMethods

        protected virtual async Task TryBackPage()
        {
            try
            {
                if (((NavigationPage)(Application.Current.MainPage)).Pages.Count() > 1)
                {
                    await _navigationService.Close(this);
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BaseViewModel.TryBackPage error: {ex.Message}");
                _userDialogs.Alert(Constants.CannotOpenPreviousPage);
            }

        }

        protected void TryCloseAllViewNodelsAsync(Type currentViewModelType)
        {
            try
            {
                var viewModels = ((NavigationPage)(Application.Current.MainPage)).Pages.Select(x => (x as MvxContentPage).ViewModel).ToList();

                for (int i = 0; i < viewModels.Count; i++)
                {
                    if (viewModels[i] != null && viewModels[i].GetType() != currentViewModelType)
                    {
                        _navigationService.Close(viewModels[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BaseViewModel.TryCloseAllViewNodelsAsync error: {ex.Message}");
            }
        }

        #endregion

        #region Override methods
        public override void ViewDisappearing()
        {
            StatusBarColor = StatusBarColor.Default;
            //StatusBarStyle = StatusBarStyle.Default;
            base.ViewDisappearing();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        #endregion

        #region Event Handlers

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                ConnectionState = InternetConnectionState.None;
                _userDialogs.Toast(Constants.InternetConnectionError);
                return;
            }
            ConnectionState = InternetConnectionState.Connected;
        }

        #endregion

        #region Protected Methods

        protected bool CheckConnectionStatus()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _userDialogs.Toast(Constants.InternetConnectionError);
                return false;
            }
            return true;
        }

        protected void SaveErrorMessage(string title, string token, string message)
        {
            //_userDialogs.Toast(message ?? Constants.SomethingWrongError);
            Debug.WriteLine($"{title}: token={token}; { message ?? "model is null"}");
        }

        #endregion
    }

    public abstract class BaseViewModel<TParameter, TResult> : MvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
        #region Services
        protected readonly IMvxNavigationService _navigationService;
        protected readonly IUserDialogs _userDialogs;
        #endregion

        protected BaseViewModel(IMvxNavigationService navigationService,
            IUserDialogs userDialogs)
        {
            _navigationService = navigationService;
            _userDialogs = userDialogs;

            BackCommand = new MvxAsyncCommand(async () => await TryBackPage());
        }

        /// <summary>
        /// Gets the internationalized string at the given <paramref name="index"/>, which is the key of the resource.
        /// </summary>
        /// <param name="index">Index key of the string from the resources of internationalized strings.</param>
        public string this[string index] => Strings.ResourceManager.GetString(index);

        #region Properties

        /// <summary>
        /// Shows the page validation state
        /// </summary>       
        private bool _isValidForm;
        public bool IsValidForm
        {
            get => _isValidForm;
            set => SetProperty(ref _isValidForm, value);
        }

        /// <summary>
        /// Set style of native status bar
        /// </summary>
        private StatusBarStyle _statusBarStyle;
        public StatusBarStyle StatusBarStyle
        {
            get { return _statusBarStyle; }
            set { SetProperty(ref _statusBarStyle, value); }
        }

        /// <summary>
        /// Set color of native status bar
        /// </summary>
        private StatusBarColor _statusBarColor;
        public StatusBarColor StatusBarColor
        {
            get { return _statusBarColor; }
            set { SetProperty(ref _statusBarColor, value); }
        }

        /// <summary>
        /// Device internet connection state
        /// </summary>
        private InternetConnectionState _connectionState;
        public InternetConnectionState ConnectionState
        {
            get { return _connectionState; }
            set { SetProperty(ref _connectionState, value); }
        }

        #endregion

        #region Commands

        public virtual IMvxCommand BackCommand { get; private set; }

        #endregion

        #region VirtualMethods

        protected virtual async Task TryBackPage()
        {
            await _navigationService.Close(this);
        }

        #endregion

        #region Override methods

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        public override void ViewDisappearing()
        {
            StatusBarColor = StatusBarColor.Default;
            //StatusBarStyle = StatusBarStyle.Default;
            base.ViewDisappearing();
        }

        #endregion

        #region Event Handlers

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
            {
                ConnectionState = InternetConnectionState.None;
                _userDialogs.Toast(Constants.InternetConnectionError);
                return;
            }
            ConnectionState = InternetConnectionState.Connected;
        }

        #endregion

        #region Protected Methods

        protected bool CheckConnectionStatus()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                _userDialogs.Toast(Constants.InternetConnectionError);
                return false;
            }
            return true;
        }

        protected void SaveErrorMessage(string title, string token, string message)
        {
            Debug.WriteLine($"{title}: token={token}; { message ?? "model is null"}");
        }

        #endregion
    }
}
