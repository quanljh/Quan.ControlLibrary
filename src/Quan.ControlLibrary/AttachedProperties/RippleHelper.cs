using System.Windows;
using System.Windows.Media;

namespace Quan.ControlLibrary
{
    public static class RippleHelper
    {
        #region BorderBrush

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached("Background",
                typeof(Brush),
                typeof(RippleHelper),
                new PropertyMetadata(Brushes.Transparent));

        public static Brush GetBackground(DependencyObject element) => (Brush)element.GetValue(BackgroundProperty);

        public static void SetBackground(DependencyObject element, Brush value) => element.SetValue(BackgroundProperty, value);

        #endregion
    }
}
