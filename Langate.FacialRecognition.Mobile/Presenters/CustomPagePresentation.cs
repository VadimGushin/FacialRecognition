using MvvmCross.Forms.Presenters.Attributes;
using System;

namespace Langate.FacialRecognition.Mobile.Presenters
{
    public class CustomPagePresentation : MvxPagePresentationAttribute
    {
        public CustomPagePresentation(Type viewModelType,
            bool wrapInNavigationPage, bool noHistory, string title)
        {
            HostViewModelType = viewModelType;
            WrapInNavigationPage = wrapInNavigationPage;
            NoHistory = noHistory;
            Title = title;
        }
    }
}
