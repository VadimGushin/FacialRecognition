using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.MobileApi.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services.Interfaces
{
    public interface ILocalDataService
    {
        string Token { get; set; }
        DataUploadingState UploadingState { get; set; }
        LocalUploadingResultModel UploadingResultModel { get; set; }
        int UploadingCount { get; set; }
        LocalUploadingResultTextModel UploadingResultTextModel { get; set; }
        void ClearStorage();
        Task<bool> InitDataAsync(bool isNeededInitMainData = true);
        Task<List<LocalUserPhotoModel>> GetAllPhotosAsync();
        Task<LocalUserPhotoModel> GetPhotoAsync(int dataKey);
        Task SetPhotoAsync(int pageNumber, byte[] userPhoto);
        Task ClearPhotoAsync(int dataKey);
        Task<LocalUserPhotoModel> GetNextPhotoAsync();
        Task<LocalUserPhotoModel> GetCurrentPhotoAsync();
        Task<LocalUserPhotoModel> GetNextNeedRetakePhotoAsync();
        Task<int> GetPhotosCountAsync();
        Task ChangeRetakedPhotoStatusAsync(int pageNumber, bool isRetaked);
        Task SetImageId(int pageNumber, int id, bool isUploaded = true);
        Task<LocalUserDataModel> GetUserDataAsync();
        Task SetUserDataAsync(LocalUserDataModel userModel);
        Task<ValidateInviteTokenDto> GetInviteModelAsync();
        Task SetInviteModelAsync(ValidateInviteTokenDto dataModel);

    }
}
