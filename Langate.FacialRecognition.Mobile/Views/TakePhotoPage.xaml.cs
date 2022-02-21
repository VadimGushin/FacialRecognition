using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.ViewModels;
using System;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class TakePhotoPage : BaseContentPage<TakePhotoViewModel>
    {
        #region MvxIteractions

        private IMvxInteraction<LocalUserPhotoModel> _layoutIteraction;
        public IMvxInteraction<LocalUserPhotoModel> LayoutIteraction
        {
            get => _layoutIteraction;
            set
            {
                if (_layoutIteraction != null)
                    _layoutIteraction.Requested -= LayoutIteraction_Requested;

                _layoutIteraction = value;
                _layoutIteraction.Requested += LayoutIteraction_Requested;
            }
        }

        #endregion

        public event EventHandler<int> ChangeViewPresentation;

        public TakePhotoPage()
        {
            InitializeComponent();

            if (true)
            {

            }
        }

        #region Overrides

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var set = this.CreateBindingSet<TakePhotoPage, TakePhotoViewModel>();
            set.Bind(this).For(v => v.LayoutIteraction).To(vm => vm.LayoutIteraction);
            set.Apply();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ChangeViewPresentation?.Invoke(null, -1);
        }

        #endregion

        #region Handlers

        private void LayoutIteraction_Requested(object sender, MvvmCross.Base.MvxValueEventArgs<LocalUserPhotoModel> e)
        {
            if (e.Value == null)
            {
                ChangeViewPresentation?.Invoke(1, 0);
                return;
            }
            int relativeLayoutValue = Constants.OneLineNavigationBarHeight;
            if (e.Value.PageNumber == 0)
            {
                relativeLayoutValue = Constants.OneLineNavigationBarHeight;
            }
            if (e.Value.PageNumber == 1)
            {
                relativeLayoutValue = Constants.TwoLineNavigationBarHeight;
            }
            if (e.Value.PageNumber >= 2 )
            {
                relativeLayoutValue = Constants.ThreeLineNavigationBarHeight;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                RelativeLayout.SetHeightConstraint(navigationBar, Constraint.RelativeToParent(layout => relativeLayoutValue));
                RelativeLayout.SetYConstraint(mainGrid, Constraint.RelativeToParent(layout => relativeLayoutValue));
                RelativeLayout.SetHeightConstraint(mainGrid, Constraint.RelativeToParent(layout => layout.Height - relativeLayoutValue));
            });

            this.bottomLayout.IsVisible = true;
            ChangeViewPresentation?.Invoke(null, e.Value.PageNumber);
        }

        #endregion

        #region Public methods

        //public Image GetCameraView()
        //{
        //    return Camera;
        //}

        public ImageButton GetCameraModeButton()
        {
            return btnCameraMode;
        }

        public ImageButton GetPhotoCreateButton()
        {
            return btnCreate;
        }

        public int GetCurrentPhotoNumber()
        {
            return ViewModel.UserPhoto.PageNumber;
        }

        #endregion
    }
}