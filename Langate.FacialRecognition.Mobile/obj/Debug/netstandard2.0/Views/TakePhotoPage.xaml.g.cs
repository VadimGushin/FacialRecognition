//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::Xamarin.Forms.Xaml.XamlResourceIdAttribute("Langate.FacialRecognition.Mobile.Views.TakePhotoPage.xaml", "Views/TakePhotoPage.xaml", typeof(global::Langate.FacialRecognition.Mobile.Views.TakePhotoPage))]

namespace Langate.FacialRecognition.Mobile.Views {
    
    
    [global::Xamarin.Forms.Xaml.XamlFilePathAttribute("Views/TakePhotoPage.xaml")]
    public partial class TakePhotoPage : global::Langate.FacialRecognition.Mobile.Views.Base.BaseContentPage<global::Langate.FacialRecognition.Mobile.ViewModels.TakePhotoViewModel> {
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Langate.FacialRecognition.Mobile.Components.CustomNavigationBar navigationBar;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.Grid mainGrid;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.RelativeLayout cameraLayout;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.ImageButton btnCreate;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.RelativeLayout bottomLayout;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private global::Xamarin.Forms.ImageButton btnCameraMode;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
        private void InitializeComponent() {
            global::Xamarin.Forms.Xaml.Extensions.LoadFromXaml(this, typeof(TakePhotoPage));
            navigationBar = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Langate.FacialRecognition.Mobile.Components.CustomNavigationBar>(this, "navigationBar");
            mainGrid = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.Grid>(this, "mainGrid");
            cameraLayout = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.RelativeLayout>(this, "cameraLayout");
            btnCreate = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ImageButton>(this, "btnCreate");
            bottomLayout = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.RelativeLayout>(this, "bottomLayout");
            btnCameraMode = global::Xamarin.Forms.NameScopeExtensions.FindByName<global::Xamarin.Forms.ImageButton>(this, "btnCameraMode");
        }
    }
}