using Langate.FacialRecognition.MobileApi.Model;
using Langate.FacialRecognition.MobileApi.Model.Invites.ValidateInviteToken;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services.Interfaces
{
    public interface IUserService
    {
        Task<ValidateInviteTokenResponseDto> GetInviteModelAsync();
        Task<EndUserAgreementDto> GetAgreementAsync();
        Task<InviteResponseResponseDto> CreateResponseAsync(InviteResponseCreateRequestDto data);
        Task<InviteResponseResponseDto> UpdateResponseWithUserFaceIdAsync(InviteResponseUpdateImagesRequestDto data);
        Task<InviteResponseResponseDto> UpdateResponseWithGovernmentIdAsync(InviteResponseUpdateDocumentRequestDto data);
        Task<InviteResponseGetResponseDto> GetUserPartialIdentifiersAsync(int InviteResponseId);
        Task<ResponseDto> RecognizeFaceAsync(int imageId, int inviteResponseId);
        Task<ResponseDto> ProcessUserPhotoAsync(int photoId);
        Task<EnquireDecisionServiceResponseDto> ProvideDecisionApiServiceAsync(int responseId, int ocrId);
        Task<SimpleTokensDto> GetValidTokensAsync();
        Task<ResponseDto> InvalidateTokenAsync();
    }
}
