using Langate.FacialRecognition.Mobile.Extensions;
using Langate.FacialRecognition.Mobile.Heplers;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Langate.FacialRecognition.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEditEntry : ContentView
    {
        public CustomEditEntry()
        {
            InitializeComponent();

            Init();
        }

        #region Properties

        /// <summary>
        /// Title text of the CustomEditEntry
        /// </summary>
        public string TitleText
        {
            get { return base.GetValue(TitleTextProperty).ToString(); }
            set { base.SetValue(TitleTextProperty, value); }
        }

        /// <summary>
        /// Main text of the CustomEditEntry
        /// </summary>
        public string MainValue
        {
            get { return base.GetValue(MainValueProperty).ToString(); }
            set { base.SetValue(MainValueProperty, value); }
        }

        /// <summary>
        /// Lenght for validation of the CustomEditEntry
        /// </summary>
        public int ValidValueLenght
        {
            get { return (int)base.GetValue(ValidValueLenghtProperty); }
            set { base.SetValue(ValidValueLenghtProperty, value); }
        }

        /// <summary>
        /// Indicates whether the value of the CustomEditEntry is a name
        /// </summary>
        public string EntryType
        {
            get { return base.GetValue(EntryTypeProperty).ToString(); }
            set { base.SetValue(EntryTypeProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
                                                 propertyName: nameof(TitleText),
                                                 returnType: typeof(string),
                                                 declaringType: typeof(CustomEditEntry),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: TitleTextPropertyChanged);

        public static readonly BindableProperty MainValueProperty = BindableProperty.Create(
                                                 propertyName: nameof(MainValue),
                                                 returnType: typeof(string),
                                                 declaringType: typeof(CustomEditEntry),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: MainValuePropertyChanged);

        public static readonly BindableProperty ValidValueLenghtProperty = BindableProperty.Create(
                                                 propertyName: nameof(ValidValueLenght),
                                                 returnType: typeof(int),
                                                 declaringType: typeof(CustomEditEntry),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: ValidValueLenghtPropertyChanged);

        public static readonly BindableProperty EntryTypeProperty = BindableProperty.Create(
                                                 propertyName: nameof(EntryType),
                                                 returnType: typeof(string),
                                                 declaringType: typeof(CustomEditEntry),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: EntryTypePropertyChanged);

        #endregion

        #region Property Handlers

        private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditEntry)bindable;
            control.topLable.Text = newValue.ToString();
        }

        private static void MainValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditEntry)bindable;
            control.titleLabel.Text = newValue.ToString();
            control.textEntry.Text = newValue.ToString();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            this.titleLabel.IsVisible = !this.titleLabel.IsVisible;
            this.textEntry.IsVisible = !this.textEntry.IsVisible;

            button.Source = !this.textEntry.IsVisible ? ImageSource.FromFile("img_edit.png") : ImageSource.FromFile("img_edit_complete.png");
        }

        private static void ValidValueLenghtPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditEntry)bindable;
            control.textEntry.ValidTextLenght = (int)newValue;
            control.textEntry.MaxLength = (int)newValue;
        }

        private static void EntryTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomEditEntry)bindable;
            var entryValueType = newValue.ToString();
            if (entryValueType.Equals(Constants.EntryNumberValueKey))
            {
                control.textEntry.Keyboard = Keyboard.Numeric;
            }
            control.textEntry.EntryValueType = entryValueType;
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            this.titleLabel.IsVisible = true;
            this.textEntry.IsVisible = false;

            this.button.IsEnabled = true;

            this.textEntry.TextChanged += (s, e) =>
            {
                this.button.IsEnabled = CheckIsButtonEnabled();
                MainValue = this.textEntry.Text;
                this.titleLabel.Text = this.textEntry.Text;
            };

            this.button.Clicked += Button_Clicked;
        }

        private bool CheckIsButtonEnabled()
        {
            var validationPattern = EntryType.GetValidatiomPattern();

            if (this.textEntry.Text.Length != ValidValueLenght || !Regex.IsMatch(this.textEntry.Text, validationPattern))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}