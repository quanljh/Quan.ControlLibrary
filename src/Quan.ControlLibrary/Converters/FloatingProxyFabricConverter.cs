using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Quan.ControlLibrary.Helpers.FloatingTextProxy;

namespace Quan.ControlLibrary.Converters;

public class FloatingProxyFabricConverter : IValueConverter
{
    private static readonly Lazy<FloatingProxyFabricConverter> _instance = new();

    public static FloatingProxyFabricConverter Instance => _instance.Value;

    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value == null ? null : FloatingProxyFabric.Get((Control)value);

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Binding.DoNothing;
}