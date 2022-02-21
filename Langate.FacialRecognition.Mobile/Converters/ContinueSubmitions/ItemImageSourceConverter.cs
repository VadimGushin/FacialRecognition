using System;
using System.Globalization;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.Converters.ContinueSubmitions
{
    public class ItemImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? "img_selectedItem.png" : "img_unselectedItem.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value ? "img_selectedItem.png" : "img_unselectedItem.png");
        }
    }
}
