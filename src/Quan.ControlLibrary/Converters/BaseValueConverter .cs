using System.Windows.Markup;

namespace Quan.ControlLibrary.Converters;

public abstract class BaseValueConverter : MarkupExtension
{
    #region Markup Extension Methods

    /// <summary>
    /// Provides a static instance of the value converter
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <returns></returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    #endregion

}