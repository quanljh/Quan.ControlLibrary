using System.Globalization;
using System.Windows.Media;

namespace Quan.ControlLibrary.Converters;

public class ColorToSolidColorBrushConverter : BaseValueConverter<Color, SolidColorBrush>
{
    public override SolidColorBrush Convert(Color value, object parameter, CultureInfo culture)
    {
        var brush = new SolidColorBrush(value);
        brush.Freeze();
        return brush;
    }

    public override Color ConvertBack(SolidColorBrush value, object parameter, CultureInfo culture)
    {
        return value.Color;
    }
}