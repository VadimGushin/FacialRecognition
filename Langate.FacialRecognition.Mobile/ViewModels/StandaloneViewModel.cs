using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.ViewModels
{
    public class StandaloneViewModel : BaseViewModel
    {
        private readonly ILocalDataService _localDataService;

        public StandaloneViewModel(
            IMvxNavigationService navigationService,
            IUserDialogs userDialogs,
            ILocalDataService localDataService)
            : base(navigationService, userDialogs)
        {
            InitControls();
            _localDataService = localDataService;
            //ClearCommand = new MvxAsyncCommand(async () => _localDataService.ClearStorage());
        }

        #region Properties

        private string _mainText;
        public string MainText
        {
            get => _mainText;
            set => SetProperty(ref _mainText, value);
        }

        #endregion

        #region Commands

        //public IMvxCommand ClearCommand { get; private set; }

        #endregion

        #region Override Methods

        public override void ViewAppearing()
        {
            base.ViewAppearing();
            StatusBarColor = Models.Enums.StatusBarColor.Transparent;
        }

        protected async override Task TryBackPage()
        {
            //return base.TryBackPage();
        }

        #endregion

        #region Private Methods

        private void InitControls()
        {
            MainText = Resources.Strings.Strings.StandaloneText;
        }

        #endregion
    }
}
