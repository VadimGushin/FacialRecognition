using Langate.FacialRecognition.Mobile.Models.Enums;

namespace Langate.FacialRecognition.Mobile.Models.Page
{
    public class PageStatusBarStyleModel
    {
        public StatusBarStyle StatusBarStyle { get; set; }
        public StatusBarColor StatusBarColor { get; set; }

        public PageStatusBarStyleModel(StatusBarStyle statusBarStyle, StatusBarColor statusBarColor)
        {
            StatusBarStyle = statusBarStyle;
            StatusBarColor = statusBarColor;
        }
    }
}
