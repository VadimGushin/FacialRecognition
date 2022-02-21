using System;

namespace Langate.FacialRecognition.Mobile.Extensions
{
    public static class DateTimeFormatExtension
    {
        public static string TryFormat(this DateTime dateTime)
        {
            string day = dateTime.Day.ToString("00");
            string month = dateTime.Month.ToString("00");
            string year = dateTime.Year.ToString();

            //return $"{day}/{month}/{year}";
            return $"{month}/{day}/{year}";
        }
    }
}
