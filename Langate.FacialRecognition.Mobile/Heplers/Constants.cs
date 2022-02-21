
using System;

namespace Langate.FacialRecognition.Mobile.Heplers
{
    public class Constants
    {
        #region Resources

        public static string DocumentImageSouce = "img_document.png";
        public static string FrontFaceImageSource = "img_front_face_photo.png";
        public static string LeftFaceImageSource = "img_left_face_photo.png";
        public static string RightFaceImageSource = "img_right_face_photo.png";

        #endregion

        #region Storage

        public static string PhotoStorageKey = "_photoStorageKey";
        public static string PhotoStorageSecondKey = "_photoStorageSecondKey";
        public static string DataStorageKey = "dataStorageKey";
        public static string UploadingResultStorageKey = "uploadingResultStorageKey";
        public static string AgreementStorageKey = "agreementStorageKey";
        public static string TokenKey = "tokenStorageKey";
        public static string InviteKey = "inviteDataKey";
        public static string ApiUploadResultKey = "apiUploadResultKey";
        public static string ApiUploadingModelKey = "apiUploadingModelKey";
        public static string ApiUploadingCountKey = "apiUploadingCountKey";
        public static string ApiUploadingResultTextKey = "apiUploadingResultTextKey";

        #endregion

        #region NumberValues

        public static int MaxPhotoCount = 4;
        public static int OneLineNavigationBarHeight = 80;
        public static int TwoLineNavigationBarHeight = 100;
        public static int ThreeLineNavigationBarHeight = 125;
        public static int TakePhotoPageBottomBarValue = 100;
        public static int MaxPhotoSize = 1000;

        #endregion

        #region Errors

        public static string CameraErrorText = "Error while receiving image from camera";
        public static string ImageLibraryErrorText = "Error while receiving image from library";
        public static string CannotOpenPreviousPage = "Сan't open previous page";
        public static string InternetConnectionError = "Please check internet connection";
        public static string UploadingImageError = "Uploading image error";
        public static string InvalidateTokenError = "Unable to dispose token";
        public static string SomethingWrongError = "Something went wrong";
        public static string AgreementResponseError = "Failed to get user agreement";
        public static string UploadingDataError = "Error while uploading data";

        #endregion

        #region Api Stack Errors

        public static string ApiGetInviteError = "API GetInviteModelAsync error";
        public static string ApiGetAgreementError = "API GetAgreementAsync error";
        public static string ApiUploadPhotoError = "API UploadPhotoAsync error";
        public static string ApiCreateResponseError = "API CreateResponseAsync error";
        public static string ApiUpdateResponseError = "API UpdateResponseAsync error";
        public static string ApiRecognizeFaceError = "API RecognizeFaceAsync error";
        public static string ApiOcrError = "API OcrGovernmentIdAsync error";
        public static string ApiOcrOfError = "API OcrOfGovernmentIdAsync error";
        public static string ApiProcessImageError = "API ProcessUserImage error";
        public static string ApiDecisionError = "API ProvideDecisionApiServiceAsync error";
        public static string ApiInvalidateTokenError = "API InvalidateTokenAsync error";

        #endregion

        #region Messages

        public static string ChangeCamera = "Camera mode changed";
        public static string DOBInvalidDate = "Invalid date";
        public static string DOBEnterDate = "Please enter date";
        public static string DOBEnterValidDate = "Please enter valid date";
        public static string DOBInvalidYear = "Invalid year";
        public static string DOBEnterYear = "Please enter year";
        public static string DOBEnterValidYear = "Please enter valid year";
        public static string DOBEnterMonth = "Please enter a month";
        public static string DOBEnterValue = "Please enter your date of birth";
        public static string InvalidTokenMessage = "The invite link is not valid. Please request a new invite from the site staff.";
        public static string DefaultAgreementText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        #endregion

        #region Values

        public static string SuccededResult = "succeded";
        public static string UnsuccededResult = "unsucceded";
        public static string DefaultToken = "default";
        public const int MinValidYear = 1900;

        #endregion

        #region Entry Keys

        public static string EntryFirstLastNameValueKey = "name";
        public static string EntryMiddleNameValueKey = "middle_name";
        public static string EntryNumberValueKey = "number";
        public static string EntryIdValueKey = "id";

        #endregion

        #region Entry Validation Patterns

        public static string MiddleNameValidationPattern = @"^[a-zA-Z-]+$";
        public static string FirstLastNameValidationPattern = @"^[a-zA-Z]+$";
        public static string IdValidationPattern = @"^[[a-zA-Z0-9-]+$";
        public static string NumberValidationPattern = @"^[0-9]+$";
        public static string DefaultValidationPattern = @"^[a-z0-9\_-]+$";

        #endregion

        #region Colors

        public static Xamarin.Forms.Color WarningColor = Xamarin.Forms.Color.FromRgb(221, 163, 33);
        public static Xamarin.Forms.Color ErrorColor = Xamarin.Forms.Color.FromRgb(241, 23, 23);

        #endregion
    }
}
