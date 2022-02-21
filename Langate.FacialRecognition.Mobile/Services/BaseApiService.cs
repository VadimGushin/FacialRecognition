using Langate.FacialRecognition.Mobile.Config;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.MobileApi.Model;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Langate.FacialRecognition.Mobile.Services
{
    public class BaseApiService
    {
        #region Variables
        
        protected string ApiUrl => $"{ApplicationConfig.SiteApiUrl}api/";

        private const string _tokenValueKey = "token";
        private const string _deviceIdKey = "deviceid";

        #endregion

        #region Services

        private readonly ILocalDataService _localDataService = Mvx.IoCProvider.Resolve<ILocalDataService>();
        private readonly IPlatformService _platformService = Mvx.IoCProvider.Resolve<IPlatformService>();

        #endregion

        #region Public Methods

        public async Task<HttpResponseMessage> ExecuteGetAsync(string url)
        {
            if (!IsInternetConnectionAvailable())
            {
                return null;
            }
            var httpClient = GetClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                Debug.WriteLine($"Resopnse result {response.StatusCode} , {response.RequestMessage} ");
                return response;
            }
            catch (Exception ex)
            {
                SendCrashText(ex, "Get");
                return new HttpResponseMessage( HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> ExecutePutAsync<T>(string url, T data)
        {
            if (!IsInternetConnectionAvailable())
            {
                return null;
            }
            var httpClient = GetClient();
            string content = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            try
            {
                var response = await httpClient.PutAsync(url, httpContent);
                Debug.WriteLine($"Resopnse result {response.StatusCode} , {response.RequestMessage} ");
                return response;
            }
            catch (Exception ex)
            {
                SendCrashText(ex, "Put");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<HttpResponseMessage> ExecutePostAsync<T>(string url, T data)
        {
            if (!IsInternetConnectionAvailable())
            {
                return null;
            }
            var httpClient = GetClient();
            string content = JsonConvert.SerializeObject(data);
            Debug.WriteLine(content);
            try
            {
                var response = await httpClient.PostAsync(url, new StringContent(content, Encoding.Unicode, "application/json"));
                Debug.WriteLine($"Resopnse result {response.StatusCode} , {response.RequestMessage}  ###");
                return response;
            }
            catch (Exception ex)
            {
                SendCrashText(ex, "Post");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }

        #endregion

        #region Protected Methods

        protected HttpClient GetClient(bool isImageUpload = false)
        {
            var httpClient = new HttpClient();
            string mediaType = isImageUpload ? "text/plain" : "application/json";
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            httpClient.DefaultRequestHeaders.Add(_tokenValueKey, _localDataService.Token);
            httpClient.DefaultRequestHeaders.Add(_deviceIdKey, _platformService.GetDeviceId());
            return httpClient;
        }

        protected bool IsInternetConnectionAvailable()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Private Methods

        private void SendCrashText(Exception ex, string methodType)
        {
            Debug.WriteLine($"Execute{methodType}Async(), error: {ex.Message}");
            Crashes.TrackError(ex);
            Analytics.TrackEvent($"Track error: {ex.Message}");
        }

        #endregion

        #region Response
        protected async Task<TItem> DeserializeResponseAsync<TItem>(HttpResponseMessage responseMessage) where TItem : ResponseDto
        {
            if (responseMessage == null)
            {
                return null;
            }
            TItem result = default;
            if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                result = Activator.CreateInstance<TItem>();
                TrySetUnsuccedResult(result, responseMessage.ReasonPhrase);
                return result;
            }

            //string response = await responseMessage.Content.ReadAsStringAsync();
            //if (string.IsNullOrWhiteSpace(response))
            //{
            //    result = Activator.CreateInstance<TItem>();
            //    TrySetUnsuccedResult(result);
            //    return result;
            //}
            try
            {
                string response = await responseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TItem>(response);
            }
            catch (Exception ex)
            {
                result = Activator.CreateInstance<TItem>();
                TrySetUnsuccedResult(result, ex.Message);
                return result;
            }

        }

        private void TrySetUnsuccedResult<TItem>(TItem model, string message)
        {
            if ((model as ResponseDto) != null)
            {
                (model as ResponseDto).Message = message;
                (model as ResponseDto).Valid = false;
            }
            
        }
        #endregion
    }
}
