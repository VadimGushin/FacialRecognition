using System;

namespace Langate.FacialRecognition.Mobile.Extensions
{
    public static class ImageConverterExtension
    {
        public static string ToBase64(this byte[] bytes)
        {
            var base64Photo = Convert.ToBase64String(bytes);
            return base64Photo;
        }

        public static byte[] ToBytes(this string base64Photo)
        {
            var bytesPhoto = Convert.FromBase64String(base64Photo);
            return bytesPhoto;
        }
    }
}
