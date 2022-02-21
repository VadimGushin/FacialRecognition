namespace Langate.FacialRecognition.Model
{
    public class FaceRecognition : Entity
    {
        public int FaceRecognitionId { get; set; }
        public string FaceId { get; set; }
        public double Score { get; set; }
        public string AzurePersonGroupId { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }

        public int? InviteId { get; set; }
        public Invite Invite { get; set; }
        
        public int? InviteResponseId { get; set; }
        public InviteResponse InviteResponse { get; set; }

        public int? PersonId { get; set; }
        public Person Person { get; set; }

        protected FaceRecognition() { }

        public FaceRecognition(int imageId)
        {
            ImageId = imageId;
        }
    }
}
