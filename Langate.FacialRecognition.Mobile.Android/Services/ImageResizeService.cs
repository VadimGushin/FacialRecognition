using System;
using System.IO;
using Android.Graphics;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Plugin.CurrentActivity;

namespace Langate.FacialRecognition.Mobile.Droid.Services
{
    public class ImageResizeService : IImageResizeService
    {
        #region Variables

        private Bitmap _iniBitmap;

        #endregion

        #region Publick Methods

        public byte[] ResizeImage(byte[] image, int pageNumber)
        {
            _iniBitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            if (_iniBitmap == null)
            {
                return null;
            }

            var resizedImage = TryResizeImage(pageNumber, ImageSourceType.Library);
            if (resizedImage == null)
            {
                return null;
            }

            return GetByteArrayFromBitmap(resizedImage);
        }

        public byte[] ResizeImage(object image, int pageNumber, CameraType cameraType, int previewHeightChange)
        {
            _iniBitmap = image as Bitmap;
            if (_iniBitmap == null)
            {
                return null;
            }

            var resizedImage = TryResizeImage(pageNumber, ImageSourceType.Camera, previewHeightChange);
            if (resizedImage == null)
            {
                return null;
            }

            return GetByteArrayFromBitmap(resizedImage);
        }

        #endregion

        #region Private Methods

        private Bitmap TryResizeImage(int pageNumber, ImageSourceType sourceType, int previewHeightChange = 0)
        {
            var halfOfPreviewHeightChange = 0;
            if (previewHeightChange > 0)
            {
                halfOfPreviewHeightChange = (int)(Math.Round((double)(previewHeightChange / 2)));
            }

            if (sourceType == ImageSourceType.Library)
            {
                return CropPhoto(_iniBitmap) ?? _iniBitmap;
            }

            var metrixDensity = CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.Density;

            int yValue = 0;
            if (pageNumber == 0)
            {
                yValue = (int)Math.Round(Constants.OneLineNavigationBarHeight * metrixDensity);
            }
            if (pageNumber == 1)
            {
                yValue = (int)Math.Round(Constants.TwoLineNavigationBarHeight * metrixDensity);
            }
            if (pageNumber >= 2)
            {
                yValue = (int)Math.Round(Constants.ThreeLineNavigationBarHeight * metrixDensity);
            }
            yValue = yValue - halfOfPreviewHeightChange;

            int bottomValue = (int)Math.Round(Constants.TakePhotoPageBottomBarValue * metrixDensity);
            var height = _iniBitmap.Height - yValue - bottomValue + halfOfPreviewHeightChange;

            var croppedPhoto = Bitmap.CreateBitmap(_iniBitmap, 0, yValue, _iniBitmap.Width, height, new Matrix(), false);
            return CropPhoto(croppedPhoto) ?? _iniBitmap;
        }

        private Bitmap CropPhoto(Bitmap photo)
        {
            var maxSize = Math.Max(photo.Width, photo.Height);

            if (maxSize > Constants.MaxPhotoSize)
            {
                var resWidth = (int)Math.Round((float)(((float)photo.Width / (float)maxSize) * Constants.MaxPhotoSize));
                var resHeight = (int)Math.Round((float)(((float)photo.Height / (float)maxSize) * Constants.MaxPhotoSize));

                return Bitmap.CreateScaledBitmap(photo, resWidth, resHeight, false);
            }
            return photo;
        }

        private byte[] GetByteArrayFromBitmap(Bitmap image)
        {
            using (var stream = new MemoryStream())
            {
                image.Compress(Bitmap.CompressFormat.Jpeg, 75, stream);
                byte[] imageByte = stream.ToArray();
                return imageByte;
            }
        }

        #endregion
    }
}