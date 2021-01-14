using System;
using System.Globalization;
using System.Windows;

namespace Quan.ControlLibrary
{
    public class BooleanToCollapsedConverter : BaseValueConverter<bool, Visibility>
    {
        public override Visibility Convert(bool value, object parameter, CultureInfo culture)
        {
            if (parameter != null)
                return value ? Visibility.Collapsed : Visibility.Visible;
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override bool ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            return value == Visibility.Visible;
        }
    }

    public class BooleanToHiddenConverter : BaseValueConverter<bool, Visibility>
    {
        public override Visibility Convert(bool value, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value ? Visibility.Hidden : Visibility.Visible;
            return value ? Visibility.Visible : Visibility.Hidden;
        }

        public override bool ConvertBack(Visibility value, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CollapsedToBooleanConverter : BaseValueConverter<Visibility, bool>
    {
        public override bool Convert(Visibility value, object parameter, CultureInfo culture)
        {
            return value == Visibility.Visible;
        }

        public override Visibility ConvertBack(bool value, object parameter, CultureInfo culture)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
