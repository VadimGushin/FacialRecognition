using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.Text;
using Xamarin.Forms;

namespace Langate.FacialRecognition.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        #region Override Methods

        protected override void OnStart()
        {
            base.OnStart();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("android=7157ac26-8716-423b-8ef4-10cb306ea5d1;");
            stringBuilder.Append("ios=4f8784b9-9066-4847-a88b-0f51dec3a994");

            AppCenter.Start(stringBuilder.ToString(),
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        #endregion
    }
}
