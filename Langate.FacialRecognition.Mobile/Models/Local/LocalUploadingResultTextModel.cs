
namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalUploadingResultTextModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BottomText { get; set; }
        public string Url { get; set; }
        public int State { get; set; }

        public LocalUploadingResultTextModel()
        {
            Title = string.Empty;
            Description = string.Empty;
            BottomText = string.Empty;
            Url = string.Empty;
            State = 0;
        }

        public LocalUploadingResultTextModel(
            string title,
            string description,
            string bottomText,
            string url,
            int state)
        {
            Title = title;
            Description = description;
            BottomText = bottomText;
            Url = url;
            State = state;
        }
    }
}
