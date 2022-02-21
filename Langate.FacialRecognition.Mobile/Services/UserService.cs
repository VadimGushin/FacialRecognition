using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.MobileApi.Model;
using Langate.FacialRecognition.MobileApi.Model.Invites.ValidateInviteToken;
using MvvmCross;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services
{
    public class UserService : BaseApiService, IUserService
    {
        #region Services

        private readonly IPlatformService _platformService = Mvx.IoCProvider.Resolve<IPlatformService>();
        private readonly ILocalDataService _localDataService = Mvx.IoCProvider.Resolve<ILocalDataService>();

        #endregion

        #region Public Methods

        public async Task<ValidateInviteTokenResponseDto> GetInviteModelAsync()
        {
            HttpResponseMessage apiResponse = await ExecuteGetAsync($"{ApiUrl}invites/validatetoken{GetRequestParameters()}");
            var result = await DeserializeResponseAsync<ValidateInviteTokenResponseDto>(apiResponse);
            return result;
        }

        public async Task<EndUserAgreementDto> GetAgreementAsync()
        {
            var token = _localDataService.Token;

            HttpResponseMessage apiResponse = await ExecuteGetAsync($"{ApiUrl}invites/eua?token={token}");
            var result = await DeserializeResponseAsync<EndUserAgreementDto>(apiResponse);
            return result;
        }

        public async Task<InviteResponseResponseDto> CreateResponseAsync(InviteResponseCreateRequestDto data)
        {
            HttpResponseMessage apiResponse = await ExecutePostAsync($"{ApiUrl}UserInput/inviteresponse/", data);
            var result = await DeserializeResponseAsync<InviteResponseResponseDto>(apiResponse);
            return result;
        }

        public async Task<InviteResponseResponseDto> UpdateResponseWithUserFaceIdAsync(InviteResponseUpdateImagesRequestDto data)
        {
            HttpResponseMessage apiResponse = await ExecutePutAsync($"{ApiUrl}UserInput/inviteresponse/", data);
            var result = await DeserializeResponseAsync<InviteResponseResponseDto>(apiResponse);
            return result;
        }

        public async Task<InviteResponseResponseDto> UpdateResponseWithGovernmentIdAsync(InviteResponseUpdateDocumentRequestDto data)
        {
            HttpResponseMessage apiResponse = await ExecutePutAsync($"{ApiUrl}UserInput/document/", data);
            var result = await DeserializeResponseAsync<InviteResponseResponseDto>(apiResponse);
            return result;
        }

        public async Task<InviteResponseGetResponseDto> GetUserPartialIdentifiersAsync(int inviteResponseId)
        {
            HttpResponseMessage apiResponse = await ExecuteGetAsync($"{ApiUrl}userInput/inviteresponse/{inviteResponseId}");
            var result = await DeserializeResponseAsync<InviteResponseGetResponseDto>(apiResponse);
            return result;
        }

        public async Task<ResponseDto> RecognizeFaceAsync(int imageId, int inviteResponseId)
        {
            var url = $"{ApiUrl}userInput/recognize?imageId={imageId}&inviteResponseId={inviteResponseId}";
            HttpResponseMessage apiResponse = await ExecuteGetAsync(url);
            var result = await DeserializeResponseAsync<ResponseDto>(apiResponse);
            return result;
        }

        public async Task<ResponseDto> ProcessUserPhotoAsync(int photoId)
        {
            HttpResponseMessage apiResponse = await ExecuteGetAsync($"{ApiUrl}userInput/processimage/{photoId}");
            var result = await DeserializeResponseAsync<ResponseDto>(apiResponse);
            return result;
        }

        public async Task<EnquireDecisionServiceResponseDto> ProvideDecisionApiServiceAsync(int responseId, int ocrId)
        {
            var url = $"{ApiUrl}userInput/decision?inviteResponseId={responseId}&ocrResultId={ocrId}";
            HttpResponseMessage apiResponse = await ExecuteGetAsync(url);
            var result = await DeserializeResponseAsync<EnquireDecisionServiceResponseDto>(apiResponse);
            return result;
        }

        public async Task<SimpleTokensDto> GetValidTokensAsync()
        {
            HttpResponseMessage apiResponse = await ExecuteGetAsync($"{ApiUrl}invites/gettokens{GetRequestParameters()}");
            if (apiResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            string response = await apiResponse.Content.ReadAsStringAsync();
            return !string.IsNullOrWhiteSpace(response)
                    ? JsonConvert.DeserializeObject<SimpleTokensDto>(response) : null;
        }

        public async Task<ResponseDto> InvalidateTokenAsync()
        {
            var token = _localDataService.Token;

            HttpResponseMessage apiResponse = await ExecutePutAsync($"{ApiUrl}invites/invalidatetoken?token={token}", string.Empty);
            var result = await DeserializeResponseAsync<ResponseDto>(apiResponse);
            return result;
        }

        #endregion

        #region Private Methods

        private string GetRequestParameters()
        {
            var token = _localDataService.Token;
            var deviceId = _platformService.GetDeviceId();
            var phoneNumber = _platformService.GetPhoneNumber();

            return $"?token={token}&deviceId={deviceId}&phoneNumber={phoneNumber}";
        }

        #endregion
    }
}
