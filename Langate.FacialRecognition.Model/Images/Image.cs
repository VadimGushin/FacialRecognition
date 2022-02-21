using Langate.FacialRecognition.MobileApi.Model;
using System;

namespace Langate.FacialRecognition.Model
{
    public class Image: Entity
    {
        public int ImageId { get; set; }
        public string AzureImageId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool Deleted { get; set; }
        public int ImageSize { get; set; }
        public ImageType ImageType { get; set; }

        public InviteResponse ImageDocumentInviteResponse { get; set; }
        public InviteResponse ImageFrontalInviteResponse { get; set; }
        public InviteResponse ImageLeftInviteResponse { get; set; }
        public InviteResponse ImageRightInviteResponse { get; set; }

        protected Image() { }

        public Image(
            string imageId,
            int imageSize,
            ImageType imageType) 
        {
            AzureImageId = imageId ?? throw new ArgumentNullException(nameof(imageId));
            ImageSize = imageSize;
            ImageType = imageType;
        }
    }
}