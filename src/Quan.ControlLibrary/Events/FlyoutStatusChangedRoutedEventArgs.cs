#nullable enable
using System.Windows;
using Quan.ControlLibrary.Controls;

namespace Quan.ControlLibrary.Events;

public class FlyoutStatusChangedRoutedEventArgs : RoutedEventArgs
{
    internal FlyoutStatusChangedRoutedEventArgs(RoutedEvent routedEvent, object source)
        : base(routedEvent, source)
    {
    }

    public Flyout? ChangedFlyout { get; internal set; }
}