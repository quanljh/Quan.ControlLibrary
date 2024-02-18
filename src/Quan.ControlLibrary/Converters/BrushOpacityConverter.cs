using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Quan.ControlLibrary.Converters;

public class BrushOpacityConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SolidColorBrush brush)
        {
            throw new ArgumentException();
        }

        var opacity = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        return new SolidColorBrush(brush.Color)
        {
            Opacity = opacity
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}