using System.Collections.Generic;

namespace Langate.FacialRecognition.Model
{
    public class Person : Entity
    {
        public int PersonId { get; set; }
        public string AzurePersonId { get; set; }
        public string AzurePersonGroupId { get; set; }
        public int SubjectId { get; set; }

        public virtual IList<FaceRecognition> FaceRecognitions { get; set; } = new List<FaceRecognition>();
    }
}
