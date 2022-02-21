using Langate.FacialRecognition.Mobile.Extensions;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Models.Enums;
using Langate.FacialRecognition.Mobile.Models.Local;
using Langate.FacialRecognition.Mobile.Services.Interfaces;
using Langate.FacialRecognition.MobileApi.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using strings = Langate.FacialRecognition.Mobile.Resources.Strings.Strings;

namespace Langate.FacialRecognition.Mobile.Services
{
    public class LocalDataService : ILocalDataService
    {
        #region Constants

        private int _maxPhotoStringLenght = 50000;

        #endregion

        #region Properties
        private List<LocalUserPhotoModel> _localData { get; set; }

        public string Token
        {
            get
            {
                return SecureStorage.GetAsync(Constants.TokenKey).Result ?? Constants.DefaultToken;
            }
            set
            {
                SecureStorage.SetAsync(Constants.TokenKey, value);
            }
        }

        public DataUploadingState UploadingState
        {
            get
            {
                var value = SecureStorage.GetAsync(Constants.ApiUploadResultKey).Result;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return DataUploadingState.Default;
                }
                var result = (DataUploadingState)Enum.Parse(typeof(DataUploadingState), value);
                return result;
            }
            set
            {
                var state = value.ToString();
                SecureStorage.SetAsync(Constants.ApiUploadResultKey, state);
            }
        }

        public LocalUploadingResultModel UploadingResultModel
        {
            get
            {
                var value = SecureStorage.GetAsync(Constants.ApiUploadingModelKey).Result;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return new LocalUploadingResultModel();
                }
                var result = JsonConvert.DeserializeObject<LocalUploadingResultModel>(value);
                return result;
            }
            set
            {
                var result = JsonConvert.SerializeObject(value);
                SecureStorage.SetAsync(Constants.ApiUploadingModelKey, result);

            }
        }


        public LocalUploadingResultTextModel UploadingResultTextModel
        {
            get
            {
                var value = SecureStorage.GetAsync(Constants.ApiUploadingResultTextKey).Result;
                if (string.IsNullOrWhiteSpace(value))
                {
                    return new LocalUploadingResultTextModel();
                }
                var result = JsonConvert.DeserializeObject<LocalUploadingResultTextModel>(value);
                return result;
            }
            set
            {
                var result = JsonConvert.SerializeObject(value);
                SecureStorage.SetAsync(Constants.ApiUploadingResultTextKey, result);
            }
        }

        public int UploadingCount
        {
            get
            {
                var result = SecureStorage.GetAsync(Constants.ApiUploadingCountKey).Result;
                int count = 0;
                var parseResult = int.TryParse(result, out count);
                return parseResult ? count : 0;
            }
            set
            {
                SecureStorage.SetAsync(Constants.ApiUploadingCountKey, value.ToString());
            }
        }

        #endregion

        #region Public Methods

        public void ClearStorage()
        {
            SecureStorage.RemoveAll();
        }

        public async Task<bool> InitDataAsync(bool isNeededInitMainData = true)
        {
            if (!string.IsNullOrWhiteSpace(await SecureStorage.GetAsync(Constants.TokenKey))
                /*&& !string.IsNullOrWhiteSpace(await SecureStorage.GetAsync(Constants.DataStorageKey))*/)
            {
                //ClearStorage();
                return false;
            }
            _localData = new List<LocalUserPhotoModel>();
            _localData.Add(new LocalUserPhotoModel(0, strings.TakePhotoId, Constants.DocumentImageSouce, ImageType.GovernmentId));
            _localData.Add(new LocalUserPhotoModel(1, strings.TakeFrontalPhotoText, Constants.FrontFaceImageSource, ImageType.FrontalFace));
            _localData.Add(new LocalUserPhotoModel(2, strings.TakeLeftPhoto, Constants.LeftFaceImageSource, ImageType.FaceFromLeft));
            _localData.Add(new LocalUserPhotoModel(3, strings.TakeRightPhoto, Constants.RightFaceImageSource, ImageType.FaceFromRight));

            for (int i = 0; i < Constants.MaxPhotoCount; i++)
            {
                var photo = JsonConvert.SerializeObject(_localData[i]);
                await SecureStorage.SetAsync($"{i}{Constants.PhotoStorageKey}", photo);
                await SecureStorage.SetAsync($"{i}{Constants.PhotoStorageSecondKey}", string.Empty);
            }
            await SecureStorage.SetAsync(Constants.DataStorageKey, JsonConvert.SerializeObject(new LocalUserDataModel()));

            if (isNeededInitMainData)
            {
                await InitMainLocalData();
            }
            return true;
        }

        public async Task<List<LocalUserPhotoModel>> GetAllPhotosAsync()
        {
            _localData = new List<LocalUserPhotoModel>();
            for (int i = 0; i < Constants.MaxPhotoCount; i++)
            {
                var localPhoto = await SecureStorage.GetAsync($"{i}{Constants.PhotoStorageKey}");
                var deserializedPhoto = JsonConvert.DeserializeObject<LocalUserPhotoModel>(localPhoto);
                await GetAllPhotoPartsAsync(deserializedPhoto);
                _localData.Add(deserializedPhoto);
            }
            return _localData;
        }

        public async Task<LocalUserPhotoModel> GetPhotoAsync(int dataKey)
        {
            var photo = await SecureStorage.GetAsync($"{dataKey}{Constants.PhotoStorageKey}");
            var result = JsonConvert.DeserializeObject<LocalUserPhotoModel>(photo);
            await GetAllPhotoPartsAsync(result);
            return result;
        }

        public async Task SetPhotoAsync(int pageNumber, byte[] userPhoto)
        {
            var photoModel = await SecureStorage.GetAsync($"{pageNumber}{Constants.PhotoStorageKey}");
            if (string.IsNullOrWhiteSpace(photoModel))
            {
                return;
            }
            var userData = JsonConvert.DeserializeObject<LocalUserPhotoModel>(photoModel);
            await SaveImageAsync(userData, userPhoto.ToBase64());
            if (userData.PageNumber < Constants.MaxPhotoCount - 1)
            {
                var nextPage = await SecureStorage.GetAsync($"{pageNumber + 1}{Constants.PhotoStorageKey}");
                var nextPhoto = JsonConvert.DeserializeObject<LocalUserPhotoModel>(nextPage);
                nextPhoto.IsCleared = false;
                await SecureStorage.SetAsync($"{pageNumber + 1}{Constants.PhotoStorageKey}", JsonConvert.SerializeObject(nextPhoto));
            }
        }

        public async Task ClearPhotoAsync(int dataKey)
        {
            var photoModel = await SecureStorage.GetAsync($"{dataKey}{Constants.PhotoStorageKey}");
            if (string.IsNullOrWhiteSpace(photoModel))
            {
                return;
            }
            var userData = JsonConvert.DeserializeObject<LocalUserPhotoModel>(photoModel);
            userData.Clear();
            await SecureStorage.SetAsync($"{dataKey}{Constants.PhotoStorageKey}", JsonConvert.SerializeObject(userData));
            await SecureStorage.SetAsync($"{dataKey}{Constants.PhotoStorageSecondKey}", string.Empty);
        }

        public async Task<LocalUserPhotoModel> GetNextPhotoAsync()
        {
            var items = await GetAllPhotosAsync();
            for (int i = 0; i < Constants.MaxPhotoCount; i++)
            {
                if (!items[i].IsFull)
                {
                    return items[i];
                }
            }
            if (items[items.Count - 1].IsFull)
            {
                return items[items.Count - 1];
            }
            return null;
        }

        public async Task<LocalUserPhotoModel> GetCurrentPhotoAsync()
        {
            var items = await GetAllPhotosAsync();
            var item = items.Where(x => x.IsFull).LastOrDefault();
            return item ?? items[0];
        }

        public async Task<LocalUserPhotoModel> GetNextNeedRetakePhotoAsync()
        {
            var items = await GetAllPhotosAsync();
            var item = items.Where(x => !x.IsRetaked).FirstOrDefault();
            return item ?? null;
        }

        public async Task<int> GetPhotosCountAsync()
        {
            var items = await GetAllPhotosAsync();
            if (items != null)
            {
                return items.Count;
            }
            return 0;
        }

        public async Task ChangeRetakedPhotoStatusAsync(int pageNumber, bool isRetaked)
        {
            var photoModel = await SecureStorage.GetAsync($"{pageNumber}{Constants.PhotoStorageKey}");
            if (string.IsNullOrWhiteSpace(photoModel))
            {
                return;
            }
            var userData = JsonConvert.DeserializeObject<LocalUserPhotoModel>(photoModel);
            userData.ChangeRetakedStatus(isRetaked);
            await SecureStorage.SetAsync($"{pageNumber}{Constants.PhotoStorageKey}", JsonConvert.SerializeObject(userData));
        }

        public async Task SetImageId(int pageNumber, int id, bool isUploaded = true)
        {
            var photoModel = await SecureStorage.GetAsync($"{pageNumber}{Constants.PhotoStorageKey}");
            if (string.IsNullOrWhiteSpace(photoModel))
            {
                return;
            }
            var userData = JsonConvert.DeserializeObject<LocalUserPhotoModel>(photoModel);
            userData.SetImageId(id, isUploaded);
            await SecureStorage.SetAsync($"{pageNumber}{Constants.PhotoStorageKey}", JsonConvert.SerializeObject(userData));
        }

        public async Task<LocalUserDataModel> GetUserDataAsync()
        {
            var userData = await SecureStorage.GetAsync(Constants.DataStorageKey);
            if (string.IsNullOrWhiteSpace(userData))
            {
                return null;
            }
            var resultModel = JsonConvert.DeserializeObject<LocalUserDataModel>(userData);
            return resultModel;
        }

        public async Task SetUserDataAsync(LocalUserDataModel userModel)
        {
            var serializedModel = JsonConvert.SerializeObject(userModel);
            await SecureStorage.SetAsync(Constants.DataStorageKey, serializedModel);
        }

        public async Task<ValidateInviteTokenDto> GetInviteModelAsync()
        {
            var inviteDataResult = await SecureStorage.GetAsync(Constants.InviteKey);
            var inviteDataModel = JsonConvert.DeserializeObject<ValidateInviteTokenDto>(inviteDataResult);
            if (inviteDataModel == null)
            {
                inviteDataModel = new ValidateInviteTokenDto();
            }
            if (inviteDataModel.Gender == null || !inviteDataModel.Gender.Any())
            {
                //inviteDataModel.Gender = Enum.GetNames(typeof(Gender));
                inviteDataModel.Gender = EnumHelper.GetCollectionByType(typeof(Gender));
            }
            if (inviteDataModel.IdType == null || !inviteDataModel.IdType.Any())
            {
                //inviteDataModel.IdType = Enum.GetNames(typeof(IdType));
                inviteDataModel.IdType = EnumHelper.GetCollectionByType(typeof(IdType));
            }
            return inviteDataModel;
        }

        public async Task SetInviteModelAsync(ValidateInviteTokenDto dataModel)
        {
            var serializedModel = JsonConvert.SerializeObject(dataModel);
            await SecureStorage.SetAsync(Constants.InviteKey, serializedModel);
        }

        #endregion

        #region Private Methods

        private async Task InitMainLocalData()
        {
            await SecureStorage.SetAsync(Constants.InviteKey, JsonConvert.SerializeObject(new ValidateInviteTokenDto()));
            Token = Constants.DefaultToken;
            UploadingState = DataUploadingState.Default;
            UploadingResultModel = new LocalUploadingResultModel();
            UploadingResultTextModel = new LocalUploadingResultTextModel();
            UploadingCount = 0;
        }

        private async Task SaveImageAsync(LocalUserPhotoModel photoModel, string photoString)
        {
            if (photoString.Length > _maxPhotoStringLenght)
            {
                var firstImagePart = photoString.Substring(0, _maxPhotoStringLenght);
                var secondImagePart = photoString.Substring(_maxPhotoStringLenght, photoString.Length - _maxPhotoStringLenght);
                photoModel.SetData(firstImagePart);
                var changedPhoto = JsonConvert.SerializeObject(photoModel);
                await SecureStorage.SetAsync($"{photoModel.PageNumber}{Constants.PhotoStorageKey}", changedPhoto);
                await SecureStorage.SetAsync($"{photoModel.PageNumber}{Constants.PhotoStorageSecondKey}", secondImagePart);
            }
            if (photoString.Length <= _maxPhotoStringLenght)
            {
                photoModel.SetData(photoString);
                var changedPhoto = JsonConvert.SerializeObject(photoModel);
                await SecureStorage.SetAsync($"{photoModel.PageNumber}{Constants.PhotoStorageKey}", changedPhoto);
            }
        }

        private async Task GetAllPhotoPartsAsync(LocalUserPhotoModel photoModel)
        {
            var secondPhotoPart = await SecureStorage.GetAsync($"{photoModel.PageNumber}{Constants.PhotoStorageSecondKey}");
            if (!string.IsNullOrEmpty(secondPhotoPart))
            {
                photoModel.PhotoString += secondPhotoPart;
            }

            photoModel.Photo = photoModel.PhotoString.ToBytes();
            photoModel.PhotoString = string.Empty;
        }

        #endregion
    }
}
