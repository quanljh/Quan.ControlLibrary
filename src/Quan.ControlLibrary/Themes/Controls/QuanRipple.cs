using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Quan.ControlLibrary;

[TemplateVisualState(GroupName = CommonStatesGroupName, Name = NormalStateName)]
[TemplateVisualState(GroupName = CommonStatesGroupName, Name = MouseDownStateName)]
[TemplateVisualState(GroupName = CommonStatesGroupName, Name = MouseUpStateName)]
public class QuanRipple : ContentControl
{
    #region Constants

    public const string CommonStatesGroupName = "CommonStates";

    public const string NormalStateName = "Normal";
    public const string MouseDownStateName = "MouseDown";
    public const string MouseUpStateName = "MouseUp";

    #endregion

    #region Fields

    private static readonly HashSet<QuanRipple> PressedInstances = [];

    #endregion

    #region Dependency Properties

    #region RecognizesAccessKey

    public bool RecognizesAccessKey
    {
        get => (bool)GetValue(RecognizesAccessKeyProperty);
        set => SetValue(RecognizesAccessKeyProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty RecognizesAccessKeyProperty =
        DependencyProperty.Register(
            nameof(RecognizesAccessKey),
            typeof(bool),
            typeof(QuanRipple),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #region Fill

    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register(
            nameof(Fill),
            typeof(Brush),
            typeof(QuanRipple),
            new PropertyMetadata(default(Brush)));

    #endregion

    #region Diameter

    public double Diameter
    {
        get => (double)GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public static readonly DependencyProperty DiameterProperty =
        DependencyProperty.Register(
            nameof(Diameter),
            typeof(double),
            typeof(QuanRipple),
            new PropertyMetadata(default(double)));

    #endregion

    #region RippleX

    public double RippleX
    {
        get => (double)GetValue(RippleXProperty);
        set => SetValue(RippleXProperty, value);
    }

    public static readonly DependencyProperty RippleXProperty =
        DependencyProperty.Register(
            nameof(RippleX),
            typeof(double),
            typeof(QuanRipple),
            new PropertyMetadata(default(double)));

    #endregion

    #region RippleY

    public double RippleY
    {
        get => (double)GetValue(RippleYProperty);
        set => SetValue(RippleYProperty, value);
    }

    public static readonly DependencyProperty RippleYProperty =
        DependencyProperty.Register(
            nameof(RippleY),
            typeof(double),
            typeof(QuanRipple),
            new PropertyMetadata(default(double)));

    #endregion

    #endregion

    #region Constructor

    static QuanRipple()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanRipple), new FrameworkPropertyMetadata(typeof(QuanRipple)));

        // Button control already handled Left button up event, so we register event handle handles the event which had been marked handled event.
        // See https://stackoverflow.com/questions/12496258/mouseleftbuttonup-does-not-fire
        EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(PreviewMouseButtonUpEventHandler), true);
        EventManager.RegisterClassHandler(typeof(ContentControl), Mouse.MouseMoveEvent, new MouseEventHandler(MouseMoveEventHandler), true);
        EventManager.RegisterClassHandler(typeof(Popup), Mouse.PreviewMouseUpEvent, new MouseButtonEventHandler(PreviewMouseButtonUpEventHandler), true);
        EventManager.RegisterClassHandler(typeof(Popup), Mouse.MouseMoveEvent, new MouseEventHandler(MouseMoveEventHandler), true);
    }

    public QuanRipple()
    {
        SizeChanged += OnSizeChanged;
    }

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        VisualStateManager.GoToState(this, NormalStateName, false);
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (RippleHelper.GetIsCentered(this))
        {
            if (Content is FrameworkElement innerContent)
            {
                var position = innerContent.TransformToAncestor(this).Transform(new Point(0, 0));

                if (FlowDirection == FlowDirection.RightToLeft)
                    RippleX = position.X - innerContent.ActualWidth / 2 - Diameter / 2;
                else
                    RippleX = position.X + innerContent.ActualWidth / 2 - Diameter / 2;
                RippleY = position.Y + innerContent.ActualHeight / 2 - Diameter / 2;
            }
            else
            {
                RippleX = ActualWidth / 2 - Diameter / 2;
                RippleY = ActualHeight / 2 - Diameter / 2;
            }
        }
        else
        {
            var point = e.GetPosition(this);
            // The eclipse's center is in the lower right corner.
            // The left offset plus point x equals eclipse's radius
            RippleX = point.X - Diameter / 2;
            // The top offset plus point y equals eclipse's radius
            RippleY = point.Y - Diameter / 2;
        }

        if (!RippleHelper.GetIsDisabled(this))
        {
            VisualStateManager.GoToState(this, NormalStateName, false);
            VisualStateManager.GoToState(this, MouseDownStateName, true);
            PressedInstances.Add(this);
        }

        base.OnPreviewMouseLeftButtonDown(e);
    }

    #endregion

    #region Methods

    private static void PreviewMouseButtonUpEventHandler(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left)
            return;

        foreach (var quanRipple in PressedInstances)
        {
            // Adjust the transition scale time according to the current animated scale
            if (!(quanRipple.Template.FindName("ScaleTransform", quanRipple) is ScaleTransform scaleTrans))
                continue;

            var currentScale = scaleTrans.ScaleX;
            var newTime = TimeSpan.FromMilliseconds(300 * (1.0 - currentScale));

            if (quanRipple.Template.FindName("MouseDownToNormalScaleXKeyFrame", quanRipple) is EasingDoubleKeyFrame scaleXKeyFrame)
                scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);

            if (quanRipple.Template.FindName("MouseDownToNormalScaleYKeyFrame", quanRipple) is EasingDoubleKeyFrame scaleYKeyFrame)
                scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(newTime);

            VisualStateManager.GoToState(quanRipple, NormalStateName, true);
        }
        PressedInstances.Clear();
    }

    private static void MouseMoveEventHandler(object sender, MouseEventArgs e)
    {
        foreach (var quanRipple in PressedInstances.ToList())
        {
            var relativePosition = Mouse.GetPosition(quanRipple);
            if (relativePosition.X < 0
                || relativePosition.Y < 0
                || relativePosition.X >= quanRipple.ActualWidth
                || relativePosition.Y >= quanRipple.ActualHeight)
            {
                VisualStateManager.GoToState(quanRipple, MouseUpStateName, true);
                PressedInstances.Remove(quanRipple);
            }
        }
    }



    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var innerContent = (Content as FrameworkElement);

        double width, height;

        if (RippleHelper.GetIsCentered(this) && innerContent != null)
        {
            width = innerContent.ActualWidth;
            height = innerContent.ActualHeight;
        }
        else
        {
            width = e.NewSize.Width;
            height = e.NewSize.Height;
        }

        var radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));

        Diameter = 2 * radius * RippleHelper.GetRadiusMultiplier(this);
    }

    #endregion
}