using System;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Components
{
    public class CustomDatePicker : DatePicker
    {
        #region Properties

        /// <summary>
        /// Image source for right image of CustomPicker
        /// </summary>
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        /// <summary>
        /// Min dateTime value of CustomPicker
        /// </summary>
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        /// <summary>
        /// Max dateTime value of CustomPicker
        /// </summary>
        public DateTime EndDate
        {
            get { return (DateTime)GetValue(EndDateProperty); }
            set { SetValue(EndDateProperty, value); }
        }

        /// <summary>
        /// Shows if there is a date value of CustomPicker
        /// </summary>
        public bool HaveInitDate
        {
            get { return (bool)GetValue(HaveInitDateProperty); }
            set { SetValue(HaveInitDateProperty, value); }
        }

        /// <summary>
        /// Init dateTime value of CustomPicker
        /// </summary>
        public DateTime InitDate
        {
            get { return (DateTime)GetValue(InitDateProperty); }
            set { SetValue(InitDateProperty, value); }
        }

        #endregion

        #region BindableProperties

        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
                                                 propertyName: nameof(Image),
                                                 returnType: typeof(string),
                                                 declaringType: typeof(CustomDatePicker),
                                                 defaultValue: string.Empty);

        public static readonly BindableProperty StartDateProperty = BindableProperty.Create(
                                                 propertyName: nameof(StartDate),
                                                 returnType: typeof(DateTime),
                                                 declaringType: typeof(CustomDatePicker),
                                                 defaultValue: default);

        public static readonly BindableProperty EndDateProperty = BindableProperty.Create(
                                                 propertyName: nameof(EndDate),
                                                 returnType: typeof(DateTime),
                                                 declaringType: typeof(CustomDatePicker),
                                                 defaultValue: default);

        public static readonly BindableProperty HaveInitDateProperty = BindableProperty.Create(
                                                 propertyName: nameof(HaveInitDate),
                                                 returnType: typeof(bool),
                                                 declaringType: typeof(CustomDatePicker),
                                                 defaultValue: false);

        public static readonly BindableProperty InitDateProperty = BindableProperty.Create(
                                                 propertyName: nameof(InitDate),
                                                 returnType: typeof(DateTime),
                                                 declaringType: typeof(CustomDatePicker),
                                                 defaultValue: default);

        #endregion
    }
}
