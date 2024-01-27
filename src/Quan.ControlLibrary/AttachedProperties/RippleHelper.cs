using System.Windows;
using System.Windows.Media;

namespace Quan.ControlLibrary;

public static class RippleHelper
{
    #region Background

    public static readonly DependencyProperty BackgroundProperty =
        DependencyProperty.RegisterAttached("Background",
            typeof(Brush),
            typeof(RippleHelper),
            new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender));

    public static Brush GetBackground(DependencyObject element) => (Brush)element.GetValue(BackgroundProperty);

    public static void SetBackground(DependencyObject element, Brush value) => element.SetValue(BackgroundProperty, value);

    #endregion

    #region ClipToBounds

    public static readonly DependencyProperty ClipToBoundsProperty =
        DependencyProperty.RegisterAttached("ClipToBounds",
            typeof(bool),
            typeof(RippleHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetClipToBounds(DependencyObject element) => (bool)element.GetValue(ClipToBoundsProperty);

    public static void SetClipToBounds(DependencyObject element, bool value) => element.SetValue(ClipToBoundsProperty, BooleanBoxes.Box(value));

    #endregion

    #region ShowOnTop

    public static readonly DependencyProperty ShowOnTopProperty =
        DependencyProperty.RegisterAttached("ShowOnTop",
            typeof(bool),
            typeof(RippleHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender));

    public static bool GetShowOnTop(DependencyObject element) => (bool)element.GetValue(ShowOnTopProperty);

    public static void SetShowOnTop(DependencyObject element, bool value) => element.SetValue(ShowOnTopProperty, BooleanBoxes.Box(value));

    #endregion

    #region IsCentered

    public static readonly DependencyProperty IsCenteredProperty =
        DependencyProperty.RegisterAttached("IsCentered",
            typeof(bool),
            typeof(RippleHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetIsCentered(DependencyObject element) => (bool)element.GetValue(IsCenteredProperty);

    public static void SetIsCentered(DependencyObject element, bool value) => element.SetValue(IsCenteredProperty, BooleanBoxes.Box(value));

    #endregion

    #region RadiusMultiplier

    public static readonly DependencyProperty RadiusMultiplierProperty =
        DependencyProperty.RegisterAttached("RadiusMultiplier",
            typeof(double),
            typeof(RippleHelper),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.Inherits));

    public static double GetRadiusMultiplier(DependencyObject element) => (double)element.GetValue(RadiusMultiplierProperty);

    public static void SetRadiusMultiplier(DependencyObject element, double value) => element.SetValue(RadiusMultiplierProperty, value);

    #endregion

    #region IsDisabled

    public static readonly DependencyProperty IsDisabledProperty =
        DependencyProperty.RegisterAttached("IsDisabled",
            typeof(bool),
            typeof(RippleHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetIsDisabled(DependencyObject element) => (bool)element.GetValue(IsDisabledProperty);

    public static void SetIsDisabled(DependencyObject element, bool value) => element.SetValue(IsDisabledProperty, BooleanBoxes.Box(value));

    #endregion

}