using System.Collections;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Langate.FacialRecognition.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEditDropDownView : ContentView
    {
        public CustomEditDropDownView()
        {
            InitializeComponent();

            InitControls();
        }


        #region Properties

        /// <summary>
        /// Title text of the CustomEditDropDownView
        /// </summary>
        public string TitleText
        {
            get { return base.GetValue(TitleTextProperty).ToString(); }
            set { base.SetValue(TitleTextProperty, value); }
        }

        /// <summary>
        /// Items source of the CustomEditDatePickerView
        /// </summary>
        public IList Items
        {
            get { return (IList)base.GetValue(ItemsProperty); }
            set { base.SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Selected item of the CustomEditDatePickerView
        /// </summary>
        public string SelectedItem
        {
            get { return base.GetValue(SelectedItemProperty).ToString(); }
            set { base.SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Init value of the CustomEditDatePickerView
        /// </summary>
        public string InitValue
        {
            get { return base.GetValue(InitValueProperty).ToString(); }
            set { base.SetValue(InitValueProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                 propertyName: nameof(TitleText),
                                 returnType: typeof(string),
                                 declaringType: typeof(CustomEditDropDownView),
                                 defaultValue: default,
                                 defaultBindingMode: BindingMode.TwoWay,
                                 propertyChanged: TitleTextPropertyChanged);

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
                                 propertyName: nameof(Items),
                                 returnType: typeof(IList),
                                 declaringType: typeof(CustomEditDropDownView),
                                 defaultValue: default,
                                 defaultBindingMode: BindingMode.TwoWay,
                                 propertyChanged: ItemsPropertyChanged);

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
                                 propertyName: nameof(SelectedItem),
                                 returnType: typeof(string),
                                 declaringType: typeof(CustomEditDropDownView),
                                 defaultValue: default);

        public static readonly BindableProperty InitValueProperty = BindableProperty.Create(
                                propertyName: nameof(InitValue),
                                returnType: typeof(string),
                                declaringType: typeof(CustomEditDropDownView),
                                defaultValue: default,
                                defaultBindingMode: BindingMode.TwoWay,
                                propertyChanged: InitValuePropertyChanged);

        #endregion

        #region Property Handlers

        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditDropDownView)bindable;
            control.topLable.Text = newValue.ToString();
        }

        private static void ItemsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditDropDownView)bindable;
            control.dropDown.ItemsSource = (IList)newValue;
        }

        private static void InitValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditDropDownView)bindable;
            control.dropDown.SelectedItem = newValue.ToString();
            control.titleLabel.Text = newValue.ToString();
        }
        #endregion

        #region Private Methods

        private void InitControls()
        {
            this.titleLabel.IsVisible = true;
            //this.dropDownFrame.IsVisible = false;
            this.dropDown.IsVisible = false;

            this.dropDown.SelectedItemChanged += (s, e) =>
            {
                this.titleLabel.Text = this.dropDown.SelectedItem.ToString();
                SelectedItem = this.dropDown.SelectedItem.ToString();
            };

            this.button.Clicked += Button_Clicked;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            this.titleLabel.IsVisible = !this.titleLabel.IsVisible;
            //this.dropDownFrame.IsVisible = !this.dropDownFrame.IsVisible;
            this.dropDown.IsVisible = !this.dropDown.IsVisible;

            button.Source = !this.dropDown.IsVisible
                ? ImageSource.FromFile("img_edit.png")
                : ImageSource.FromFile("img_edit_complete.png");
        }

        #endregion
    }
}