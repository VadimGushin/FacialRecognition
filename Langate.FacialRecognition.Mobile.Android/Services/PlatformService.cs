using Android.Content;
using Android.Telephony;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Plugin.CurrentActivity;
using System;
using System.Diagnostics;
using Xamarin.Essentials;

namespace Langate.FacialRecognition.Mobile.Droid.Services
{
    public class PlatformService : IPlatformService
    {
        public string GetDeviceId()
        {
            var deviceId = Android.Provider.Settings.Secure.GetString(
                Android.App.Application.Context.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId);

            return deviceId;
        }

        public string GetPhoneNumber()
        {
            try
            {
                var context = CrossCurrentActivity.Current.AppContext;
                TelephonyManager telephonyManager = (TelephonyManager)context.GetSystemService(Context.TelephonyService);
                string phoneNumber = telephonyManager.Line1Number;
                return phoneNumber ?? string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error while getting phone number android: {ex.Message}");
            }
            return string.Empty;
        }

        public string GetOSVersion()
        {
            return $"{DeviceInfo.Platform} {DeviceInfo.VersionString}";
        }
    }
}