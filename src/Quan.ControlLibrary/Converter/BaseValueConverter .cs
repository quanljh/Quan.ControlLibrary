﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Quan.ControlLibrary
{
    public abstract class BaseValueConverter<TSource, TTarget> : MarkupExtension, IValueConverter
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

        /// <summary>
        /// The method to convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return Convert((TSource)value, parameter, culture);
        }


        /// <summary>
        /// The method to convert TSource value to TTarget value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract TTarget? Convert(TSource value, object? parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert value back to it's source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return ConvertBack((TTarget)value, parameter, culture);
        }

        /// <summary>
        /// The method to convert TTarget value back to it's source TSource
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract TSource? ConvertBack(TTarget value, object? parameter, CultureInfo culture);

        #endregion

    }
}
