using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Langate.FacialRecognition.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationBar : ContentView
    {
        public CustomNavigationBar()
        {
            InitializeComponent();

            backButton.Clicked += Button_Clicked;
        }

        #region Properties

        /// <summary>
        /// Title of the CustomNavigationBar
        /// </summary>
        public string Title
        {
            get { return base.GetValue(TitleProperty).ToString(); }
            set { base.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Image source of the CustomNavigationBar
        /// </summary>
        public string ImgSource
        {
            get { return base.GetValue(ImageSourceProperty).ToString(); }
            set { base.SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Background color of the CustomNavigationBar
        /// </summary>
        public Color BackgroundViewColor
        {
            get { return (Color)base.GetValue(BackgroundViewColorProperty); }
            set { base.SetValue(BackgroundViewColorProperty, value); }
        }

        /// <summary>
        /// Indicates whether the Back button is visible at CustomEditEntry
        /// </summary>
        public bool IsBackButtonVisible
        {
            get { return (bool)base.GetValue(IsBackButtonVisibleProperty); }
            set { base.SetValue(IsBackButtonVisibleProperty, value); }
        }

        #endregion

        #region Commands

        /// <summary>
        /// Button click command of CustomNavigationBar
        /// </summary>
        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
                                                 propertyName: nameof(Title),
                                                 returnType: typeof(string),
                                                 declaringType: typeof(CustomNavigationBar),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: TitlePropertyChanged);

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
                                                  propertyName: nameof(ImgSource),
                                                  returnType: typeof(string),
                                                  declaringType: typeof(CustomNavigationBar),
                                                  defaultValue: string.Empty,
                                                  defaultBindingMode: BindingMode.TwoWay,
                                                  propertyChanged: ImageSourcePropertyChanged);

        public static readonly BindableProperty BackgroundViewColorProperty = BindableProperty.Create(
                                                  propertyName: nameof(BackgroundViewColor),
                                                  returnType: typeof(Color),
                                                  declaringType: typeof(CustomNavigationBar),
                                                  defaultValue: Color.Default,
                                                  defaultBindingMode: BindingMode.TwoWay,
                                                  propertyChanged: BackgroundViewColorPropertyChanged);

        public static readonly BindableProperty ClickCommandProperty =
                                                BindableProperty.Create(
                                                propertyName: nameof(ClickCommand),
                                                returnType: typeof(ICommand),
                                                declaringType: typeof(CustomNavigationBar),
                                                defaultValue: null);

        public static readonly BindableProperty IsBackButtonVisibleProperty = BindableProperty.Create(
                                                propertyName: nameof(IsBackButtonVisible),
                                                returnType: typeof(bool),
                                                declaringType: typeof(CustomNavigationBar),
                                                defaultValue: true,
                                                defaultBindingMode: BindingMode.TwoWay,
                                                propertyChanged: IsBackButtonVisiblePropertyChanged);

        #endregion

        #region Property Handlers

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomNavigationBar)bindable;
            control.titleText.Text = newValue.ToString();
            control.titleText.IsVisible = string.IsNullOrWhiteSpace(control.titleText.Text) ? false : true;
            control.logoImage.IsVisible = !(control.titleText.IsVisible);
        }

        private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomNavigationBar)bindable;
            //control.backButton.Source = ImageSource.FromResource($"{CommonConstants.ImageResourcesPath}{newValue.ToString()}");
            control.backButton.Source = (string)newValue;
        }

        private static void BackgroundViewColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomNavigationBar)bindable;
            control.parentLayout.BackgroundColor = (Color)newValue;
        }

        private static void IsBackButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomNavigationBar)bindable;
            control.backButton.IsVisible = (bool)newValue;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ClickCommand?.Execute(null);
        }

        #endregion

    }
}