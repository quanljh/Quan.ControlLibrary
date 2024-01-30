using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ControlzEx.Theming;
using Quan.ControlLibrary.Events;
using Quan.ControlLibrary.Helpers;

namespace Quan.ControlLibrary.Controls;

[TemplatePart(Name = "PART_Min", Type = typeof(Button))]
[TemplatePart(Name = "PART_Max", Type = typeof(Button))]
[TemplatePart(Name = "PART_Close", Type = typeof(Button))]
[StyleTypedProperty(Property = nameof(LightMinButtonStyle), StyleTargetType = typeof(Button))]
[StyleTypedProperty(Property = nameof(LightMaxButtonStyle), StyleTargetType = typeof(Button))]
[StyleTypedProperty(Property = nameof(LightCloseButtonStyle), StyleTargetType = typeof(Button))]
[StyleTypedProperty(Property = nameof(DarkMinButtonStyle), StyleTargetType = typeof(Button))]
[StyleTypedProperty(Property = nameof(DarkMaxButtonStyle), StyleTargetType = typeof(Button))]
[StyleTypedProperty(Property = nameof(DarkCloseButtonStyle), StyleTargetType = typeof(Button))]
public class WindowButtons : ContentControl
{
    #region Events

    public event ClosingWindowEventHandler ClosingWindow;

    public delegate void ClosingWindowEventHandler(object sender, ClosingWindowEventHandlerArgs args);

    #endregion

    #region LightMinButtonStyle

    public Style LightMinButtonStyle
    {
        get => (Style)GetValue(LightMinButtonStyleProperty);
        set => SetValue(LightMinButtonStyleProperty, value);
    }

    public static readonly DependencyProperty LightMinButtonStyleProperty =
        DependencyProperty.Register(
            nameof(LightMinButtonStyle),
            typeof(Style),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region LightMaxButtonStyle

    public Style LightMaxButtonStyle
    {
        get => (Style)GetValue(LightMaxButtonStyleProperty);
        set => SetValue(LightMaxButtonStyleProperty, value);
    }

    public static readonly DependencyProperty LightMaxButtonStyleProperty =
        DependencyProperty.Register(
            nameof(LightMaxButtonStyle),
            typeof(Style),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region LightCloseButtonStyle

    public Style LightCloseButtonStyle
    {
        get => (Style)GetValue(LightCloseButtonStyleProperty);
        set => SetValue(LightCloseButtonStyleProperty, value);
    }

    public static readonly DependencyProperty LightCloseButtonStyleProperty =
        DependencyProperty.Register(
            nameof(LightCloseButtonStyle),
            typeof(Style),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region DarkMinButtonStyle

    public Style DarkMinButtonStyle
    {
        get => (Style)GetValue(DarkMinButtonStyleProperty);
        set => SetValue(DarkMinButtonStyleProperty, value);
    }

    public static readonly DependencyProperty DarkMinButtonStyleProperty =
        DependencyProperty.Register(
            nameof(DarkMinButtonStyle),
            typeof(Style),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region DarkMaxButtonStyle

    public Style DarkMaxButtonStyle
    {
        get => (Style)GetValue(DarkMaxButtonStyleProperty);
        set => SetValue(DarkMaxButtonStyleProperty, value);
    }

    public static readonly DependencyProperty DarkMaxButtonStyleProperty =
        DependencyProperty.Register(
            nameof(DarkMaxButtonStyle),
            typeof(Style),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region DarkCloseButtonStyle

    public Style DarkCloseButtonStyle
    {
        get => (Style)GetValue(DarkCloseButtonStyleProperty);
        set => SetValue(DarkCloseButtonStyleProperty, value);
    }

    public static readonly DependencyProperty DarkCloseButtonStyleProperty =
        DependencyProperty.Register(
            nameof(DarkCloseButtonStyle),
            typeof(Style),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region Theme

    public string Theme
    {
        get => (string)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty =
        DependencyProperty.Register(
            nameof(Theme),
            typeof(string),
            typeof(WindowButtons),
            new PropertyMetadata(ThemeManager.BaseColorLight));

    #endregion

    #region Minimize

    public string Minimize
    {
        get => (string)GetValue(MinimizeProperty);
        set => SetValue(MinimizeProperty, value);
    }

    public static readonly DependencyProperty MinimizeProperty =
        DependencyProperty.Register(
            nameof(Minimize),
            typeof(string),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region Maximize

    public string Maximize
    {
        get => (string)GetValue(MaximizeProperty);
        set => SetValue(MaximizeProperty, value);
    }

    public static readonly DependencyProperty MaximizeProperty =
        DependencyProperty.Register(
            nameof(Maximize),
            typeof(string),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region Close

    public string Close
    {
        get => (string)GetValue(CloseProperty);
        set => SetValue(CloseProperty, value);
    }

    public static readonly DependencyProperty CloseProperty =
        DependencyProperty.Register(
            nameof(Close),
            typeof(string),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region Restore

    public string Restore
    {
        get => (string)GetValue(RestoreProperty);
        set => SetValue(RestoreProperty, value);
    }

    public static readonly DependencyProperty RestoreProperty =
        DependencyProperty.Register(
            nameof(Restore),
            typeof(string),
            typeof(WindowButtons),
            new PropertyMetadata(null));

    #endregion

    #region ParentWindow

    public QuanWindow ParentWindow
    {
        get => (QuanWindow)GetValue(ParentWindowProperty);
        protected set => SetValue(ParentWindowPropertyKey, value);
    }

    internal static readonly DependencyPropertyKey ParentWindowPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(ParentWindow),
                                            typeof(Window),
                                            typeof(WindowButtons),
                                            new PropertyMetadata(null));

    public static readonly DependencyProperty ParentWindowProperty = ParentWindowPropertyKey.DependencyProperty;

    #endregion

    #region Constructor

    static WindowButtons()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowButtons), new FrameworkPropertyMetadata(typeof(WindowButtons)));
    }

    public WindowButtons()
    {
        CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow));
        CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));

        Dispatcher.BeginInvoke(() =>
            {
                if (ParentWindow is null)
                {
                    var window = this.FindVisualParent<Window>();
                    SetValue(ParentWindowPropertyKey, window);
                }

                if (string.IsNullOrWhiteSpace(Minimize))
                {
                    SetCurrentValue(MinimizeProperty, WinApiHelper.GetCaption(900));
                }

                if (string.IsNullOrWhiteSpace(Maximize))
                {
                    SetCurrentValue(MaximizeProperty, WinApiHelper.GetCaption(901));
                }

                if (string.IsNullOrWhiteSpace(Close))
                {
                    SetCurrentValue(CloseProperty, WinApiHelper.GetCaption(905));
                }

                if (string.IsNullOrWhiteSpace(Restore))
                {
                    SetCurrentValue(RestoreProperty, WinApiHelper.GetCaption(903));
                }
            },
            DispatcherPriority.Loaded);
    }

    #endregion

    #region Methods

    private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
    {
        if (ParentWindow != null)
        {
            SystemCommands.MinimizeWindow(ParentWindow);
        }
    }

    private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
    {
        if (ParentWindow != null)
        {
            SystemCommands.MaximizeWindow(ParentWindow);
        }
    }

    private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
    {
        if (ParentWindow != null)
        {
            SystemCommands.RestoreWindow(ParentWindow);
        }
    }

    private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
    {
        if (ParentWindow != null)
        {
            var args = new ClosingWindowEventHandlerArgs();
            ClosingWindow?.Invoke(this, args);

            if (args.Cancelled)
            {
                return;
            }

            SystemCommands.CloseWindow(ParentWindow);
        }
    }

    #endregion
}