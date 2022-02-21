using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Components
{
    public class CustomScrollView : ScrollView
    {
        #region Properties

        /// <summary>
        /// Set visibility for overviews of CustomScrollView
        /// </summary>
        public bool IsOverViewVisible
        {
            get { return (bool)GetValue(IsOverViewVisibleProperty); }
            set { SetValue(IsOverViewVisibleProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty IsOverViewVisibleProperty = BindableProperty.Create(
                                 propertyName: nameof(IsOverViewVisible),
                                 returnType: typeof(bool),
                                 declaringType: typeof(CustomScrollView),
                                 defaultValue: default);

        #endregion
    }
}
