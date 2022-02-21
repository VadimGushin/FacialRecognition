
namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalContinueSubmissionItemModel
    {
        public string Token { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Study { get; set; }
        public bool IsSelected { get; set; }
        public bool IsCurrent { get; set; }
    }
}
