using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quan.ControlLibrary
{
    public class FloatingProxyFabricConverter
        : IValueConverter
    {
        private static readonly Lazy<FloatingProxyFabricConverter> _instance = new Lazy<FloatingProxyFabricConverter>();

        public static FloatingProxyFabricConverter Instance => _instance.Value;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return FloatingProxyFabric.Get(value as Control);
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Binding.DoNothing;
    }
}
