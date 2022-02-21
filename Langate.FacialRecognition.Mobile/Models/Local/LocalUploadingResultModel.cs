
namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalUploadingResultModel
    {

        public int ResponseId { get; set; }
        public int OcrId { get; set; }
        public int EuaId { get; set; }
        public bool IsAgreementConfirm { get; set; }
        public bool IsInviteResponseCreated { get; set; }
        public bool IsRecognizeComplete { get; set; }
        public bool IsOcrComplete { get; set; }
        public bool IsOcrOfComplete { get; set; }
        public bool IsDecisionComplete { get; set; }
        public bool IsUploadingComplete { get; set; }

        public LocalUploadingResultModel()
        {
            this.ResponseId = 0;
            this.OcrId = 0;
            this.EuaId = 0;
            this.IsAgreementConfirm = false;
            this.IsInviteResponseCreated = false;
            this.IsRecognizeComplete = false;
            this.IsOcrComplete = false;
            this.IsOcrOfComplete = false;
            this.IsDecisionComplete = false;
            this.IsUploadingComplete = false;
        }
    }
}
