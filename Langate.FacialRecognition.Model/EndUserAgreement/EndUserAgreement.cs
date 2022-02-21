using System;
using System.Collections.Generic;
using System.Text;

namespace Langate.FacialRecognition.Model
{
    public class EndUserAgreement : Entity
    {
        public int EndUserAgreementId { get; set; }
        public int AgreementId { get; set; }
        public string Version { get; set; }
        public int ProtocolId { get; set; }
        public string Text { get; set; }
    }
}