
using Langate.FacialRecognition.MobileApi.Model;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services.Interfaces
{
    public interface IPhotoUploadingService
    {
        Task<UploadImageResponseDto> UploadPhotoAsync(int photoNumber);
    }
}
