using Plugin.InputKit.Shared.Controls;
using System;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Components
{
    public class CustomDropDownView : Dropdown
    {

        #region Enents

        public event EventHandler OnMenuShowed;

        #endregion

        public CustomDropDownView() : base()
        {
            Init();
        }

        #region Properties

        /// <summary>
        /// Height of text entry for CustomDropDownView
        /// </summary>
        public int TextHeight
        {
            get { return (int)base.GetValue(ViewHeightProperty); }
            set { base.SetValue(ViewHeightProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty ViewHeightProperty = BindableProperty.Create(
                                                 propertyName: nameof(TextHeight),
                                                 returnType: typeof(int),
                                                 declaringType: typeof(CustomDropDownView),
                                                 defaultValue: 0,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: TextHeightPropertyChanged);

        #endregion


        #region Property Handlers

        private static void TextHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomDropDownView)bindable;
            control.txtInput.HeightRequest = (int)newValue;
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            this.Spacing = 0;
            this.txtInput.VerticalTextAlignment = TextAlignment.Center;
            this.txtInput.VerticalOptions = LayoutOptions.Center;
            this.imgIcon.Margin = 0;
            this.PlaceholderColor = Color.FromRgb(165, 165, 165);

            this.frmBackground.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(OnMenuOpened),
                    CommandParameter = frmBackground
                });
        }

        private void OnMenuOpened(object sender)
        {
            OnMenuShowed?.Invoke(sender, null);
        }

        #endregion

        #region Public methodds

        public void SetPlaceholderText(string text)
        {
            this.txtInput.Placeholder = text;
        }

        #endregion
    }
}
