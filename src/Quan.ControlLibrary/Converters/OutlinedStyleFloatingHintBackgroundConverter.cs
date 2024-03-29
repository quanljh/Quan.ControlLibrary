﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Quan.ControlLibrary.AttachedProperties;

namespace Quan.ControlLibrary.Converters;

public class OutlinedStyleFloatingHintBackgroundConverter : BaseValueConverter, IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is { Length: 2 } &&
            values[0] is Brush hintAssistBrush &&
            values[1] is Brush defaultBackgroundBrush)
        {
            return Equals(FloatingTextHelper.DefaultBackground, hintAssistBrush) ? defaultBackgroundBrush : hintAssistBrush;
        }
        return Binding.DoNothing;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}