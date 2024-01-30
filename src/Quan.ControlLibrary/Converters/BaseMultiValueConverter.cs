using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Quan.ControlLibrary.Converters;

public abstract class BaseMultiValueConverter<TTarget> : MarkupExtension, IMultiValueConverter
{
    #region Markup Extension Methods

    /// <summary>
    /// Provides a static instance of the value converter
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    #endregion

    #region Value Converter Methods

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => Convert(values, parameter, culture);

    /// <summary>
    /// Converts object array to target type
    /// </summary>
    /// <param name="values"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public abstract TTarget Convert(object[] values, object parameter, CultureInfo culture);

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => ConvertBack((TTarget)value, parameter, culture);

    /// <summary>
    /// Converts target type value back to object array to 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public abstract object[] ConvertBack(TTarget value, object parameter, CultureInfo culture);


    #endregion
}