using System;
using System.Globalization;
using System.Windows;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// A converter that takes in a boolean and returns a thickness of 2 if true, useful for applying 
    /// border radius on a true value
    /// </summary>
    public class BooleanToBorderThicknessConverter : BaseValueConverter<bool, Thickness>
    {
        public override Thickness Convert(bool value, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value ? new Thickness(2) : new Thickness(0);
            else
                return value ? new Thickness(0) : new Thickness(2);
        }

        public override bool ConvertBack(Thickness value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
