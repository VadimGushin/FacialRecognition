
namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalReviewPhotoModel
    {
        public byte[] Photo { get; set; }
        public bool IsFacePhoto { get; set; }

        public LocalReviewPhotoModel(byte[] photo, bool isFacePhoto)
        {
            Photo = photo;
            IsFacePhoto = isFacePhoto;
        }
    }
}
