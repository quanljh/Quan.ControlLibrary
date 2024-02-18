using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

public class QuanThumbContentControlDragCompletedEventArgs : DragCompletedEventArgs
{
    public QuanThumbContentControlDragCompletedEventArgs(double horizontalOffset, double verticalOffset, bool canceled) : base(horizontalOffset, verticalOffset, canceled)
    {
        RoutedEvent = QuanThumbContentControl.DragCompletedEvent;
    }
}