using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

public class QuanClockItemButton : ToggleButton
{
    public const string ThumbPartName = "PART_Thumb";

    private Thumb _thumb;

    #region Dependency properties

    #region CentreX

    public double CentreX
    {
        get => (double)GetValue(CentreXProperty);
        set => SetValue(CentreXProperty, value);
    }

    public static readonly DependencyProperty CentreXProperty =
        DependencyProperty.Register(
            nameof(CentreX),
            typeof(double),
            typeof(QuanClockItemButton),
            new PropertyMetadata(default(double)));

    #endregion

    #region CentreY

    public double CentreY
    {
        get => (double)GetValue(CentreYProperty);
        set => SetValue(CentreYProperty, value);
    }

    public static readonly DependencyProperty CentreYProperty =
        DependencyProperty.Register(
            nameof(CentreY),
            typeof(double),
            typeof(QuanClockItemButton),
            new PropertyMetadata(default(double)));

    #endregion

    #region X

    public double X
    {
        get => (double)GetValue(XProperty);
        private set => SetValue(XPropertyKey, value);
    }

    private static readonly DependencyPropertyKey XPropertyKey =
        DependencyProperty.RegisterReadOnly(
            "X",
            typeof(double),
            typeof(QuanClockItemButton),
            new PropertyMetadata(default(double)));

    public static readonly DependencyProperty XProperty = XPropertyKey.DependencyProperty;

    #endregion

    #region Y

    public double Y
    {
        get => (double)GetValue(YProperty);
        private set => SetValue(YPropertyKey, value);
    }

    private static readonly DependencyPropertyKey YPropertyKey =
        DependencyProperty.RegisterReadOnly(
            "Y",
            typeof(double),
            typeof(QuanClockItemButton),
            new PropertyMetadata(default(double)));

    public static readonly DependencyProperty YProperty = YPropertyKey.DependencyProperty;

    #endregion

    #endregion

    #region Events

    public static readonly RoutedEvent DragDeltaEvent =
        EventManager.RegisterRoutedEvent(
            "DragDelta",
            RoutingStrategy.Bubble,
            typeof(DragDeltaEventHandler),
            typeof(QuanClockItemButton));

    public static readonly RoutedEvent DragStartedEvent =
        EventManager.RegisterRoutedEvent(
            "DragStarted",
            RoutingStrategy.Bubble,
            typeof(DragStartedEventHandler),
            typeof(QuanClockItemButton));

    public static readonly RoutedEvent DragCompletedEvent =
        EventManager.RegisterRoutedEvent(
            "DragCompleted",
            RoutingStrategy.Bubble,
            typeof(DragCompletedEventHandler),
            typeof(QuanClockItemButton));

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
        if (_thumb != null)
        {
            _thumb.DragStarted -= ThumbOnDragStarted;
            _thumb.DragDelta -= ThumbOnDragDelta;
            _thumb.DragCompleted -= ThumbOnDragCompleted;
        }

        _thumb = GetTemplateChild(ThumbPartName) as Thumb;

        if (_thumb != null)
        {
            _thumb.DragStarted += ThumbOnDragStarted;
            _thumb.DragDelta += ThumbOnDragDelta;
            _thumb.DragCompleted += ThumbOnDragCompleted;
        }

        base.OnApplyTemplate();
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (_thumb == null) return;

        base.OnPreviewMouseLeftButtonDown(e);
        if (!IsChecked.HasValue || !IsChecked.Value)
        {
            OnToggle();
        }
    }

    /// <summary> 
    /// This override method is called when the control is clicked by mouse or keyboard
    /// </summary> 
    protected override void OnClick()
    {
        if (_thumb == null)
            base.OnClick();
    }


    protected override Size ArrangeOverride(Size finalSize)
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            X = CentreX - finalSize.Width / 2;
            Y = CentreY - finalSize.Height / 2;
        }));

        return base.ArrangeOverride(finalSize);
    }

    #endregion

    #region Methods

    private static void OnDragDelta(
        DependencyObject d, double horizontalChange, double verticalChange)
    {
        var instance = (QuanClockItemButton)d;
        var dragDeltaEventArgs = new DragDeltaEventArgs(horizontalChange, verticalChange)
        {
            RoutedEvent = DragDeltaEvent,
            Source = d
        };

        instance.RaiseEvent(dragDeltaEventArgs);
    }

    private static void OnDragStarted(DependencyObject d, double horizontalOffset, double verticalOffset)
    {
        var instance = (QuanClockItemButton)d;
        var dragStartedEventArgs = new DragStartedEventArgs(horizontalOffset, verticalOffset)
        {
            RoutedEvent = DragStartedEvent,
            Source = d
        };

        instance.RaiseEvent(dragStartedEventArgs);
    }

    private static void OnDragCompleted(DependencyObject d, double horizontalChange, double verticalChange, bool canceled)
    {
        var instance = (QuanClockItemButton)d;
        var dragCompletedEventArgs = new DragCompletedEventArgs(horizontalChange, verticalChange, canceled)
        {
            RoutedEvent = DragCompletedEvent,
            Source = d
        };

        instance.RaiseEvent(dragCompletedEventArgs);
    }


    private void ThumbOnDragStarted(object sender, DragStartedEventArgs dragStartedEventArgs)
    {
        //Get the absolute position of where the drag operation started
        OnDragStarted(this, CentreX + dragStartedEventArgs.HorizontalOffset - Width / 2.0, CentreY + dragStartedEventArgs.VerticalOffset - Height / 2.0);
    }

    private void ThumbOnDragDelta(object sender, DragDeltaEventArgs dragDeltaEventArgs)
        => OnDragDelta(this, dragDeltaEventArgs.HorizontalChange, dragDeltaEventArgs.VerticalChange);

    private void ThumbOnDragCompleted(object sender, DragCompletedEventArgs dragCompletedEventArgs)
        => OnDragCompleted(this, dragCompletedEventArgs.HorizontalChange, dragCompletedEventArgs.VerticalChange, dragCompletedEventArgs.Canceled);

    #endregion

}