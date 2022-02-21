using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Components;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Components
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        #region Properties

        private CustomDatePicker _element { get; set; }
		private Context _context { get; set; }
		private Xamarin.Forms.Color _textColor { get; set; }

		#endregion

		public CustomDatePickerRenderer(Context contex) : base(contex)
		{
			_context = contex;
		}

        #region Override methods

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged(e);

			_element = (CustomDatePicker)this.Element;

			if (Control != null
				&& this.Element != null
				&& !string.IsNullOrEmpty(_element.Image))
			{
				Control.Background = AddPickerStyles(_element.Image);
				Control.SetPadding(22,0,22,0);

				InitElement();
			}
		}
		#endregion

		#region Private methods

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

			_element.DateSelected += Element_DateSelected;
			_element.Focused += Element_Focused;
		}

        private LayerDrawable AddPickerStyles(string imagePath)
		{
			var borderDrawable = ContextCompat.GetDrawable(_context, Resource.Drawable.rounded_corner_white_drawable); 

			Drawable[] layers = { borderDrawable, GetDrawable(imagePath) };
			LayerDrawable layerDrawable = new LayerDrawable(layers);
			layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

			return layerDrawable;
		}

		private BitmapDrawable GetDrawable(string imagePath)
		{
			int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
			var drawable = ContextCompat.GetDrawable(this.Context, resID);
			var bitmap = ((BitmapDrawable)drawable).Bitmap;

			var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 30, 30, true));
			result.Gravity = Android.Views.GravityFlags.ClipVertical | Android.Views.GravityFlags.Right;

			return result;
		}

        #endregion

        #region Handlers

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