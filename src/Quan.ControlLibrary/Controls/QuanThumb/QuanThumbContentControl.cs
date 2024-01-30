using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Quan.ControlLibrary.Automations;
using Quan.ControlLibrary.Helpers.Boxes;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

public class QuanThumbContentControl : ContentControlEx, IQuanThumb
{
    #region Private Members

    private TouchDevice _currentDevice;

    private Point _oldDragScreenPoint;

    private Point _startDragPoint;

    private Point _startDragScreenPoint;

    #endregion

    #region Public properties

    #region DragStarted

    public event DragStartedEventHandler DragStarted
    {
        add => AddHandler(DragStartedEvent, value);
        remove => RemoveHandler(DragStartedEvent, value);
    }

    public static readonly RoutedEvent DragStartedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(DragStarted),
            RoutingStrategy.Bubble,
            typeof(DragStartedEventHandler),
            typeof(QuanThumbContentControl));

    #endregion

    #region DragDelta

    public event DragDeltaEventHandler DragDelta
    {
        add => AddHandler(DragDeltaEvent, value);
        remove => RemoveHandler(DragDeltaEvent, value);
    }

    public static readonly RoutedEvent DragDeltaEvent =
        EventManager.RegisterRoutedEvent(
            nameof(DragDelta),
            RoutingStrategy.Bubble,
            typeof(DragDeltaEventHandler),
            typeof(QuanThumbContentControl));

    #endregion

    #region DragCompleted

    public event DragCompletedEventHandler DragCompleted
    {
        add => AddHandler(DragCompletedEvent, value);
        remove => RemoveHandler(DragCompletedEvent, value);
    }

    public static readonly RoutedEvent DragCompletedEvent =
        EventManager.RegisterRoutedEvent(
            nameof(DragCompleted),
            RoutingStrategy.Bubble,
            typeof(DragCompletedEventHandler),
            typeof(QuanThumbContentControl));

    #endregion

    #region IsDragging

    public bool IsDragging
    {
        get => (bool)GetValue(IsDraggingProperty);
        protected set => SetValue(IsDraggingPropertyKey, BooleanBoxes.Box(value));
    }

    private static readonly DependencyPropertyKey IsDraggingPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(IsDragging),
            typeof(bool),
            typeof(QuanThumbContentControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

    public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

    #endregion

    #endregion

    #region Constructor

    static QuanThumbContentControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanThumbContentControl),
            new FrameworkPropertyMetadata(typeof(QuanThumbContentControl)));
        FocusableProperty.OverrideMetadata(typeof(QuanThumbContentControl),
            new FrameworkPropertyMetadata(default(bool)));
        EventManager.RegisterClassHandler(typeof(QuanThumbContentControl), Mouse.LostMouseCaptureEvent,
            new MouseEventHandler(OnLostMouseCapture));
    }

    #endregion

    #region Overrides

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (!IsDragging)
        {
            e.Handled = true;
            try
            {
                // focus me
                Focus();
                // now capture the mouse for the drag action
                CaptureMouse();
                // so now we are in dragging mode
                SetValue(IsDraggingPropertyKey, BooleanBoxes.TrueBox);
                // get the mouse points
                _startDragPoint = e.GetPosition(this);
                _oldDragScreenPoint = _startDragScreenPoint = PointToScreen(_startDragPoint);

                RaiseEvent(new QuanThumbContentControlDragStartedEventArgs(_startDragPoint.X, _startDragPoint.Y));
            }
            catch (Exception exception)
            {
                Trace.TraceError($"{this}: Something went wrong here: {exception} {Environment.NewLine} {exception.StackTrace}");
                CancelDragAction();
            }
        }

        base.OnMouseLeftButtonDown(e);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (IsMouseCaptured && IsDragging)
        {
            e.Handled = true;
            // now we are in normal mode
            ClearValue(IsDraggingPropertyKey);
            // release the captured mouse
            ReleaseMouseCapture();
            // get the current mouse position and call the completed event with the horizontal/vertical change
            var currentMouseScreenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
            var horizontalChange = currentMouseScreenPoint.X - _startDragScreenPoint.X;
            var verticalChange = currentMouseScreenPoint.Y - _startDragScreenPoint.Y;
            RaiseEvent(new QuanThumbContentControlDragCompletedEventArgs(horizontalChange, verticalChange, false));
        }

        base.OnMouseLeftButtonUp(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (!IsDragging) return;

        if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
        {
            var currentDragPoint = e.GetPosition(this);
            // Get client point and convert it to screen point
            var currentDragScreenPoint = PointToScreen(currentDragPoint);
            if (currentDragScreenPoint != _oldDragScreenPoint)
            {
                _oldDragScreenPoint = currentDragScreenPoint;
                e.Handled = true;
                var horizontalChange = currentDragPoint.X - _startDragPoint.X;
                var verticalChange = currentDragPoint.Y - _startDragPoint.Y;
                RaiseEvent(new DragDeltaEventArgs(horizontalChange, verticalChange) { RoutedEvent = DragDeltaEvent });
            }
        }
        else
        {
            // clear some saved stuff
            if (ReferenceEquals(e.MouseDevice.Captured, this)) ReleaseMouseCapture();

            ClearValue(IsDraggingPropertyKey);
            _startDragPoint.X = 0;
            _startDragPoint.Y = 0;
        }
    }

    protected override void OnPreviewTouchDown(TouchEventArgs e)
    {
        // Release any previous capture
        ReleaseCurrentDevice();
        // Capture the new touch
        CaptureCurrentDevice(e);
    }

    protected override void OnPreviewTouchUp(TouchEventArgs e)
    {
        ReleaseCurrentDevice();
    }

    protected override void OnLostTouchCapture(TouchEventArgs e)
    {
        // Only re-capture if the reference is not null
        // This way we avoid re-capturing after calling ReleaseCurrentDevice()
        if (_currentDevice != null) CaptureCurrentDevice(e);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new QuanThumbContentControlAutomationPeer(this);
    }

    #endregion

    #region Methods

    private static void OnLostMouseCapture(object sender, MouseEventArgs e)
    {
        // Cancel the drag action if we lost capture
        var thumb = (QuanThumbContentControl)sender;
        if (!ReferenceEquals(Mouse.Captured, thumb)) thumb.CancelDragAction();
    }

    public void CancelDragAction()
    {
        if (!IsDragging) return;

        if (IsMouseCaptured) ReleaseMouseCapture();

        ClearValue(IsDraggingPropertyKey);
        var horizontalChange = _oldDragScreenPoint.X - _startDragScreenPoint.X;
        var verticalChange = _oldDragScreenPoint.Y - _startDragScreenPoint.Y;
        RaiseEvent(new QuanThumbContentControlDragCompletedEventArgs(horizontalChange, verticalChange, true));
    }

    private void ReleaseCurrentDevice()
    {
        if (_currentDevice != null)
        {
            // Set the reference to null so that we don't re-capture in the OnLostTouchCapture() method
            var temp = _currentDevice;
            _currentDevice = null;
            ReleaseTouchCapture(temp);
        }
    }

    private void CaptureCurrentDevice(TouchEventArgs e)
    {
        var gotTouch = CaptureTouch(e.TouchDevice);
        if (gotTouch) _currentDevice = e.TouchDevice;
    }

    #endregion
}