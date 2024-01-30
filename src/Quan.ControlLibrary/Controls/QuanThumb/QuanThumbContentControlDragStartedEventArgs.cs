using System.Windows.Controls.Primitives;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

public class QuanThumbContentControlDragStartedEventArgs : DragStartedEventArgs
{
    public QuanThumbContentControlDragStartedEventArgs(double horizontalOffset, double verticalOffset) : base(horizontalOffset, verticalOffset)
    {
        RoutedEvent = QuanThumbContentControl.DragStartedEvent;
    }
}