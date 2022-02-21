using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.ViewModels;
using System.IO;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class ReviewPhotoPage : BaseContentPage<ReviewPhotoViewModel>
    {
        #region MvxIteractions

        private IMvxInteraction<LocalReviewPhotoModel> _photoImageIteraction;
        public IMvxInteraction<LocalReviewPhotoModel> PhotoImageIteraction
        {
            get => _photoImageIteraction;
            set
            {
                if (_photoImageIteraction != null)
                    _photoImageIteraction.Requested -= PhotoImageIteraction_Requested;

                _photoImageIteraction = value;
                _photoImageIteraction.Requested += PhotoImageIteraction_Requested; ;
            }
        }

        #endregion

        public ReviewPhotoPage()
        {
            InitializeComponent();
        }

        #region Overrides

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var set = this.CreateBindingSet<ReviewPhotoPage, ReviewPhotoViewModel>();
            set.Bind(this).For(v => v.PhotoImageIteraction).To(vm => vm.UserPhotoIteraction);
            set.Apply();
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    ViewModel.BackCommand.Execute();
        //    return true;
        //}

        #endregion

        #region Handlers

        private void PhotoImageIteraction_Requested(object sender, MvvmCross.Base.MvxValueEventArgs<LocalReviewPhotoModel> e)
        {
            if (e.Value == null || userPhoto == null)
            {
                return;
            }
            if (e.Value.IsFacePhoto)
            {
                userPhoto.Source = ImageSource.FromStream(() => new MemoryStream(e.Value.Photo));
            }
            if (!e.Value.IsFacePhoto)
            {
                userId.Source = ImageSource.FromStream(() => new MemoryStream(e.Value.Photo));
            }
        }

        #endregion
    }
}