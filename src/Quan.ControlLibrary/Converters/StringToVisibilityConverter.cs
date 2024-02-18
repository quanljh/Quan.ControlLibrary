using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Quan.ControlLibrary.Converters;

public class StringToVisibilityConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
        {
            throw new ArgumentException();
        }

        return string.IsNullOrEmpty(stringValue) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}