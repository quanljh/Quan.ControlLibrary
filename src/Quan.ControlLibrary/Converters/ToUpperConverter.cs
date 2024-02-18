using System.Globalization;
using System.Windows.Data;

namespace Quan.ControlLibrary.Converters;

public class ToUpperConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
        {
            throw new ArgumentException();
        }

        return stringValue.ToUpper(culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}