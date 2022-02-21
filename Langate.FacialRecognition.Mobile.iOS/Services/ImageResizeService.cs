using System;
using CoreGraphics;
using Foundation;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using UIKit;

namespace Langate.FacialRecognition.Mobile.iOS.Services
{
    public class ImageResizeService : IImageResizeService
    {

        #region Publick Methods

        public byte[] ResizeImage(byte[] image, int pageNumber)
        {
            var data = NSData.FromArray(image);
            var uiImage = UIImage.LoadFromData(data);
            if (uiImage == null)
            {
                return null;
            }

            var resizedImage = ResizeImage(uiImage);
            if (resizedImage == null)
            {
                return null;
            }

            return GetByteArrayFromUIImage(resizedImage);
        }

        public byte[] ResizeImage(object image, int pageNumber, CameraType cameraType, int previewHeightChange)
        {
            var uiImage = image as UIImage;
            if (uiImage == null)
            {
                return null;
            }

            var resizedImage = ResizeAndRotateImage(uiImage, pageNumber, cameraType);
            if (resizedImage == null)
            {
                return null;
            }

            return GetByteArrayFromUIImage(resizedImage);
        }

        #endregion

        #region Private Methods

        private UIImage ResizeAndRotateImage(UIImage sourceImage, int pageNumber, CameraType cameraType = CameraType.Default)
        {
            sourceImage = CropImage(sourceImage, pageNumber, cameraType);

            var orientation = sourceImage.Orientation;
            nfloat angle = 0;

            if (orientation == UIImageOrientation.Up || orientation == UIImageOrientation.UpMirrored)
            {
                return sourceImage;
            }
            if (orientation == UIImageOrientation.Right || orientation == UIImageOrientation.RightMirrored)
            {
                angle = 90;
            }
            if (orientation == UIImageOrientation.Down || orientation == UIImageOrientation.DownMirrored)
            {
                angle = 180;
            }
            if (orientation == UIImageOrientation.Left || orientation == UIImageOrientation.LeftMirrored)
            {
                angle = 270;
            }

            nfloat radians = GetRadians(angle);
            var transform = CGAffineTransform.MakeRotation(radians);

            var rotatedViewBox = new UIView(new CGRect(0, 0, sourceImage.Size.Width, sourceImage.Size.Height))
            {
                Transform = transform
            };
            CGSize rotatedSize = rotatedViewBox.Frame.Size;

            UIGraphics.BeginImageContext(sourceImage.Size);
            CGContext context = UIGraphics.GetCurrentContext();
            context.TranslateCTM(sourceImage.Size.Width / 2.0f, sourceImage.Size.Height / 2.0f);
            context.RotateCTM(radians);
            context.ScaleCTM(1.0f, -1.0f);
            context.TranslateCTM(-rotatedSize.Width / 2.0f, -rotatedSize.Height / 2.0f);
            context.DrawImage(new CGRect(0, 0, rotatedSize.Width, rotatedSize.Height), sourceImage.CGImage);
            var responseImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return responseImage;
        }

        private UIImage CropImage(UIImage sourceImage, int pageNumber, CameraType cameraType)
        {
            nfloat yValue = 0;

            int scaleParameter = cameraType == CameraType.Front ? 2 : 3;

            if (pageNumber == 0)
            {
                yValue = Constants.OneLineNavigationBarHeight;
            }
            if (pageNumber == 1)
            {
                yValue = Constants.TwoLineNavigationBarHeight;
            }
            if (pageNumber >= 2)
            {
                yValue = Constants.ThreeLineNavigationBarHeight;
            }

            var photoCroppedHeight = sourceImage.Size.Height - scaleParameter * yValue - scaleParameter * Constants.TakePhotoPageBottomBarValue;

            CGSize imgSize = sourceImage.Size;

            UIGraphics.BeginImageContext(new CGSize(imgSize.Width, photoCroppedHeight));
            CGContext imgToCrop = UIGraphics.GetCurrentContext();

            CGRect croppingRectangle = new CGRect(0, 0, imgSize.Width, photoCroppedHeight);
            imgToCrop.ClipToRect(croppingRectangle);

            CGRect drawRectangle = new CGRect(0, scaleParameter * -yValue, imgSize.Width, imgSize.Height);

            sourceImage.Draw(drawRectangle);
            var croppedImg = UIGraphics.GetImageFromCurrentImageContext();

            if (cameraType == CameraType.Front)
            {
                croppedImg = croppedImg.GetImageWithHorizontallyFlippedOrientation();
            }

            UIGraphics.EndImageContext();

            croppedImg = ResizeImage(croppedImg);

            return croppedImg;
        }

        private UIImage ResizeImage(UIImage sourceImage)
        {
            var maxSize = Math.Max(sourceImage.Size.Width, sourceImage.Size.Height);

            var scaledSize = new CGSize(sourceImage.Size.Width, sourceImage.Size.Height);
            if (maxSize > Constants.MaxPhotoSize)
            {
                scaledSize = new CGSize(
                    (nfloat)Math.Round((sourceImage.Size.Width / maxSize) * Constants.MaxPhotoSize),
                    (nfloat)Math.Round((sourceImage.Size.Height / maxSize) * Constants.MaxPhotoSize));
            }

            UIGraphics.BeginImageContext(scaledSize);
            sourceImage.Draw(new CGRect(0, 0, scaledSize.Width, scaledSize.Height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return resultImage;
        }

        private static nfloat GetRadians(double degrees)
        {
            return (nfloat)(degrees * Math.PI / 180.0);
        }

        private byte[] GetByteArrayFromUIImage(UIImage image)
        {
            using (NSData imageData = image.AsJPEG(0.75f))
            {
                byte[] photoByteArray = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, photoByteArray, 0, Convert.ToInt32(imageData.Length));
                return photoByteArray;
            }
        }

        #endregion
    }
}