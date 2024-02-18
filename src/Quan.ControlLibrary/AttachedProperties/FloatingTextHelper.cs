using System.Windows;
using System.Windows.Media;
using Quan.ControlLibrary.Helpers.Boxes;

namespace Quan.ControlLibrary.AttachedProperties;

public static class FloatingTextHelper
{
    private const double DefaultFloatingScale = 0.75;
    private static readonly Point DefaultFloatingOffset = new Point(0, -15);
    private const double DefaultHintOpacity = 0.46;
    internal static readonly Brush DefaultBackground = new SolidColorBrush(Colors.Transparent);

    #region IsUseFloating

    public static readonly DependencyProperty IsUseFloatingProperty =
        DependencyProperty.RegisterAttached(
            "IsUseFloating",
            typeof(bool),
            typeof(FloatingTextHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetIsUseFloating(DependencyObject element) => (bool)element.GetValue(IsUseFloatingProperty);

    public static void SetIsUseFloating(DependencyObject element, bool value) => element.SetValue(IsUseFloatingProperty, BooleanBoxes.Box(value));

    #endregion

    #region FloatingScale

    public static readonly DependencyProperty FloatingScaleProperty =
        DependencyProperty.RegisterAttached(
            "FloatingScale",
            typeof(double),
            typeof(FloatingTextHelper),
            new FrameworkPropertyMetadata(DefaultFloatingScale, FrameworkPropertyMetadataOptions.Inherits));

    public static double GetFloatingScale(DependencyObject element) => (double)element.GetValue(FloatingScaleProperty);

    public static void SetFloatingScale(DependencyObject element, double value) => element.SetValue(FloatingScaleProperty, value);


    #endregion

    #region FloatingOffset

    public static readonly DependencyProperty FloatingOffsetProperty =
        DependencyProperty.RegisterAttached(
            "FloatingOffset",
            typeof(Point),
            typeof(FloatingTextHelper),
            new FrameworkPropertyMetadata(DefaultFloatingOffset, FrameworkPropertyMetadataOptions.Inherits));

    public static Point GetFloatingOffset(DependencyObject element) => (Point)element.GetValue(FloatingOffsetProperty);

    public static void SetFloatingOffset(DependencyObject element, Point value) => element.SetValue(FloatingOffsetProperty, value);


    #endregion

    #region FloatingOpacity

    public static readonly DependencyProperty FloatingOpacityProperty =
        DependencyProperty.RegisterAttached(
            "FloatingOpacity",
            typeof(double),
            typeof(FloatingTextHelper),
            new FrameworkPropertyMetadata(DefaultHintOpacity, FrameworkPropertyMetadataOptions.Inherits));

    public static double GetFloatingOpacity(DependencyObject element) => (double)element.GetValue(FloatingOpacityProperty);

    public static void SetFloatingOpacity(DependencyObject element, double value) => element.SetValue(FloatingOpacityProperty, value);


    #endregion

    #region Foreground

    public static readonly DependencyProperty ForegroundProperty =
        DependencyProperty.RegisterAttached(
            "Foreground",
            typeof(Brush),
            typeof(FloatingTextHelper),
            new FrameworkPropertyMetadata(default(Brush)));

    public static Brush GetForeground(DependencyObject element) => (Brush)element.GetValue(ForegroundProperty);

    public static void SetForeground(DependencyObject element, Brush value) => element.SetValue(ForegroundProperty, value);


    #endregion
}