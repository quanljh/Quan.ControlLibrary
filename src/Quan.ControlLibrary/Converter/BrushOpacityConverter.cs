using System;
using System.Globalization;
using System.Windows.Media;

namespace Quan.ControlLibrary;

public class BrushOpacityConverter : BaseValueConverter<SolidColorBrush, SolidColorBrush>
{
    public override SolidColorBrush Convert(SolidColorBrush value, object parameter, CultureInfo culture)
    {
        var opacity = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
        return new SolidColorBrush(value.Color)
        {
            Opacity = opacity
        };
    }

    public override SolidColorBrush ConvertBack(SolidColorBrush value, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}