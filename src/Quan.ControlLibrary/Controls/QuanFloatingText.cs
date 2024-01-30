using System.Windows;
using System.Windows.Controls;
using Quan.ControlLibrary.Helpers;
using Quan.ControlLibrary.Helpers.Boxes;
using Quan.ControlLibrary.Interfaces;
using Point = System.Drawing.Point;

namespace Quan.ControlLibrary.Controls;

[TemplateVisualState(GroupName = ContentStatesGroupName, Name = FloatingPositionName)]
[TemplateVisualState(GroupName = ContentStatesGroupName, Name = FocusedPositionName)]
[TemplateVisualState(GroupName = ContentStatesGroupName, Name = OriginalPositionName)]
public class QuanFloatingText : Control
{
    #region Constants

    public const string ContentStatesGroupName = "ContentStates";

    public const string FloatingPositionName = "FloatingPosition";
    public const string FocusedPositionName = "FocusedPosition";
    public const string OriginalPositionName = "OriginalPosition";

    #endregion

    #region Dependency Properties

    #region FloatingProxy

    public IFloatingProxy FloatingProxy
    {
        get => (IFloatingProxy)GetValue(FloatingProxyProperty);
        set => SetValue(FloatingProxyProperty, value);
    }

    public static readonly DependencyProperty FloatingProxyProperty =
        DependencyProperty.Register(
            nameof(FloatingProxy),
            typeof(IFloatingProxy),
            typeof(QuanFloatingText),
            new PropertyMetadata(null, FloatingProxyPropertyChangedCallback));

    private static void FloatingProxyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not QuanFloatingText quanFloatingText)
        {
            return;
        }

        if (e.OldValue is IFloatingProxy oldHintProxy)
        {
            oldHintProxy.IsVisibleChanged -= quanFloatingText.OnFloatingProxyIsVisibleChanged;
            oldHintProxy.ContentChanged -= quanFloatingText.OnFloatingProxyContentChanged;
            oldHintProxy.Loaded -= quanFloatingText.OnFloatingProxyContentChanged;
            oldHintProxy.FocusedChanged -= quanFloatingText.OnFloatingProxyFocusedChanged;
            oldHintProxy.Dispose();
        }

        if (e.NewValue is IFloatingProxy newHintProxy)
        {
            newHintProxy.IsVisibleChanged += quanFloatingText.OnFloatingProxyIsVisibleChanged;
            newHintProxy.ContentChanged += quanFloatingText.OnFloatingProxyContentChanged;
            newHintProxy.Loaded += quanFloatingText.OnFloatingProxyContentChanged;
            newHintProxy.FocusedChanged += quanFloatingText.OnFloatingProxyFocusedChanged;
            quanFloatingText.RefreshState(false);
        }
    }

    #endregion

    #region IsUseFloating

    public bool IsUseFloating
    {
        get => (bool)GetValue(IsUseFloatingProperty);
        set => SetValue(IsUseFloatingProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsUseFloatingProperty =
        DependencyProperty.Register(
            nameof(IsUseFloating),
            typeof(bool),
            typeof(QuanFloatingText),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #region FloatingText

    public object FloatingText
    {
        get => GetValue(FloatingTextProperty);
        set => SetValue(FloatingTextProperty, value);
    }

    public static readonly DependencyProperty FloatingTextProperty =
        DependencyProperty.Register(
            nameof(FloatingText),
            typeof(object),
            typeof(QuanFloatingText),
            new PropertyMetadata(null));

    #endregion

    #region FloatingScale

    public double FloatingScale
    {
        get => (double)GetValue(FloatingScaleProperty);
        set => SetValue(FloatingScaleProperty, value);
    }

    public static readonly DependencyProperty FloatingScaleProperty =
        DependencyProperty.Register(
            nameof(FloatingScale),
            typeof(double),
            typeof(QuanFloatingText),
            new PropertyMetadata(.75));

    #endregion

    #region FloatingOffset

    public Point FloatingOffset
    {
        get => (Point)GetValue(FloatingOffsetProperty);
        set => SetValue(FloatingOffsetProperty, value);
    }

    public static readonly DependencyProperty FloatingOffsetProperty =
        DependencyProperty.Register(
            nameof(FloatingOffset),
            typeof(Point),
            typeof(QuanFloatingText),
            new PropertyMetadata(new Point(0, -15)));

    #endregion

    #region FloatingOpacity

    public double FloatingOpacity
    {
        get => (double)GetValue(FloatingOpacityProperty);
        set => SetValue(FloatingOpacityProperty, value);
    }

    public static readonly DependencyProperty FloatingOpacityProperty =
        DependencyProperty.Register(
            nameof(FloatingOpacity),
            typeof(double),
            typeof(QuanFloatingText),
            new PropertyMetadata(.46));

    #endregion

    #region IsInFloatingPosition

    public bool IsInFloatingPosition
    {
        get => (bool)GetValue(IsInFloatingPositionProperty);
        set => SetValue(IsInFloatingPositionProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsInFloatingPositionProperty =
        DependencyProperty.Register(
            nameof(IsInFloatingPosition),
            typeof(bool),
            typeof(QuanFloatingText),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #region IsContentNullOrEmpty


    #endregion

    #endregion

    #region Constructor

    static QuanFloatingText()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanFloatingText), new FrameworkPropertyMetadata(typeof(QuanFloatingText)));
    }

    #endregion

    #region Methods

    protected virtual void OnFloatingProxyIsVisibleChanged(object sender, EventArgs e)
        => RefreshState(false);

    protected virtual void OnFloatingProxyContentChanged(object sender, EventArgs e)
    {
        if (FloatingProxy == null)
            return;

        if (FloatingProxy.IsLoaded)
            RefreshState(true);
        else
            FloatingProxy.Loaded += FloatingProxy_OnLoaded;
    }

    private void FloatingProxy_OnLoaded(object sender, EventArgs e)
    {
        if (FloatingProxy == null)
            return;

        RefreshState(false);

        FloatingProxy.Loaded -= FloatingProxy_OnLoaded;
    }

    protected virtual void OnFloatingProxyFocusedChanged(object sender, EventArgs e)
    {
        if (FloatingProxy == null)
            return;

        if (FloatingProxy.IsLoaded)
            RefreshState(true);
        else
            FloatingProxy.Loaded += FloatingProxySetState_OnLoaded;
    }

    private void FloatingProxySetState_OnLoaded(object sender, EventArgs e)
    {
        if (FloatingProxy == null)
            return;

        RefreshState(false);

        FloatingProxy.Loaded -= FloatingProxySetState_OnLoaded;
    }

    private void RefreshState(bool useTransitions)
    {
        var proxy = FloatingProxy;

        if (proxy == null)
            return;

        if (!proxy.IsVisible)
            return;

        var action = new Action(() =>
        {
            var state = string.Empty;

            var isEmpty = proxy.IsEmpty();
            var isFocused = proxy.IsFocused();

            if (IsUseFloating)
                state = !isEmpty || isFocused ? FloatingPositionName : OriginalPositionName;
            else
            {
                if (isFocused)
                    state = isEmpty ? FocusedPositionName : FloatingPositionName;
                else
                    state = isEmpty ? OriginalPositionName : FloatingPositionName;
            }

            IsInFloatingPosition = state == FloatingPositionName;

            VisualStateManager.GoToState(this, state, useTransitions);
        });

        if (DesignerHelper.IsInDesignMode)
            action();
        else
            Dispatcher.InvokeAsync(action);
    }

    #endregion
}