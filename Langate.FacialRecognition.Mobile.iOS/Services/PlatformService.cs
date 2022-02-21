using Langate.FacialRecognition.Mobile.Services.Interfaces;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile.iOS.Services
{
    public class PlatformService : IPlatformService
    {
        public string GetDeviceId()
        {
            var deviceId = UIDevice.CurrentDevice.IdentifierForVendor.ToString();
            return deviceId;
        }

        public string GetPhoneNumber()
        {
            return string.Empty;
        }

        public string GetOSVersion()
        {
            return $"{DeviceInfo.Platform} {DeviceInfo.VersionString}";
        }
    }
}