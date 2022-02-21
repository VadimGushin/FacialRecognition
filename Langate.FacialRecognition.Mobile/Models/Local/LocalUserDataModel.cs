using System;

namespace Langate.FacialRecognition.Mobile.Models.Local
{
    public class LocalUserDataModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string IdType { get; set; }
        public string IdValue { get; set; }
        public bool IsFull { get; set; }
        public bool IsRetaked { get; set; }

        public LocalUserDataModel()
        {
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            Gender = string.Empty;
            DateOfBirth = default;
            IdType = string.Empty;
            IdValue = string.Empty;
            IsFull = false;
            IsRetaked = false;
        }
    }
}
