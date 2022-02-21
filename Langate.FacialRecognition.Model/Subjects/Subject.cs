using System;
using System.Collections.Generic;
using System.Text;

namespace Langate.FacialRecognition.Model
{
    public partial class Subject
    {
        public Subject()
        {
            InverseOriginalSubject = new HashSet<Subject>();
            InviteResponses = new HashSet<InviteResponse>();
            Invites = new HashSet<Invite>();
        }

        public int SubjectId { get; set; }
        public int? AddedById { get; set; }
        public string LoginType { get; set; }
        public string FirstName { get; set; }
        public string LastNamePrefix { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FirstNamePartial { get; set; }
        public string LastNamePartial { get; set; }
        public int DobDay { get; set; }
        public int DobMonth { get; set; }
        public int DobYear { get; set; }
        public decimal? Bmi { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string EmailId { get; set; }
        public string Zipcode { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string ImHandle { get; set; }
        public string ImType { get; set; }
        public string Gender { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? SubmitDate { get; set; }
        public string OtherDisease { get; set; }
        public string DiseaseDesc { get; set; }
        public string DiseaseDetail { get; set; }
        public string TravelWill { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentMiddleName { get; set; }
        public string ParentLastName { get; set; }
        public DateTime? ParentDob { get; set; }
        public string ParentGender { get; set; }
        public string Relationship { get; set; }
        public bool Isminor { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedById { get; set; }
        public string UpdatedByLoginType { get; set; }
        public string OptLastName { get; set; }
        public string IpAddress { get; set; }
        public string Comments { get; set; }
        public string SubjectNumberBk { get; set; }
        public int? OriginalSubjectId { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string MedicareNumber { get; set; }
        public string DeletedByLoginType { get; set; }
        public int? DeletedById { get; set; }
        public bool? IsFingerprintOnFile { get; set; }
        public bool IsFakeIdSubject { get; set; }

        public virtual Subject OriginalSubject { get; set; }
        public virtual ICollection<Subject> InverseOriginalSubject { get; set; }
        public virtual ICollection<InviteResponse> InviteResponses { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
    }
}
