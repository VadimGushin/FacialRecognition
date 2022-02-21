using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class UserAgreementPage : BaseContentPage<UserAgreementViewModel>
    {
        public UserAgreementPage()
        {
            InitializeComponent();
        }
    }
}