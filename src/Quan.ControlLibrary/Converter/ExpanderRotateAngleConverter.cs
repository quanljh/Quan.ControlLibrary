using System.Globalization;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class ExpanderRotateAngleConverter : BaseValueConverter<ExpandDirection, double>
    {
        /// <inheritdoc />
        public override double Convert(ExpandDirection value, object? parameter, CultureInfo culture)
        {
            double factor = 1.0;
            if (parameter != null)
            {
                if (!double.TryParse(parameter.ToString(), out factor))
                {
                    factor = 1.0;
                }
            }

            switch (value)
            {
                case ExpandDirection.Left:
                    return 90 * factor;
                case ExpandDirection.Right:
                    return -90 * factor;
                default:
                    return 0;
            }
        }

        /// <inheritdoc />
        public override ExpandDirection ConvertBack(double value, object? parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
