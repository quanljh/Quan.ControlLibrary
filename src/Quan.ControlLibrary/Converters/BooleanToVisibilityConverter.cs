using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Quan.ControlLibrary.Converters;

public class BooleanToCollapsedConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool visible)
        {
            throw new ArgumentException();
        }

        if (parameter != null)
        {
            return visible ? Visibility.Collapsed : Visibility.Visible;
        }

        return visible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visible)
        {
            return false;
        }
        return visible == Visibility.Visible;
    }
}

public class BooleanToHiddenConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool visible)
        {
            throw new ArgumentException();
        }

        if (parameter != null)
        {
            return visible ? Visibility.Hidden : Visibility.Visible;
        }

        return visible ? Visibility.Visible : Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visible)
        {
            return false;
        }
        return visible == Visibility.Visible;
    }
}

public class CollapsedToBooleanConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visible)
        {
            return false;
        }
        return visible == Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool visible)
        {
            throw new ArgumentException();
        }

        return visible ? Visibility.Visible : Visibility.Hidden;
    }
}