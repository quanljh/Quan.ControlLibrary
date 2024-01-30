using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ControlzEx;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

public class ContentPresenterEx : ContentPresenter
{
    static ContentPresenterEx()
    {
        ContentProperty.OverrideMetadata(typeof(ContentPresenterEx), new FrameworkPropertyMetadata(OnContentPropertyChanged));
    }

    private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is IInputElement and DependencyObject oldInputElement)
        {
            BindingOperations.ClearBinding(oldInputElement, WindowChrome.IsHitTestVisibleInChromeProperty);
        }

        if (e.NewValue is IInputElement and DependencyObject newInputElement)
        {
            BindingOperations.SetBinding(
                newInputElement,
                WindowChrome.IsHitTestVisibleInChromeProperty,
                new Binding
                {
                    Path = new PropertyPath(WindowChrome.IsHitTestVisibleInChromeProperty),
                    Source = d
                });
        }
    }
}