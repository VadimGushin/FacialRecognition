using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Localization = Langate.FacialRecognition.Mobile.Resources.Strings.Strings;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class ContinueSubmissionViewModel : BaseViewModel
    {
        #region Services

        private readonly IUserService _userService;
        private readonly ILocalDataService _localDataService;

        #endregion
        public ContinueSubmissionViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            IUserService userService,
            ILocalDataService localDataService)
            : base(navigationService, userDialogs)
        {
            _userService = userService;
            _localDataService = localDataService;

            NextCommand = new MvxAsyncCommand(ShowNextPageAsync);
            Title = Localization.ContinueWith;
        }

        #region Properties

        private MvxObservableCollection<LocalContinueSubmissionItemModel> _items;
        public MvxObservableCollection<LocalContinueSubmissionItemModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion

        #region Commands

        public IMvxCommand NextCommand { get; private set; }

        #endregion

        #region Overrides

        public async override void ViewAppearing()
        {
            IsValidForm = true;
            StatusBarColor = Models.Enums.StatusBarColor.White;
            base.ViewAppearing();
            await InitCollection();
        }

        protected async override Task TryBackPage()
        {
        }

        #endregion

        #region Private Methods

        private async Task InitCollection()
        {
            Items = new MvxObservableCollection<LocalContinueSubmissionItemModel>();

            _userDialogs.ShowLoading();
            var result = await _userService.GetValidTokensAsync();
            if ( result == null 
                || !result.Tokens.Any()
                || result.Tokens.Count == 1)
            {
                await base.TryBackPage();
                _userDialogs.HideLoading();
                return;
            }
            if (result != null && result.Tokens.Any())
            {
                Items.AddRange(result.Tokens.OrderBy(item => !item.Value.Equals(_localDataService.Token)).Select(item => new LocalContinueSubmissionItemModel()
                {
                    Token = item.Value,
                    Title = _localDataService.Token == item.Value ? Localization.ContinueSubmissionTo : Localization.StartSubmissionTo,
                    Location = $"{Localization.At} {item.Location}",
                    Study = item.Study,
                    IsCurrent = _localDataService.Token == item.Value ? true : false,
                    IsSelected = _localDataService.Token == item.Value ? true : false
                }));
            }
            _userDialogs.HideLoading();
        }

        private async Task ShowNextPageAsync()
        {
            var selectedItem = Items.Where(item => item.IsSelected).FirstOrDefault();
            if (selectedItem == null)
            {
                return;
            }
            if (selectedItem.IsCurrent 
                && (await _localDataService.GetInviteModelAsync()).InviteId > 0)
            {
                await base.TryBackPage();
                return;
            }

            _localDataService.ClearStorage();
            await _localDataService.InitDataAsync();
            _localDataService.Token = selectedItem.Token;

            await _navigationService.Navigate<WelcomeViewModel>();
            TryCloseAllViewNodelsAsync(typeof(WelcomeViewModel));
        }

        #endregion

        #region Public Methods

        public void OnSelected(string token)
        {
            var item = Items.Where(x => x.Token.Equals(token)).FirstOrDefault();
            if (item == null)
            {
                IsValidForm = false;
                return;
            }
            for (int i = 0; i < Items.Count; i++)
            {
                if (!Items[i].IsSelected && Items[i].Token != item.Token)
                {
                    continue;
                }
                Items[i] = new LocalContinueSubmissionItemModel()
                {
                    IsSelected = item.Token == Items[i].Token ? true : false,
                    Token = Items[i].Token,
                    Title = Items[i].Title,
                    Location = Items[i].Location,
                    IsCurrent = Items[i].IsCurrent,
                    Study = Items[i].Study
                };
            }
        }

        #endregion
    }
}
