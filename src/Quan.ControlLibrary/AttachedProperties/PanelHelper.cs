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

        public static Thickness GetChildMargin(DependencyObject element) => (Thickness)element.GetValue(ChildMarginProperty);

        public static void SetChildMargin(DependencyObject element, Thickness value) => element.SetValue(ChildMarginProperty, value);

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
                foreach (var child in panel.Children)
                {
                    if (child is FrameworkElement chidElement && e.NewValue is Thickness margin)
                    {
                        chidElement.Margin = new Thickness(
                            chidElement.Margin.Left + margin.Left,
                            chidElement.Margin.Top + margin.Top,
                            chidElement.Margin.Right + margin.Right,
                            chidElement.Margin.Bottom + margin.Bottom
                            );
                    }
                }
            }
        }

        private static void Panel_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(sender is Panel panel))
                return;

            var margin = GetChildMargin(panel);
            foreach (var child in panel.Children)
            {
                if (child is FrameworkElement chidElement)
                {
                    chidElement.Margin = new Thickness(
                        chidElement.Margin.Left + margin.Left,
                        chidElement.Margin.Top + margin.Top,
                        chidElement.Margin.Right + margin.Right,
                        chidElement.Margin.Bottom + margin.Bottom
                    );
                }
            }

            panel.Loaded -= Panel_OnLoaded;
        }

        #endregion
    }
}
