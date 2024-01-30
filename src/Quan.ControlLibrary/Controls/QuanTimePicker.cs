using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Quan.ControlLibrary.Helpers;

namespace Quan.ControlLibrary.Controls;

[TemplatePart(Name = QuanTextBoxName, Type = typeof(QuanTextBox))]
[TemplatePart(Name = PopupName, Type = typeof(Popup))]
[TemplatePart(Name = PopupHoursListBoxName, Type = typeof(Popup))]
[TemplatePart(Name = PopupMinutesListBoxName, Type = typeof(Popup))]
[TemplatePart(Name = PopupSecondsListBoxName, Type = typeof(Popup))]
public class QuanTimePicker : Control
{
    #region Enums

    #endregion

    #region Properties

    private const string QuanTextBoxName = "PART_QuanTextBox";

    private const string PopupName = "PART_Popup";

    private const string PopupHoursListBoxName = "PART_Popup_HoursListBox";

    private const string PopupMinutesListBoxName = "PART_Popup_MinutesListBox";

    private const string PopupSecondsListBoxName = "PART_Popup_SecondsListBox";

    private const string TimeFormat = @"hh\:mm\:ss";

    private QuanTextBox _quanTextBox;

    private Popup _popup;

    private ListBox _popupHoursListBox;

    private ListBox _popupMinutesListBox;

    private ListBox _popupSecondsListBox;

    #endregion

    #region Dependency Properties

    #region SelectedTime

    public TimeSpan SelectedTime
    {
        get => (TimeSpan)GetValue(SelectedTimeProperty);
        set => SetValue(SelectedTimeProperty, value);
    }

    public static readonly DependencyProperty SelectedTimeProperty =
        DependencyProperty.Register(
            nameof(SelectedTime),
            typeof(TimeSpan),
            typeof(QuanTimePicker),
            new UIPropertyMetadata(TimeSpan.Zero, OnSelectedTimeChanged));

    private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not QuanTimePicker { IsLoaded: true } timePicker)
        {
            return;
        }


        if (e.NewValue is not TimeSpan timeSpan)
        {
            return;
        }

        var timeString = timeSpan.ToString(TimeFormat);

        timePicker._quanTextBox.Text = timeString;
    }

    #endregion

    #region HourNumbers

    public IEnumerable<int> HourNumbers
    {
        get => (IEnumerable<int>)GetValue(HourNumbersProperty);
        set => SetValue(HourNumbersProperty, value);
    }

    public static readonly DependencyProperty HourNumbersProperty =
        DependencyProperty.Register(
            nameof(HourNumbers),
            typeof(IEnumerable<int>),
            typeof(QuanTimePicker),
            new PropertyMetadata(Enumerable.Range(0, 24)));

    #endregion

    #region MinutesNumbers

    public IEnumerable<int> MinutesNumbers
    {
        get => (IEnumerable<int>)GetValue(MinutesNumbersProperty);
        set => SetValue(MinutesNumbersProperty, value);
    }

    public static readonly DependencyProperty MinutesNumbersProperty =
        DependencyProperty.Register(
            nameof(MinutesNumbers),
            typeof(IEnumerable<int>),
            typeof(QuanTimePicker),
            new PropertyMetadata(Enumerable.Range(0, 60)));

    #endregion

    #region SecondsNumbers

    public IEnumerable<int> SecondsNumbers
    {
        get => (IEnumerable<int>)GetValue(SecondsNumbersProperty);
        set => SetValue(SecondsNumbersProperty, value);
    }

    public static readonly DependencyProperty SecondsNumbersProperty =
        DependencyProperty.Register(
            nameof(SecondsNumbers),
            typeof(IEnumerable<int>),
            typeof(QuanTimePicker),
            new PropertyMetadata(Enumerable.Range(0, 60)));

    #endregion

    #endregion

    #region Constructor

    static QuanTimePicker()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanTimePicker), new FrameworkPropertyMetadata(typeof(QuanTimePicker)));
    }

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
        if (_quanTextBox != null)
        {
            _quanTextBox.KeyUp -= QuanTextBoxOnKeyUp;
            _quanTextBox.PreviewMouseUp -= QuanTextBoxOnPreviewMouseButtonUp;
        }

        if (_popup != null)
        {
            _popup.Opened -= PopupOnOpened;
        }

        if (_popupHoursListBox != null)
        {
            _popupHoursListBox.SelectionChanged -= PopupHoursListBoxOnSelectionChanged;
        }

        if (_popupMinutesListBox != null)
        {
            _popupMinutesListBox.SelectionChanged -= PopupMinutesListBoxOnSelectionChanged;
        }

        if (_popupSecondsListBox != null)
        {
            _popupSecondsListBox.SelectionChanged -= PopupSecondsListBoxOnSelectionChanged;
        }

        base.OnApplyTemplate();

        _quanTextBox = GetTemplateChild(QuanTextBoxName) as QuanTextBox;
        _popup = GetTemplateChild(PopupName) as Popup;
        _popupHoursListBox = GetTemplateChild(PopupHoursListBoxName) as ListBox;
        _popupMinutesListBox = GetTemplateChild(PopupMinutesListBoxName) as ListBox;
        _popupSecondsListBox = GetTemplateChild(PopupSecondsListBoxName) as ListBox;

        if (_popup != null)
        {
            _popup.Opened += PopupOnOpened;
        }

        if (_quanTextBox != null)
        {
            _quanTextBox.Text = SelectedTime.ToString(TimeFormat);
            _quanTextBox.KeyUp += QuanTextBoxOnKeyUp;
            _quanTextBox.PreviewMouseUp += QuanTextBoxOnPreviewMouseButtonUp;
        }

        if (_popupHoursListBox != null)
        {
            _popupHoursListBox.SelectionChanged += PopupHoursListBoxOnSelectionChanged;
        }

        if (_popupMinutesListBox != null)
        {
            _popupMinutesListBox.SelectionChanged += PopupMinutesListBoxOnSelectionChanged;
        }


        if (_popupSecondsListBox != null)
        {
            _popupSecondsListBox.SelectionChanged += PopupSecondsListBoxOnSelectionChanged;
        }

    }



    #endregion

    #region Events  

    private void QuanTextBoxOnKeyUp(object sender, KeyEventArgs e)
    {
        if (!_popup.IsOpen)
        {
            Dispatcher.InvokeAsync(() =>
            {
                _popup.IsOpen = true;
            });
        }

        if (sender is not QuanTextBox quanTextBox)
        {
            return;
        }

        if (quanTextBox.Text.Length != 8)
        {
            return;
        }

        if (TimeSpan.TryParseExact(quanTextBox.Text, TimeFormat, CultureInfo.CurrentCulture, out var timeSpan))
        {
            SetValue(SelectedTimeProperty, timeSpan);
        }
    }

    private void QuanTextBoxOnPreviewMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_popup.IsOpen)
        {
            Dispatcher.InvokeAsync(() =>
            {
                _popup.IsOpen = true;
            });
        }
    }

    private void PopupOnOpened(object sender, EventArgs e)
    {
        _popupHoursListBox.SelectedIndex = SelectedTime.Hours;
        _popupMinutesListBox.SelectedIndex = SelectedTime.Minutes;
        _popupSecondsListBox.SelectedIndex = SelectedTime.Seconds;
    }

    private void PopupHoursListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox { SelectedValue: int hours } listBox)
        {
            return;
        }

        SelectedTime = new TimeSpan(0, hours, SelectedTime.Minutes, SelectedTime.Seconds);

        var scrollViewer = listBox.FindVisualChild<ScrollViewer>();
        if (scrollViewer == null)
        {
            return;
        }

        scrollViewer.ScrollToBottom();
        listBox.ScrollIntoView(hours);
    }
    private void PopupMinutesListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox { SelectedValue: int minutes } listBox)
        {
            return;
        }

        SelectedTime = new TimeSpan(0, SelectedTime.Hours, minutes, SelectedTime.Seconds);

        var scrollViewer = listBox.FindVisualChild<ScrollViewer>();
        if (scrollViewer == null)
        {
            return;
        }
        scrollViewer.ScrollToBottom();
        listBox.ScrollIntoView(minutes);
    }

    private void PopupSecondsListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is not ListBox { SelectedValue: int seconds } listBox)
        {
            return;
        }

        SelectedTime = new TimeSpan(0, SelectedTime.Hours, SelectedTime.Minutes, seconds);
        var scrollViewer = listBox.FindVisualChild<ScrollViewer>();
        if (scrollViewer == null)
        {
            return;
        }
        scrollViewer.ScrollToBottom();
        listBox.ScrollIntoView(seconds);
    }

    #endregion
}