using CoreGraphics;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Components;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ProgressBar), typeof(CustomProgressBarRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Components
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        #region Variables

        private readonly nfloat _cornerRadius = 2f;
        private readonly UIColor _trackColor = UIColor.FromRGB(233, 233, 240);
        private readonly UIColor _tintColor = UIColor.FromRGB(49, 105, 246);

        #endregion

        #region Override Methods

        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.ProgressTintColor = _tintColor;
                Control.TrackTintColor = _trackColor;

                Control.ClipsToBounds = true;
                Control.Subviews[1].ClipsToBounds = true;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Control.Transform = CGAffineTransform.MakeScale(1f, 3f);
            Control.Layer.CornerRadius = _cornerRadius;
            Control.Layer.Sublayers[1].CornerRadius = _cornerRadius;
        }

        #endregion
    }
}