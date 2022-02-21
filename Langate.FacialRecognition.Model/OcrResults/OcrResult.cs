using System;

namespace Langate.FacialRecognition.Model
{
    public class OcrResults
    {
        public int OcrId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? InviteId { get; set; }
        public int? InviteResponseId { get; set; }
        public int? ImageId { get; set; }
        public string OcrResult { get; set; }

        public Image Image { get; set; }

        public InviteResponse InviteResponse { get; set; }

        public Invite Invite { get; set; }

        protected OcrResults() { }

        public OcrResults(Image image, 
            string ocrResultJson)
        {
            Image = image;
            OcrResult = ocrResultJson;
        }
    }
}
