using System;
using System.Globalization;
using System.Windows;

namespace Quan.ControlLibrary
{
    public class StringToVisibilityConverter : BaseValueConverter<string, Visibility>
    {
        public override Visibility Convert(string value, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public override string ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
