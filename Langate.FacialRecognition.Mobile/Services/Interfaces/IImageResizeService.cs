
using Langate.FacialRecognition.Mobile.Models.Enums;

namespace Langate.FacialRecognition.Mobile.Services.Interfaces
{
    public interface IImageResizeService
    {
        byte[] ResizeImage(byte[] image, int pageNumber);

        byte[] ResizeImage(object image, int pageNumber, CameraType cameraType, int previewHeightChange);
    }
}
