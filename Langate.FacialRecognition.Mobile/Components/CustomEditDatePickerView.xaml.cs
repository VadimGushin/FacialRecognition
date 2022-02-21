using Langate.FacialRecognition.Mobile.Extensions;
using Langate.FacialRecognition.Mobile.Models.Local;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Langate.FacialRecognition.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEditDatePickerView : ContentView
    {
        public CustomEditDatePickerView()
        {
            InitializeComponent();

            InitControls();
        }

        #region Properties

        /// <summary>
        /// Title text of the CustomEditDatePickerView
        /// </summary>
        public string TitleText
        {
            get { return base.GetValue(TitleTextProperty).ToString(); }
            set { base.SetValue(TitleTextProperty, value); }
        }

        /// <summary>
        /// Selected date of the CustomEditDatePickerView
        /// </summary>
        public DateTime DateSelected
        {
            get { return (DateTime)base.GetValue(DateSelectedProperty); }
            set { base.SetValue(DateSelectedProperty, value); }
        }

        #endregion

        #region BindableProperties

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                 propertyName: nameof(TitleText),
                                                 returnType: typeof(string),
                                                 declaringType: typeof(CustomEditDatePickerView),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: TitleTextPropertyChanged);

        public static readonly BindableProperty DateSelectedProperty = BindableProperty.Create(
                                                 propertyName: nameof(DateSelected),
                                                 returnType: typeof(DateTime),
                                                 declaringType: typeof(CustomEditDatePickerView),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: DateSelectedPropertyChanged);

        #endregion

        #region Property Handlers

        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditDatePickerView)bindable;
            control.topLable.Text = newValue.ToString();
        }

        private static void DateSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditDatePickerView)bindable;
            control.datePicker.SelectedDOB = (DateTime)newValue;
            control.titleLabel.Text = ((DateTime)newValue).TryFormat();
        }

        #endregion

        #region Private Methods

        private void InitControls()
        {
            this.titleLabel.IsVisible = true;
            this.datePicker.IsVisible = false;

            this.datePicker.SelectedDateChanged += (s, e) =>
            {
                if (e > DateTime.MinValue)
                {
                    this.titleLabel.Text = this.datePicker.SelectedDOB.TryFormat();
                    this.DateSelected = e;
                }
            };

            this.button.Clicked += Button_Clicked;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            this.titleLabel.IsVisible = !this.titleLabel.IsVisible;
            this.datePicker.IsVisible = !this.datePicker.IsVisible;

            button.Source = !this.datePicker.IsVisible
                ? ImageSource.FromFile("img_edit.png")
                : ImageSource.FromFile("img_edit_complete.png");
        }

        #endregion
    }
}