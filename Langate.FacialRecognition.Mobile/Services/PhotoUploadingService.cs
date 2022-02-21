using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.MobileApi.Model;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Langate.FacialRecognition.Mobile.Services
{
    public class PhotoUploadingService : BaseApiService, IPhotoUploadingService
    {
        #region Services

        private readonly ILocalDataService _localDataService = Mvx.IoCProvider.Resolve<ILocalDataService>();

        #endregion

        #region Public Methods

        public async Task<UploadImageResponseDto> UploadPhotoAsync(int photoNumber)
        {
            var photoModel = await _localDataService.GetPhotoAsync(photoNumber);
            var multipartContent = new MultipartFormDataContent();

            var fileContent = new ByteArrayContent(photoModel.Photo);

            //fileContent.Headers.ContentLength = photoModel.Photo.Length;
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            multipartContent.Add(fileContent, "image", "image.jpg");

            HttpResponseMessage responseMessage;
            try
            {
                var httpClient = GetClient(true);
                responseMessage = await httpClient.PostAsync($"{ApiUrl}Upload?imageType={(int)(photoModel.PhotoType)}", multipartContent);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Debug.WriteLine($"ExecutePostAsync(), error: {ex.Message}");
                responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var result = await DeserializeResponseAsync<UploadImageResponseDto>(responseMessage);
            return result;
        }

        #endregion
    }
}
