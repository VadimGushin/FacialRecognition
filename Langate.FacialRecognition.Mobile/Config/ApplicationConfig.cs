using Langate.FacialRecognition.Mobile.Models.Enums;

namespace Langate.FacialRecognition.Mobile.Config
{
    public static class ApplicationConfig
    {
        #region Properties & Variables
        private static BuildConfiguration _build { get; set; } = BuildConfiguration.Live;
        public static string SiteApiUrl { get; private set; }
        #endregion

        #region Constructors
        static ApplicationConfig()
        {
            if (_build == BuildConfiguration.Development)
            {
                ConfigDevelopment();
            }
            if (_build == BuildConfiguration.Live)
            {
                ConfigApiRelease();
            }
        }
        #endregion

        #region Private Methods
        private static void ConfigDevelopment()
        {
            //SiteApiUrl = "https://face-recognition-langate.azurewebsites.net/";
            //SiteApiUrl = "https://facial-recognition-hxdv6s.azurewebsites.net/";
            SiteApiUrl = "https://fr.vctrials.com/";
        }
        private static void ConfigApiRelease()
        {
            //SiteApiUrl = "https://face-recognition-langate.azurewebsites.net/";
            SiteApiUrl = "https://fr.vctrials.com/";
        }
        #endregion
    }
}
