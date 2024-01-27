using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary;

public class QuanTextBox : TextBox
{
    #region Enum

    public enum DisplayType
    {
        Normal,
        Floating
    }

    #endregion

    #region Dependency Properties

    #region DisplayMode

    public DisplayType DisplayMode
    {
        get => (DisplayType)GetValue(DisplayModeProperty);
        set => SetValue(DisplayModeProperty, QuanTextBoxDisplayTypeBoxes.Box(value));
    }

    public static readonly DependencyProperty DisplayModeProperty =
        DependencyProperty.Register(
            nameof(DisplayMode),
            typeof(DisplayType),
            typeof(QuanTextBox),
            new PropertyMetadata(QuanTextBoxDisplayTypeBoxes.NormalBox));

    #endregion

    #endregion

    #region Constructor

    static QuanTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanTextBox), new FrameworkPropertyMetadata(typeof(QuanTextBox)));
    }

    #endregion

    #region Override


    #endregion
}