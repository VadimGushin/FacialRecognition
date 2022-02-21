using CoreGraphics;
using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Components;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Components
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        #region Fields

		private CustomDatePicker _element { get; set; }

		private readonly CGColor _validColor = UIColor.FromRGB(213, 213, 214).CGColor;
		private Color _textColor { get; set; }

		#endregion

		#region Override methods
		protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			_element = (CustomDatePicker)this.Element;

			if (this.Control != null
				&& this.Element != null
				&& !string.IsNullOrEmpty(_element.Image))
			{
				InitEntryBorder(_element);
				InitElement();
			}
		}

        #endregion

        #region Private Methods

        private void InitEntryBorder(CustomDatePicker element)
		{
			var downArrow = UIImage.FromBundle(element.Image);
			Control.RightViewMode = UITextFieldViewMode.Always;
			Control.RightView = new UIImageView(downArrow);

			Control.BorderStyle = UITextBorderStyle.RoundedRect;
			Control.Layer.BorderColor = _validColor;
			Control.Layer.CornerRadius = 5f;
			Control.Layer.BorderWidth = 1f;

			UIView leftView = new UIView(new CGRect(0, 0, 5, 20));
			Control.LeftView = leftView;
			Control.LeftViewMode = UITextFieldViewMode.Always;

			Control.Layer.MasksToBounds = true;
		}

		private void InitElement()
		{
			_element.Date = default;
			_element.MinimumDate = _element.StartDate;
			_element.MaximumDate = _element.EndDate;

			_textColor = _element.TextColor;

			if (!_element.HaveInitDate)
			{
				_element.TextColor = Xamarin.Forms.Color.White;
			}
			if (_element.HaveInitDate)
			{
				_element.Date = _element.InitDate == DateTime.MinValue
					? DateTime.Now
					: _element.InitDate;
			}

			_element.Focused += Element_Focused;
			_element.DateSelected += Element_DateSelected;
		}

		#endregion

		#region Event Handlers

		private void Element_Focused(object sender, FocusEventArgs e)
		{
			if (!_element.HaveInitDate)
			{
				_element.Date = DateTime.Now;
			}
			_element.Focused -= Element_Focused;
		}

		private void Element_DateSelected(object sender, DateChangedEventArgs e)
		{
			_element.TextColor = _textColor;
			_element.DateSelected -= Element_DateSelected;
		}

		#endregion
	}
}