// Copy from Modern WPF https://github.com/Kinnara/ModernWpf

using System.ComponentModel;
using System.Windows;

namespace Quan.ControlLibrary;

public class WindowHelper
{
    #region FixMaximizedWindow

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly DependencyProperty FixMaximizedWindowProperty =
        DependencyProperty.RegisterAttached(
            "FixMaximizedWindow",
            typeof(bool),
            typeof(WindowHelper),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnFixMaximizedWindowChanged));

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static bool GetFixMaximizedWindow(Window window) => (bool)window.GetValue(FixMaximizedWindowProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetFixMaximizedWindow(Window window, bool value) => window.SetValue(FixMaximizedWindowProperty, BooleanBoxes.Box(value));

    private static void OnFixMaximizedWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Window window)
        {
            if ((bool)e.NewValue)
            {
                MaximizedWindowFixer.SetMaximizedWindowFixer(window, new MaximizedWindowFixer());
            }
            else
            {
                window.ClearValue(MaximizedWindowFixer.MaximizedWindowFixerProperty);
            }
        }
    }

    #endregion
}