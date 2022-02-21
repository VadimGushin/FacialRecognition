using Langate.FacialRecognition.Mobile.Heplers;

namespace Langate.FacialRecognition.Mobile.Extensions
{
    public static class EntryValueValidationPattersExtension
    {
        public static string GetValidatiomPattern(this string entryType)
        {
            string validationPattern = string.Empty;
            if (string.Equals(entryType, Constants.EntryFirstLastNameValueKey))
            {
                validationPattern = Constants.FirstLastNameValidationPattern;
            }
            if (string.Equals(entryType, Constants.EntryIdValueKey))
            {
                validationPattern = Constants.IdValidationPattern;
            }
            if (string.Equals(entryType, Constants.EntryMiddleNameValueKey))
            {
                validationPattern = Constants.MiddleNameValidationPattern;
            }
            if (string.Equals(entryType, Constants.EntryNumberValueKey))
            {
                validationPattern = Constants.NumberValidationPattern;
            }
            if (string.IsNullOrWhiteSpace(entryType))
            {
                validationPattern = Constants.DefaultValidationPattern;
            }
            return validationPattern;
        }
    }
}
