using System;
using System.Collections.Generic;
using System.Text;

namespace Langate.FacialRecognition.Model
{
    public class SiteLocation
    {
        public SiteLocation()
        {
            Invites = new HashSet<Invite>();
        }

        public int LocationId { get; set; }
        public int SiteId { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string LocationName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string CreatedByLoginType { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Notes { get; set; }
        public bool IsDisplayIdNumOnCert { get; set; }
        public bool? IsTrackConsent { get; set; }
        public bool? IsPartialIdentifiers { get; set; }
        public bool? IsUseBiometrics { get; set; }
        public int? MaxDosedAmount { get; set; }
        public int? MaxDosedAmountWindow { get; set; }
        public string MaxDosedAmountWindowUnit { get; set; }
        public bool IsIdentityCheckAllowed { get; set; }

        public virtual ICollection<Invite> Invites { get; set; }
    }
}
