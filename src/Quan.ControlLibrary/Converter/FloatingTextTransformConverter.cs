using System;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using Point = System.Drawing.Point;

namespace Quan.ControlLibrary
{
    public class FloatingTextTransformConverter : BaseMultiValueConverter<Transform>
    {
        /// <inheritdoc />
        public override Transform? Convert(object?[]? values, object? parameter, CultureInfo? culture)
        {
            if (values == null
                || values.Length != 3
                || values.Any(o => o == null)
                || !double.TryParse(values[0]?.ToString(), out double scale)
                || !double.TryParse(values[1]?.ToString(), out double floatingScale)
                || !(values[2] is Point offset))
            {
                return Transform.Identity;
            }

            double result = 1 + (floatingScale - 1) * scale;

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

        /// <inheritdoc />
        public override object?[]? ConvertBack(Transform? value, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
