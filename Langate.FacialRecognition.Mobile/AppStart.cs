using Acr.UserDialogs;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.Mobile.ViewModels;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile
{
    public class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app,
            IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            Mvx.IoCProvider.Resolve<ILocalDataService>().ClearStorage();

            return NavigationService.Navigate<MainViewModel>();
        }
    }
}
