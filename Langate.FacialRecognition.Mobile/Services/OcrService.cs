using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.MobileApi.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services
{
    public class OcrService : BaseApiService, IOcrService
    {
        #region Publick Methods

        public async Task<OcrGovernmentIdResponseDto> OcrGovernmentIdAsync(int imageId, int inviteResponseId)
        {
            var url = $"{ApiUrl}UserInput/ocr?imageId={imageId}&inviteResponseId={inviteResponseId}";
            HttpResponseMessage apiResponse = await ExecuteGetAsync(url);
            var result = await DeserializeResponseAsync<OcrGovernmentIdResponseDto>(apiResponse);
            return result;
        }

        public async Task<ResponseDto> OcrOfGovernmentIdAsync(OcrOfIdRequestDto userData, int ocrId)
        {
            HttpResponseMessage apiResponse = await ExecutePostAsync($"{ApiUrl}ocr?id={ocrId}", userData);
            var result = await DeserializeResponseAsync<InviteResponseResponseDto>(apiResponse);
            return result;
        }

        #endregion
    }
}
