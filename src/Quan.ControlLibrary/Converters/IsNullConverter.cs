﻿using System.Globalization;
using System.Windows.Data;

namespace Quan.ControlLibrary.Converters;

public class IsNullConverter : BaseValueConverter, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}