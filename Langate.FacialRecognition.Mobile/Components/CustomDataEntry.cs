using System.Windows.Input;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Components
{
    public class CustomDataEntry : Entry
    {
        #region Properties

        /// <summary>
        /// Valid lenght for value of CustomDataEntry
        /// </summary>
        public int ValidTextLenght
        {
            get { return (int)base.GetValue(ValidTextLenghtProperty); }
            set { base.SetValue(ValidTextLenghtProperty, value); }
        }

        /// <summary>
        /// Indicates the type of value for CustomDataEntry
        /// </summary>
        public string EntryValueType
        {
            get { return base.GetValue(EntryValueTypeProperty).ToString(); }
            set { base.SetValue(EntryValueTypeProperty, value); }
        }

        #endregion

        #region Commands

        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(
                                                propertyName: nameof(TextChangedCommand),
                                                returnType: typeof(ICommand),
                                                declaringType: typeof(CustomDataEntry),
                                                defaultValue: null);

        public static readonly BindableProperty ValidTextLenghtProperty = BindableProperty.Create(
                                                propertyName: nameof(ValidTextLenghtProperty),
                                                returnType: typeof(int),
                                                declaringType: typeof(CustomDataEntry),
                                                defaultValue: 0);

        public static readonly BindableProperty EntryValueTypeProperty = BindableProperty.Create(
                                                propertyName: nameof(EntryValueType),
                                                returnType: typeof(string),
                                                declaringType: typeof(CustomDataEntry),
                                                defaultValue: default);

        #endregion
    }
}
