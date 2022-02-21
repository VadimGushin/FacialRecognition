using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Views.Base;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Langate.FacialRecognition.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContinueSubmissionPage : BaseContentPage<ContinueSubmissionViewModel>
    {
        public ContinueSubmissionPage()
        {
            InitializeComponent();
        }

        public void OnItemTapped(object sender, EventArgs e)
        {
            var token = ((TappedEventArgs)e).Parameter.ToString();
            ViewModel.OnSelected(token);
        }
    }
}