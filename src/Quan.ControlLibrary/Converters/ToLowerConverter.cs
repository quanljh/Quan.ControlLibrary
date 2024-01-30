using System.Globalization;

namespace Quan.ControlLibrary.Converters;

public class ToLowerConverter : BaseValueConverter<string, string>
{
    public override string Convert(string value, object parameter, CultureInfo culture)
    {
        return value.ToLower(culture);
    }

    public override string ConvertBack(string value, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}