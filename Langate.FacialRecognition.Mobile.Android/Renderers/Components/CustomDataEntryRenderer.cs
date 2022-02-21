using System.ComponentModel;
using System.Text.RegularExpressions;
using Android.Content;
using Android.Views;
using Android.Widget;
using Langate.FacialRecognition.Mobile.Extensions;
using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.Droid.Renderers.Components;
using Langate.FacialRecognition.Mobile.Heplers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDataEntry), typeof(CustomDataEntryRenderer))]
namespace Langate.FacialRecognition.Mobile.Droid.Renderers.Components
{
    public class CustomDataEntryRenderer : EntryRenderer
    {
        #region Properties

        private readonly Context _context;
        private int _textValidLenght { get; set; }

        public string _entryValueType { get; set; }

        #endregion

        public CustomDataEntryRenderer(Context context) : base(context)
        {
            _context = context;
        }

        #region Override methods

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                _textValidLenght = (e.NewElement as CustomDataEntry).ValidTextLenght;
                UpdateControlGravity();

                var nativeEditText = (global::Android.Widget.EditText)Control;

                nativeEditText.SetBackgroundResource(Resource.Drawable.rounded_corner_white_drawable);
                Control.SetPadding(22,0,0,0);

                _entryValueType = (e.NewElement as CustomDataEntry).EntryValueType;

                SetCharType(nativeEditText);
            }

            if (e.OldElement != null)
            {
                e.OldElement.TextChanged -= OnTextChanged;
            }

            if (e.NewElement != null)
            {
                e.NewElement.TextChanged += OnTextChanged;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomDataEntry.HorizontalTextAlignmentProperty.PropertyName)
            {
                UpdateControlGravity();
            }
        }

        #endregion

        #region Handlers

        private void OnTextChanged(object o, TextChangedEventArgs args)
        {
            if (Element is CustomDataEntry castedEntry)
            {
                ChangeEntryBorder(castedEntry, args.NewTextValue);
            }
        }

        #endregion

        #region Private methods

        private void UpdateControlGravity()
        {
            if (Element.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.Center)
            {
                Control.Gravity |= GravityFlags.CenterHorizontal;
            }
            if (Element.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.End)
            {
                Control.Gravity |= GravityFlags.End;
            }
            if (Element.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.Start)
            {
                Control.Gravity |= GravityFlags.Start;
            }
        }

        private void ChangeEntryBorder(CustomDataEntry castedEntry, string text)
        {

            castedEntry.TextChangedCommand?.Execute(text);

            var nativeEditText = (global::Android.Widget.EditText)Control;

            if (_textValidLenght == 0)
            {
                return;
            }

            var validationPattern = _entryValueType.GetValidatiomPattern();

            if (text.Length != _textValidLenght || !Regex.IsMatch(text, validationPattern))
            {
                nativeEditText.SetBackgroundResource(Resource.Drawable.rounded_corner_red_drawable);
                return;
            }
            nativeEditText.SetBackgroundResource(Resource.Drawable.rounded_corner_white_drawable);
        }

        private void SetCharType(EditText editTExt)
        {
            if (string.Equals(_entryValueType, Constants.EntryFirstLastNameValueKey))
            {
                editTExt.InputType = Android.Text.InputTypes.TextFlagCapSentences;
            }
        }

        #endregion
    }
}