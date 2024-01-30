using System.Globalization;

namespace Quan.ControlLibrary.Converters;

public class IsNullConverter : BaseValueConverter<object, bool>
{
    public override bool Convert(object value, object parameter, CultureInfo culture)
    {
        return value is null;
    }

    public override object ConvertBack(bool value, object parameter, CultureInfo culture)
    {
        throw new System.NotImplementedException();
    }
}