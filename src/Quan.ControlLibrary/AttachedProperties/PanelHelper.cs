using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class PanelHelper
    {
        #region ChildMargin

        public static readonly DependencyProperty ChildMarginProperty =
            DependencyProperty.RegisterAttached("ChildMargin",
                typeof(Thickness),
                typeof(PanelHelper),
                new FrameworkPropertyMetadata(default(Thickness), ChildMargin_OnChanged));

        public static Thickness GetChildMarginProperty(DependencyObject element) => (Thickness)element.GetValue(ChildMarginProperty);

        public static void SetChildMarginProperty(DependencyObject element, Thickness value) => element.SetValue(ChildMarginProperty, value);

        private static void ChildMargin_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is Panel panel))
                return;

            if (!panel.IsLoaded)
            {
                panel.Loaded += Panel_OnLoaded;
            }
            else
            {
                var margin = GetChildMarginProperty(panel);

                foreach (var child in panel.Children)
                {
                    if (child is FrameworkElement chidElement && e.NewValue is Thickness thickness)
                    {
                        chidElement.Margin = margin;
                    }
                }
            }
        }

        private static void Panel_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Panel panel))
                return;

            var margin = GetChildMarginProperty(panel);
            foreach (var child in panel.Children)
            {
                if (child is FrameworkElement chidElement)
                {
                    chidElement.Margin = margin;
                }
            }

            panel.Loaded -= Panel_OnLoaded;
        }

        #endregion
    }
}
