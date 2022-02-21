using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Forms.Presenters.Attributes;

namespace Langate.FacialRecognition.Mobile.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class PersonalDataPage : BaseContentPage<PersonalDataViewModel>
    {
        public PersonalDataPage()
        {
            InitializeComponent();
        }

        #region Override Methods

        #endregion
    }
}