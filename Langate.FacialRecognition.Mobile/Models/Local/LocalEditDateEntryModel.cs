using System;
using System.Collections.Generic;
using System.Text;

namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalEditDateEntryModel
    {
        public string ImageSource { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndtDate { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
