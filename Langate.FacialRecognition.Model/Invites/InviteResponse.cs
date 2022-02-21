using System;
using System.Collections.Generic;

namespace Langate.FacialRecognition.Model
{
    public class InviteResponse: Entity
    {
        public int InviteResponseId { get; set; }
        public int InviteId { get; set; }
        public int ProtocolId { get; set; }
        public string SubjectNumber { get; set; }
        public int? ImageFrontalId { get; set; }
        public int? ImageLeftId { get; set; }
        public int? ImageRightId { get; set; }
        public int? ImageDocumentId { get; set; }
        public int ApiCallId { get; set; }
        public string PartialIdentifiers { get; set; }
        public int? Status { get; set; }
        public int? SubVerificationId { get; set; }

        public virtual Invite Invite { get; set; }
        public virtual SponsorProtocol Protocol { get; set; }

        public int? SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public Image ImageFrontal { get; set; }

        public Image ImageLeft { get; set; }

        public Image ImageRight { get; set; }

        public Image ImageDocument { get; set; }



        public virtual IList<FaceRecognition> FaceRecognitions { get; set; } = new List<FaceRecognition>();

        protected InviteResponse()
        {

        }

        public InviteResponse(Invite invite)
        {
            if (invite == null)
            {
                throw new ArgumentNullException(nameof(invite));
            }

            Invite = invite;
        }
    }
}
