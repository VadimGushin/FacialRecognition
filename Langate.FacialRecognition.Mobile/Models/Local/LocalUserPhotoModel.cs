using Langate.FacialRecognition.MobileApi.Model;

namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalUserPhotoModel
    {
        public int PageNumber { get; set; }
        public bool IsFull { get; set; }
        public bool IsCleared { get; set; }
        public bool IsRetaked { get; set; }
        public string PageTitle { get; set; }
        public string FaceImageSource { get; set; }
        public ImageType PhotoType { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoString { get; set; }
        public int ImageId { get; set; }
        public bool IsImageUploaded { get; set; }

        public LocalUserPhotoModel()
        {
            PageNumber = 0;
            PageTitle = string.Empty;
            FaceImageSource = string.Empty;
            PhotoType = ImageType.None;
            IsFull = false;
            IsRetaked = false;
            Photo = null;
            Photo = null; 
            PhotoString = string.Empty;
            ImageId = 0;
            IsImageUploaded = false;
        }

        public LocalUserPhotoModel(int pageNumber, string title, string faceImageSource, ImageType photoType)
        {
            PageNumber = pageNumber;
            PageTitle = title;
            FaceImageSource = faceImageSource;
            PhotoType = photoType;
            IsFull = false;
            IsCleared = false;
            IsRetaked = false;
            Photo = null;
            PhotoString = string.Empty;
            ImageId = 0;
            IsImageUploaded = false;
        }

        public void Clear()
        {
            this.IsFull = false;
            this.IsCleared = true;
            IsRetaked = false;
            this.Photo = null;
            PhotoString = string.Empty;
            this.ImageId = 0;
            this.IsImageUploaded = false;
        }

        public void SetData(string photo, byte[] bytes = null)
        {
            this.IsFull = true;
            this.IsCleared = false;
            this.PhotoString = photo;
            this.Photo = bytes;
        }

        public void ChangeRetakedStatus(bool isRetaked)
        {
            IsRetaked = isRetaked;
        }

        public void SetImageId(int id, bool isImageUploaded)
        {
            this.ImageId = id;
            this.IsImageUploaded = isImageUploaded;
        }
    }
}
