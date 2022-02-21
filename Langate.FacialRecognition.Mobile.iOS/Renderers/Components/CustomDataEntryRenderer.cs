using CoreGraphics;
using Langate.FacialRecognition.Mobile.Components;
using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.iOS.Renderers.Components;
using System.Text.RegularExpressions;
using Langate.FacialRecognition.Mobile.Extensions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDataEntry), typeof(CustomDataEntryRenderer))]
namespace Langate.FacialRecognition.Mobile.iOS.Renderers.Components
{
    public class CustomDataEntryRenderer : EntryRenderer
    {
        #region Fields

        private readonly CGColor _invalidColor = UIColor.FromRGB(255, 22, 0).CGColor;
        private readonly CGColor _validColor = UIColor.FromRGB(213, 213, 214).CGColor;

        #endregion

        #region Properties

        private int _textValidLenght { get; set; }
        public string _entryValueType { get; set; }

        #endregion

        #region Overrides methods

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                _textValidLenght = (e.NewElement as CustomDataEntry).ValidTextLenght;
                _entryValueType = (e.NewElement as CustomDataEntry).EntryValueType;

                SetCharType();

                InitEntryBorder();
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

        #endregion

        #region Private methods

        private void OnTextChanged(object o, TextChangedEventArgs args)
        {
            if (Element is CustomDataEntry castedEntry)
            {
                ChangeEntryBorder(castedEntry, args.NewTextValue);
            }
        }

        private void InitEntryBorder()
        {
            Control.BorderStyle = UITextBorderStyle.RoundedRect;
            Control.Layer.BorderColor = _validColor;
            Control.Layer.CornerRadius = 5f;
            Control.Layer.BorderWidth = 1f;

            UIView leftView = new UIView(new CGRect(0, 0, 2, 10));
            Control.LeftView = leftView;
            Control.LeftViewMode = UITextFieldViewMode.Always;
        }

        private void ChangeEntryBorder(CustomDataEntry castedEntry, string text)
        {
            castedEntry.TextChangedCommand?.Execute(text);

            if (_textValidLenght == 0)
            {
                return;
            }

            var validationPattern = _entryValueType.GetValidatiomPattern();

            if (text.Length != _textValidLenght || !Regex.IsMatch(text, validationPattern))
            {
                Control.Layer.BorderColor = _invalidColor;
                return;
            }
            Control.Layer.BorderColor = _validColor;
        }

        private void SetCharType()
        {
            if (string.Equals(_entryValueType, Constants.EntryFirstLastNameValueKey))
            {
                Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
            }
        }

        #endregion
    }
}