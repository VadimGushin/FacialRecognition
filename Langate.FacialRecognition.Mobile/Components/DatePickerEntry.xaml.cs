using Langate.FacialRecognition.Mobile.Heplers;
using Langate.FacialRecognition.Mobile.Resources.Strings;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Langate.FacialRecognition.Mobile.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerEntry : ContentView
    {
        #region Constants

        private const int _minYearValue = Constants.MinValidYear;
        private const int _validationYearLenght = 4;
        private const int _defaultMaxDate = 31;
        private const int _defaultYear = 2020;

        #endregion

        #region Variables

        private int _selectedYearValue;
        private int _selectedMonthIndex;
        private int _selectedDateValue;
        private static DateTime _initDateTime;
        public event EventHandler<DateTime> SelectedDateChanged;

        #endregion

        public DatePickerEntry()
        {
            InitializeComponent();

            InitControls();
        }

        #region Properties

        private string _selectedMonth { get; set; }
        private string _selectedDate { get; set; }
        private string _selectedYear { get; set; }

        /// <summary>
        /// Selected date value of DatePickerEntry
        /// </summary>
        public DateTime SelectedDOB
        {
            get { return (DateTime)base.GetValue(SelectedDOBProperty); }
            set { base.SetValue(SelectedDOBProperty, value); }
        }

        #endregion

        #region BindableProperties

        public static readonly BindableProperty SelectedDOBProperty = BindableProperty.Create(
                                                 propertyName: nameof(SelectedDOB),
                                                 returnType: typeof(DateTime),
                                                 declaringType: typeof(DatePickerEntry),
                                                 defaultValue: default,
                                                 defaultBindingMode: BindingMode.TwoWay,
                                                 propertyChanged: InitDOBPropertyChanged);

        #endregion

        #region Property Handlers

        private static void InitDOBPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (DatePickerEntry)bindable;
            _initDateTime = (DateTime)newValue;
            if (_initDateTime > DateTime.MinValue)
            {
                control.month.SelectedItem = new DateTimeFormatInfo().GetMonthName(_initDateTime.Month).ToString();
                control.year.Text = _initDateTime.Year.ToString();
                control.date.Text = _initDateTime.Day.ToString();
                control.error.IsVisible = false;
                return;
            }
        }

        #endregion

        #region Private Methods

        private void InitControls()
        {
            this.month.SelectedItem = string.Empty;
            this.month.SetPlaceholderText(Strings.Month);
            this.error.IsVisible = true;
            this.error.Text = Constants.DOBEnterValue;
            SetItemSource();
            this.month.SelectedItemChanged += (s, e) => 
            {
                if (e.NewItemIndex < 0)
                {
                    return;
                }
                _selectedMonth = e.NewItem.ToString();
                _selectedMonthIndex = e.NewItemIndex + 1;
                ValidateDate();
            };
            this.date.TextChanged += (s, e) => 
            {
                _selectedDate = e.NewTextValue;
                ValidateDate();
            };
            this.year.TextChanged += (s, e) => 
            {
                _selectedYear = e.NewTextValue;
                ValidateDate();
            };
        }

        private void SetItemSource()
        {
            CultureInfo usEnglish = new CultureInfo("en-US");
            DateTimeFormatInfo englishInfo = usEnglish.DateTimeFormat;
            var monthSource = englishInfo.MonthNames.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            month.ItemsSource = monthSource;
        }

        private void ValidateDate()
        {
            if (ParseDateValue()
                && ParseMonthValue()
                && ParseYearValue()
                && CheckDateIsMoreThenNow())
            {
                SelectedDOB = new DateTime(_selectedYearValue, _selectedMonthIndex, _selectedDateValue);
                SelectedDateChanged?.Invoke(this, SelectedDOB);
                this.error.IsVisible = false;
                return;
            }
            SelectedDOB = DateTime.MinValue;
            this.error.IsVisible = true;
        }

        private bool ParseDateValue()
        {
            if (string.IsNullOrWhiteSpace(_selectedDate))
            {
                this.error.Text = Constants.DOBEnterDate;
                return false;
            }

            var parsResult = int.TryParse(_selectedDate, out _selectedDateValue);

            if (!parsResult || _selectedDateValue <= 0 || _selectedDateValue > _defaultMaxDate)
            {
                this.date.Text = this.date.Text.Length > 1 ? this.date.Text.Substring(0, 1) : string.Empty;
                this.error.Text = Constants.DOBInvalidDate;
                return false;
            }
            return TryCheckMonthAndYear();
        }

        private bool ParseMonthValue()
        {
            if (string.IsNullOrWhiteSpace(_selectedMonth))
            {
                this.error.Text = Constants.DOBEnterMonth;
                return false;
            }
            return true;
        }

        private bool ParseYearValue()
        {
            if (string.IsNullOrWhiteSpace(_selectedYear))
            {
                _selectedYearValue = 0;
                this.error.Text = Constants.DOBEnterYear;
                return false;
            }
            var parsResult = int.TryParse(_selectedYear, out _selectedYearValue);
            if (!parsResult || _selectedYear.Length != _validationYearLenght
                || _selectedYearValue < _minYearValue || _selectedYearValue > DateTime.Now.Year)
            {
                this.year.Text = this.year.Text.Length == 4 ? this.year.Text.Substring(0, 3) : this.year.Text;
                this.error.Text = Constants.DOBInvalidYear;
                return false;
            }

            return ParseDateValue();
        }

        private bool TryCheckMonthAndYear()
        {
            if (string.IsNullOrWhiteSpace(_selectedMonth))
            {
                this.error.Text = Constants.DOBEnterMonth;
                return false;
            }
            if (!string.IsNullOrWhiteSpace(_selectedMonth))
            {
                return TryCheckYear();
            }
            if (string.IsNullOrWhiteSpace(_selectedYear))
            {
                this.error.Text = Constants.DOBEnterYear;
                return false;
            }
            return true;
        }

        private bool TryCheckYear()
        {
            var maxDaysInMonthOfDefaultYear = DateTime.DaysInMonth(
                (_selectedYearValue < _minYearValue || _selectedYearValue > DateTime.Now.Year
                ? _defaultYear : _selectedYearValue),
                _selectedMonthIndex);

            if (_selectedDateValue > maxDaysInMonthOfDefaultYear)
            {
                this.date.Text = string.Empty;
                this.error.Text = Constants.DOBEnterValidDate;
                return false;
            }
            return true;
        }

        private bool CheckDateIsMoreThenNow()
        {
            if (new DateTime(_selectedYearValue, _selectedMonthIndex, _selectedDateValue) > DateTime.Now)
            {
                this.error.Text = Constants.DOBEnterValidYear;
                this.year.Text = this.year.Text.Length == 4 ? this.year.Text.Substring(0, 3) : this.year.Text;
                int.TryParse(this.year.Text, out _selectedYearValue);
                return false;
            }
            return true;
        }

        #endregion
    }
}