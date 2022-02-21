using System;
using System.Collections.Generic;


namespace Langate.FacialRecognition.Model
{
    public partial class SponsorProtocol
    {
        public SponsorProtocol()
        {
            InviteResponses = new HashSet<InviteResponse>();
            Invites = new HashSet<Invite>();
        }

        public int ProtocolId { get; set; }
        public int? SponsorId { get; set; }
        public string ProtocolName { get; set; }
        public string ProtocolNumber { get; set; }
        public string Uin { get; set; }
        public string Price { get; set; }
        public string Instructions { get; set; }
        public int? VisitNumber { get; set; }
        public DateTime? SubmitDate { get; set; }
        public bool? IsActive { get; set; }
        public string AddedByLoginType { get; set; }
        public int? AddedBy { get; set; }
        public DateTime? LastDosingDate { get; set; }
        public string ProtocolDesc { get; set; }
        public string ProtocolFullName { get; set; }
        public string IsProductBiologic { get; set; }
        public int? BioWindow { get; set; }
        public string HalfLifeDrug { get; set; }
        public string AgeRange { get; set; }
        public string HalfLivesLastProtocol { get; set; }
        public string CompoundName { get; set; }
        public string SubjectDosedOnSameDate { get; set; }
        public int? LengthOfProtocol { get; set; }
        public string LengthUnit { get; set; }
        public string WashoutStudy { get; set; }
        public string DoseDateWindow { get; set; }
        public DateTime? ScreenDate { get; set; }
        public DateTime? AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string Country { get; set; }
        public decimal? HalfLifeDrugNo { get; set; }
        public string HalfLifeDrugUnit { get; set; }
        public int? WashoutStudyNo { get; set; }
        public string WashoutStudyUnit { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedById { get; set; }
        public string UpdatedByLoginType { get; set; }
        public bool? IsArchive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsDosingCohort { get; set; }
        public int? NumberOfCohorts { get; set; }
        public int? NumberOfDays { get; set; }
        public DateTime? FirstDosingDate { get; set; }
        public decimal? HalfLifeLastProtocol { get; set; }
        public string HalfLifeLastProUnit { get; set; }
        public string BioWindowUnit { get; set; }
        public bool? ProtocolType { get; set; }
        public bool? TrackVisits { get; set; }
        public int? AdmissionDayVisit { get; set; }
        public int? RandomizationVisit { get; set; }
        public int? FirstDosingVisit { get; set; }
        public int? LastDosingVisit { get; set; }
        public int? DischargeDayVisit { get; set; }
        public int? DosingDateWindow { get; set; }
        public DateTime? StudyStartDate { get; set; }
        public int? NumberDaysFirstToLast { get; set; }
        public string IpAddress { get; set; }
        public bool? IsAutoSf { get; set; }
        public int? SfTimePoint { get; set; }
        public string SfUnit { get; set; }
        public int? Reminders { get; set; }
        public string ReminderEmailText { get; set; }
        public string ReminderSmsText { get; set; }
        public string DosingDateWindowUnit { get; set; }
        public bool? IsSubjectNoMandatory { get; set; }
        public string CohortEntitled { get; set; }
        public string WashoutStudyFrom { get; set; }
        public string WashoutStudyTo { get; set; }
        public DateTime? LastBloodDrawDate { get; set; }
        public int? LastBloodDrawDayVisit { get; set; }
        public bool IsMedicareSecondPayerActEligible { get; set; }
        public short VerificationPlusOptions { get; set; }
        public bool? IsAutocomplete { get; set; }
        public bool? IsCanExtend { get; set; }
        public bool? IsEnableProtocolAsFormOfId { get; set; }
        public bool? IsScreenFailAnotherSite { get; set; }
        public int? BuiltFromFormId { get; set; }
        public string NotificationEmails { get; set; }
        public bool? HaveQualPeriod { get; set; }
        public bool? NotifyQualPeriod { get; set; }
        public short? QualDaysPriorFd { get; set; }
        public int? RolloverProtocolId { get; set; }
        public bool? IsRadioactiveIsotope { get; set; }
        public int? RadioactiveWindow { get; set; }
        public string RadioactiveWindowsUnit { get; set; }
        public bool? IsGrowthHormone { get; set; }
        public int? GrowthHormoneWindow { get; set; }
        public string GrowthHormoneWindowUnit { get; set; }
        public bool? IsMinorStudy { get; set; }
        public bool? IsSubjectRegistry { get; set; }
        public bool? IsBiometrics { get; set; }
        public bool? IsFullIdentifiers { get; set; }
        public bool? IsKinase { get; set; }
        public int? KinaseWindow { get; set; }
        public string KinaseWindowUnit { get; set; }
        public bool? IsConsentRequired { get; set; }
        public bool? IsMultiYearConsentRequired { get; set; }
        public string SubjectNumberLength { get; set; }
        public bool? IsHealthyCondOnlyAllowed { get; set; }
        public bool? IsHealthCondExclusionary { get; set; }
        public string OtherExclusionaryHealthCondition { get; set; }
        public bool? IsPrescreenStudy { get; set; }
        public string ExclusionaryProtocolNumbers { get; set; }
        public bool? NotifyAdminOnValerts { get; set; }
        public string SponsorEmailList { get; set; }
        public bool? IsCheckinStudy { get; set; }
        public bool? IsDualScreenAllowed { get; set; }
        public int? AdvocacyGroupDonationAmount { get; set; }
        public bool? IsDirectConnect { get; set; }
        public bool? IsEnterEmergencyContacts { get; set; }
        public int? MaxDosedAmount { get; set; }
        public int? MaxDosedAmountWindow { get; set; }
        public string MaxDosedAmountWindowUnit { get; set; }
        public short DonationStudyType { get; set; }
        public bool IsSpecimenStudy { get; set; }
        public bool IsDisplaySubNumOnly { get; set; }
        public short CertificateViewType { get; set; }
        public bool IsHealthyVolunteersCohort { get; set; }
        public bool IsObservationalStudy { get; set; }
        public bool? IsBloodDrawRequired { get; set; }
        public bool IsPriorSpecimenStudiesAllowed { get; set; }
        public bool IsMultiSiteStudy { get; set; }
        public string SponsorIssuesContactEmailList { get; set; }
        public short PartnerTypeId { get; set; }
        public short ConsentTypeId { get; set; }
        public short AllowIfPastTrainedMonths { get; set; }
        public int AllowIfPastTrainedSponsor { get; set; }
        public bool IsHideFromVerifyScreen { get; set; }
        public string MaxDosedAmountSpecificStudies { get; set; }
        public bool IsSameStudyParticipationAllowed { get; set; }
        public string AdminComment { get; set; }
        public int? IdentityCheckNumber { get; set; }
        public bool IsGeotargeting { get; set; }
        public int Distance { get; set; }
        public bool IsFacialRecognitionStudy { get; set; }

        public virtual ICollection<InviteResponse> InviteResponses { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
    }
}
