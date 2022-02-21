using System.Text.RegularExpressions;

namespace Langate.FacialRecognition.Mobile.Extensions
{
    public static class ValidationEntryExtension
    {
        public static bool IsValid(this string text, string type, int validLenght)
        {
            var validationPattern = type.GetValidatiomPattern();
            if (string.IsNullOrWhiteSpace(text)
                || text.Length != validLenght
                || !Regex.IsMatch(text, validationPattern))
            {
                return false;
            }
            return true;
        }
    }
}
