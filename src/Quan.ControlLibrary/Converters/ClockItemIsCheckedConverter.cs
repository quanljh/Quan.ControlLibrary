using System.Globalization;
using System.Windows.Data;
using Quan.ControlLibrary.Enums;

namespace Quan.ControlLibrary.Converters;

internal class ClockItemIsCheckedConverter(
    Func<DateTime> currentTimeGetter,
    ClockDisplayMode displayMode,
    bool is24Hours)
    : BaseValueConverter, IValueConverter
{
    private readonly Func<DateTime> _currentTimeGetter = currentTimeGetter ?? throw new ArgumentNullException(nameof(currentTimeGetter));

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dateTime = (DateTime)value;
        var i = (int)parameter;

        int converted;
        if (displayMode == ClockDisplayMode.Hours)
            converted = MassageHour(dateTime.Hour, is24Hours);
        else if (displayMode == ClockDisplayMode.Minutes)
            converted = MassageMinuteSecond(dateTime.Minute);
        else
            converted = MassageMinuteSecond(dateTime.Second);
        return converted == i;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value != true) return Binding.DoNothing;

        var currentTime = _currentTimeGetter();

        return new DateTime(
            currentTime.Year, currentTime.Month, currentTime.Day,
            (displayMode == ClockDisplayMode.Hours) ? ReverseMassageHour((int)parameter, currentTime, is24Hours) : currentTime.Hour,
            (displayMode == ClockDisplayMode.Minutes) ? ReverseMassageMinuteSecond((int)parameter) : currentTime.Minute,
            (displayMode == ClockDisplayMode.Seconds) ? ReverseMassageMinuteSecond((int)parameter) : currentTime.Second);
    }

    private static int MassageHour(int val, bool is24Hours)
    {
        if (is24Hours)
        {
            return val == 0 ? 24 : val;
        }

        if (val == 0) return 12;
        if (val > 12) return val - 12;
        return val;
    }

    private static int MassageMinuteSecond(int val)
        => val == 0 ? 60 : val;

    private static int ReverseMassageHour(int val, DateTime currentTime, bool is24Hours)
    {
        if (is24Hours)
        {
            return val == 24 ? 0 : val;
        }

        return currentTime.Hour < 12
            ? (val == 12 ? 0 : val)
            : (val == 12 ? 12 : val + 12);
    }

    private static int ReverseMassageMinuteSecond(int val)
        => val == 60 ? 0 : val;
}