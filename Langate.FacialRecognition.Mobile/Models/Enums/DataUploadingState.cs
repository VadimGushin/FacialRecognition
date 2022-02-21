
namespace Langate.FacialRecognition.Mobile.Models.Enums
{
    public enum DataUploadingState
    {
        Default = 0,
        FirstStart = 1,
        SetData = 2,
        FirstFlowInProgress = 3,
        SecondFlowInProgress = 4,
        Succeded = 5,
        UncorrectlyId = 6,
        UncorrectlyPhoto = 7,
        FirstFlowUploadingError = 8,
        SecondFlowUploadingError = 9,
        Unsucceded = 10
    }
}
