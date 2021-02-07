using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Quan.ControlLibrary
{
    public static class ExpanderHelper
    {
        private static readonly Thickness DefaultHorizontalHeaderPadding = new Thickness(24, 12, 24, 12);
        private static readonly Thickness DefaultVerticalHeaderPadding = new Thickness(12, 24, 12, 24);

        #region HorizontalHeaderPadding

        public static readonly DependencyProperty HorizontalHeaderPaddingProperty = DependencyProperty.RegisterAttached(
                "HorizontalHeaderPadding",
                typeof(Thickness),
                typeof(ExpanderHelper),
                new FrameworkPropertyMetadata(DefaultHorizontalHeaderPadding, FrameworkPropertyMetadataOptions.Inherits));

        public static Thickness GetHorizontalHeaderPadding(Expander element) => (Thickness)element.GetValue(HorizontalHeaderPaddingProperty);
        public static void SetHorizontalHeaderPadding(Expander element, Thickness value) => element.SetValue(HorizontalHeaderPaddingProperty, value);

        #endregion

        #region HeaderFontSize

        public static readonly DependencyProperty HeaderFontSizeProperty = DependencyProperty.RegisterAttached(
            "HeaderFontSize",
            typeof(double),
            typeof(ExpanderHelper),
                new FrameworkPropertyMetadata(15.0));

        public static double GetHeaderFontSize(Expander element) => (double)element.GetValue(HeaderFontSizeProperty);
        public static void SetHeaderFontSize(Expander element, double value) => element.SetValue(HeaderFontSizeProperty, value);

        #endregion

        #region HeaderBackground
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.RegisterAttached(
            "HeaderBackground",
            typeof(Brush),
            typeof(ExpanderHelper));

        public static Brush GetHeaderBackground(Expander element) => (Brush)element.GetValue(HeaderBackgroundProperty);
        public static void SetHeaderBackground(Expander element, Brush value) => element.SetValue(HeaderBackgroundProperty, value);

        #endregion
    }
}
