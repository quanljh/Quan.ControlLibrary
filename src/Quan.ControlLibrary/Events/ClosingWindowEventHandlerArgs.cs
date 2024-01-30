namespace Quan.ControlLibrary.Events;

public class ClosingWindowEventHandlerArgs : EventArgs
{
    public bool Cancelled { get; set; }
}