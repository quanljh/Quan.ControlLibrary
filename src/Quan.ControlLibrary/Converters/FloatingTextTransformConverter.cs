using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Point = System.Drawing.Point;

namespace Quan.ControlLibrary.Converters;

public class FloatingTextTransformConverter : BaseValueConverter, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is not { Length: 3 }
            || values.Any(o => o == null)
            || !double.TryParse(values[0]?.ToString(), out var scale)
            || !double.TryParse(values[1]?.ToString(), out var floatingScale)
            || values[2] is not Point offset)
        {
            return Transform.Identity;
        }

        var result = 1 + (floatingScale - 1) * scale;

        var transformGroup = new TransformGroup();
        transformGroup.Children.Add(new ScaleTransform
        {
            ScaleX = result,
            ScaleY = result
        });
        transformGroup.Children.Add(new TranslateTransform
        {
            X = scale * offset.X,
            Y = scale * offset.Y
        });

        return transformGroup;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}