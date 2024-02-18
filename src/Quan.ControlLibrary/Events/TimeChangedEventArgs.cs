using System.Windows;

namespace Quan.ControlLibrary.Events;

public sealed class TimeChangedEventArgs(RoutedEvent routedEvent, DateTime oldTime, DateTime newTime)
    : RoutedEventArgs(routedEvent)
{
    public DateTime OldTime { get; } = oldTime;
    public DateTime NewTime { get; } = newTime;
}