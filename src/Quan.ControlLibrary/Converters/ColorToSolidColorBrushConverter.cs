using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Quan.ControlLibrary.Converters;

public class ColorToSolidColorBrushConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Color color)
        {
            throw new ArgumentException();
        }

        var brush = new SolidColorBrush(color);
        brush.Freeze();
        return brush;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush brush)
        {
            throw new ArgumentException();
        }
        return brush.Color;
    }
}