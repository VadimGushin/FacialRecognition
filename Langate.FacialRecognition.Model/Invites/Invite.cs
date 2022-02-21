using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Langate.FacialRecognition.Model
{
    public class Invite : Entity
    {
        public virtual int InviteId { get; set; }
        public string SubjectNumber { get; set; }
        public int? CohortId { get; set; }
        public int UserId { get; set; }
        public string IpAdress { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Deeplink { get; set; }
        public string UserType { get; set; }
        public int SubVerificationId { get; set; }
        public int VerificationStatus { get; set; }

        public int LocationId { get; set; }
        public virtual SiteLocation Location { get; set; }

        public int ProtocolId { get; set; }
        public virtual SponsorProtocol Protocol { get; set; }

        public int? SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int? TokenId { get; set; }
        public virtual Token Token { get; set; }

        public virtual ICollection<InviteResponse> InviteResponses { get; set; } = new List<InviteResponse>();
        public virtual IList<FaceRecognition> FaceRecognitions { get; set; } = new List<FaceRecognition>();
        public virtual IList<OcrResults> OcrResults { get; set; } = new List<OcrResults>();

    }
}