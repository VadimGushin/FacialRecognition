using System;
using System.Globalization;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Converters.ContinueSubmitions
{
    public class ItemBorderColorConverter : IValueConverter
    {
        private readonly Color _greenColor = (Color)(Application.Current.Resources["tcolor_b8"]);
        private readonly Color _grayColor = Color.FromHex("#e0e0e0");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? _greenColor : _grayColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value ? _greenColor : _grayColor);
        }
    }
}
