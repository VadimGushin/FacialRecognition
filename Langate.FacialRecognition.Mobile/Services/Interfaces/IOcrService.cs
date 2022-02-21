using Langate.FacialRecognition.MobileApi.Model;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services.Interfaces
{
    public interface IOcrService
    {
        Task<OcrGovernmentIdResponseDto> OcrGovernmentIdAsync(int imageId, int inviteResponseId);
        Task<ResponseDto> OcrOfGovernmentIdAsync(OcrOfIdRequestDto userData, int ocrId);
    }
}
