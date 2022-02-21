using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.ViewModels.Base;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using System;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Views.Base
{
    public class BaseContentPage<TViewModel> : MvxContentPage<TViewModel> where TViewModel : MvxViewModel
    {
        private static MvxContentPage<TViewModel> _currentPage;

        public BaseContentPage()
        {
            this.BackgroundColor = Color.White;
            _currentPage = this;
        }

        #region Public Properties

        /// <summary>
        /// Set visibility of native NavigationBar
        /// </summary>
        public bool HasNativeNavBar
        {
            get { return (bool)GetValue(HasNativeNavBarProperty); }
            set { SetValue(HasNativeNavBarProperty, value); }
        }

        /// <summary>
        /// Set visibility of BackButton at native NavigationBar
        /// </summary>
        public bool HasNativeBackButton
        {
            get { return (bool)GetValue(HasNativeBackButtonProperty); }
            set { SetValue(HasNativeBackButtonProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static BindableProperty HasNativeNavBarProperty = BindableProperty.Create(
                                        propertyName: nameof(HasNativeNavBar),
                                        returnType: typeof(bool),
                                        declaringType: typeof(BaseContentPage<TViewModel>),
                                        defaultValue: true,
                                        propertyChanged: OnHasNativeNavBarPropertyChanged);

        public static BindableProperty HasNativeBackButtonProperty = BindableProperty.Create(
                                        propertyName: nameof(HasNativeBackButton),
                                        returnType: typeof(bool),
                                        declaringType: typeof(BaseContentPage<TViewModel>),
                                        defaultValue: true,
                                        propertyChanged: OnHasNativeBackButtonPropertyChanged);

        #endregion

        #region Override Methods

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            (_currentPage.ViewModel as BaseViewModel).BackCommand.Execute();
            return true;
        }

        #endregion

        #region Protected Methods

        protected void InputFocused(object sender, EventArgs args)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Content.LayoutTo(new Rectangle(0, -180, Content.Bounds.Width, Content.Bounds.Height));
            }
        }

        protected void InputUnfocused(object sender, EventArgs args)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Content.LayoutTo(new Rectangle(0, 0, Content.Bounds.Width, Content.Bounds.Height));
            }
        }

        #endregion

        #region Private Methods

        private static void OnHasNativeNavBarPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            NavigationPage.SetHasNavigationBar(bindable, (bool)newValue);
        }

        private static void OnHasNativeBackButtonPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            NavigationPage.SetHasBackButton(_currentPage, (bool)newValue);
        }

        #endregion
    }
}