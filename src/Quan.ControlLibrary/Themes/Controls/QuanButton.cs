using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary;

public class QuanButton : Button
{
    #region Enum

    public enum DisplayType
    {
        Normal,
        Flat,
        OutLined
    }

    #endregion

    #region Dependency Properties

    #region DisplayMode

    public DisplayType DisplayMode
    {
        get => (DisplayType)GetValue(DisplayModeProperty);
        set => SetValue(DisplayModeProperty, QuanButtonDisplayTypeBoxes.Box(value));
    }

    public static readonly DependencyProperty DisplayModeProperty =
        DependencyProperty.Register(
            nameof(DisplayMode),
            typeof(DisplayType),
            typeof(QuanButton),
            new PropertyMetadata(QuanButtonDisplayTypeBoxes.NormalBox));

    #endregion

    #region CornerRaidus

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(QuanButton),
            new PropertyMetadata(default(CornerRadius)));

    #endregion

    #endregion

    #region Constructor

    static QuanButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanButton), new FrameworkPropertyMetadata(typeof(QuanButton)));
    }

    #endregion
}