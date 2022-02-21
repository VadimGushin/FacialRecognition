using Langate.FacialRecognition.Mobile.ViewModels;
using Langate.FacialRecognition.Mobile.Views.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.ViewModels;

namespace Langate.FacialRecognition.Mobile.Views
{
    [MvxContentPagePresentation(WrapInNavigationPage = true, NoHistory = false)]
    public partial class ReviewDataPage : BaseContentPage<ReviewDataViewModel>
    {
        public ReviewDataPage()
        {
            InitializeComponent();
        }

        #region MvxIteractions

        private IMvxInteraction<bool> _scrollIteraction;
        public IMvxInteraction<bool> ScrollIteraction
        {
            get => _scrollIteraction;
            set
            {
                if (_scrollIteraction != null)
                    _scrollIteraction.Requested -= ScrollIteraction_Requested;

                _scrollIteraction = value;
                _scrollIteraction.Requested += ScrollIteraction_Requested;
            }
        }

        #endregion

        #region Overriedes

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();

            var set = this.CreateBindingSet<ReviewDataPage, ReviewDataViewModel>();
            set.Bind(this).For(v => v.ScrollIteraction).To(vm => vm.ScrollIteraction);
            set.Apply();
        }

        #endregion

        #region Handlers

        private void ScrollIteraction_Requested(object sender, MvvmCross.Base.MvxValueEventArgs<bool> e)
        {
            if (e.Value)
            {
                this.scrollView.ScrollToAsync(0, 0, true);
            }
        }

        #endregion
    }
}