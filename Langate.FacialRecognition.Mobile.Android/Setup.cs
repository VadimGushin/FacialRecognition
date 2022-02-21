using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace Langate.FacialRecognition.Mobile.Droid
{
    public class Setup : MvxFormsAndroidSetup<CoreApp, App>
    {
        protected override IMvxApplication CreateApp()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Helper")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            return base.CreateApp();
        }
    }
}