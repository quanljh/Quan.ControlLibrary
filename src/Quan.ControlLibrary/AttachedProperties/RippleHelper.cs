using System.Drawing;
using System.Windows;

namespace Quan.ControlLibrary
{
    public static class RippleHelper
    {
        #region BorderBrush

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.RegisterAttached("BorderBrush",
                typeof(Brush),
                typeof(RippleHelper),
                new PropertyMetadata(Brushes.Transparent));

        public static Brush GetBorderBrush(DependencyObject element) => (Brush)element.GetValue(BorderBrushProperty);

        public static void SetBorderBrush(DependencyObject element, Brush value) => element.SetValue(BorderBrushProperty, value);

        #endregion
    }
}
