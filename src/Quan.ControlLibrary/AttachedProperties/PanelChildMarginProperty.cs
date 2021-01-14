using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// The NoFrameHistory attached property for creating a <see cref="Frame"/> that never shows navigation
    /// and keeps the navigation history empty 
    /// </summary>
    public class PanelChildMarginProperty : BaseAttachedProperty<PanelChildMarginProperty, string>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Get the panel (grid typically)
            if (sender is Panel panel)
            {
                //Wait for panel to load
                panel.Loaded += (ss, ee) =>
                {
                    //loop each child
                    foreach (UIElement child in panel.Children)
                        //Set it's margin to given value
                        if (child is FrameworkElement childElement && e.NewValue is string newValue)
                            childElement.Margin = (Thickness)(new ThicknessConverter().ConvertFromString(newValue) ?? 0);
                };
            }
        }


    }
}
