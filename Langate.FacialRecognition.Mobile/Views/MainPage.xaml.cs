using Langate.FacialRecognition.Mobile.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace Langate.FacialRecognition.Mobile.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = false, NoHistory = true)]
    public partial class MainPage : MvxContentPage<MainViewModel>, IMvxOverridePresentationAttribute
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            if (request.PresentationValues != null)
            {
                if (request.PresentationValues.ContainsKey("NavigationMode") &&
                    request.PresentationValues["NavigationMode"] == "Modal")
                {
                    return new MvxModalPresentationAttribute
                    {
                        WrapInNavigationPage = true,
                        NoHistory = true
                    };
                }
            }

            return null;
        }

        protected override void OnAppearing()
        {
            ViewModel.ShowFirstViewModelCommand.Execute(null);

            base.OnAppearing();
        }
    }
}