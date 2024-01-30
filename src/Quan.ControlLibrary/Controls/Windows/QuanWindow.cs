using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ControlzEx;
using ControlzEx.Native;
using ControlzEx.Theming;
using Microsoft.Xaml.Behaviors;
using Quan.ControlLibrary.Automations;
using Quan.ControlLibrary.Behaviors;
using Quan.ControlLibrary.Enums;
using Quan.ControlLibrary.Events;
using Quan.ControlLibrary.Helpers;
using Quan.ControlLibrary.Helpers.Boxes;
using Windows.Win32;
using Windows.Win32.Foundation;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

[TemplatePart(Name = PART_TitleBarBackground, Type = typeof(ContentPresenter))]
[TemplatePart(Name = PART_Icon, Type = typeof(Image))]
[TemplatePart(Name = PART_TitleBar, Type = typeof(UIElement))]
[TemplatePart(Name = PART_TitleThumb, Type = typeof(Thumb))]
[TemplatePart(Name = PART_FlyoutModalDragMoveThumb, Type = typeof(Thumb))]
[TemplatePart(Name = PART_LeftWindowCommands, Type = typeof(ContentPresenter))]
[TemplatePart(Name = PART_RightWindowCommands, Type = typeof(ContentPresenter))]
[TemplatePart(Name = PART_WindowButtons, Type = typeof(ContentPresenter))]
[TemplatePart(Name = PART_Content, Type = typeof(ContentControl))]
[TemplatePart(Name = PART_InactiveDialogsContainer, Type = typeof(Grid))]
[TemplatePart(Name = PART_ActiveDialogsContainer, Type = typeof(Grid))]
[TemplatePart(Name = PART_OverlayBox, Type = typeof(Grid))]
[TemplatePart(Name = PART_Flyouts, Type = typeof(UIElement))]
[TemplatePart(Name = PART_FlyoutModal, Type = typeof(Rectangle))]
public class QuanWindow : WindowChromeWindow
{
    #region Private Members

    private const string PART_TitleBarBackground = "PART_TitleBarBackground";
    private const string PART_Icon = "PART_Icon";
    private const string PART_TitleBar = "PART_TitleBar";
    private const string PART_TitleThumb = "PART_TitleThumb";
    private const string PART_FlyoutModalDragMoveThumb = "PART_FlyoutModalDragMoveThumb";
    private const string PART_LeftWindowCommands = "PART_LeftWindowCommands";
    private const string PART_RightWindowCommands = "PART_RightWindowCommands";
    private const string PART_WindowButtons = "PART_WindowButtons";
    private const string PART_Content = "PART_Content";
    private const string PART_InactiveDialogsContainer = "PART_InactiveDialogsContainer";
    private const string PART_ActiveDialogsContainer = "PART_ActiveDialogsContainer";
    private const string PART_OverlayBox = "PART_OverlayBox";
    private const string PART_Flyouts = "PART_Flyouts";
    private const string PART_FlyoutModal = "PART_FlyoutModal";

    private UIElement _titleBarBackground;
    private UIElement _titleBar;
    private FrameworkElement _icon;
    private Thumb _titleThumb;
    private Thumb _flyoutModalDragMoveThumb;
    private ContentPresenter _leftWindowCommandsPresenter;
    private ContentPresenter _rightWindowCommandsPresenter;
    private ContentPresenter _windowButtonsPresenter;

    internal Grid _inactiveDialogContainer;
    internal Grid _activeDialogContainer;
    internal Grid _overlayBox;
    private UIElement _flyouts;
    private Rectangle _flyoutModal;

    #endregion

    #region Properties

    protected internal IntPtr CriticalHandle
    {
        get
        {
            VerifyAccess();
            var value = typeof(Window)
                .GetProperty("CriticalHandle", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(this, Array.Empty<object>()) ?? IntPtr.Zero;
            return (IntPtr)value;
        }
    }

    #endregion

    #region Dependency Properties

    #region Title Related

    #region ShowTitleBar

    public bool ShowTitleBar
    {
        get => (bool)GetValue(ShowTitleBarProperty);
        set => SetValue(ShowTitleBarProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowTitleBarProperty =
        DependencyProperty.Register(
            nameof(ShowTitleBar),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnShowTitleBarPropertyChangedCallback));


    #endregion

    #region TitleTemplate

    public DataTemplate TitleTemplate
    {
        get => (DataTemplate)GetValue(TitleTemplateProperty);
        set => SetValue(TitleTemplateProperty, value);
    }

    public static readonly DependencyProperty TitleTemplateProperty =
        DependencyProperty.Register(
            nameof(TitleTemplate),
            typeof(DataTemplate),
            typeof(QuanWindow),
            new PropertyMetadata(null));

    #endregion

    #region TitleAlignment

    public HorizontalAlignment TitleAlignment
    {
        get => (HorizontalAlignment)GetValue(TitleAlignmentProperty);
        set => SetValue(TitleAlignmentProperty, value);
    }

    public static readonly DependencyProperty TitleAlignmentProperty
        = DependencyProperty.Register(nameof(TitleAlignment),
            typeof(HorizontalAlignment),
            typeof(QuanWindow),
            new PropertyMetadata(HorizontalAlignment.Stretch));

    #endregion

    #region TitleBarHeight

    public int TitleBarHeight
    {
        get => (int)GetValue(TitleBarHeightProperty);
        set => SetValue(TitleBarHeightProperty, value);
    }

    public static readonly DependencyProperty TitleBarHeightProperty =
        DependencyProperty.Register(
            nameof(TitleBarHeight),
            typeof(int),
            typeof(QuanWindow),
            new PropertyMetadata(30, TitleBarHeightPropertyChangedCallback));

    private static void TitleBarHeightPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue)
        {
            ((QuanWindow)d).UpdateTitleBarElementsVisibility();
        }
    }

    #endregion

    #region TitleBarBackground

    public Brush TitleBarBackground
    {
        get => (Brush)GetValue(TitleBarBackgroundProperty);
        set => SetValue(TitleBarBackgroundProperty, value);
    }

    public static readonly DependencyProperty TitleBarBackgroundProperty =
        DependencyProperty.Register(
            nameof(TitleBarBackground),
            typeof(Brush),
            typeof(QuanWindow),
            new PropertyMetadata(Brushes.White));

    #endregion

    #region TitleForeground

    public Brush TitleForeground
    {
        get => (Brush)GetValue(TitleForegroundProperty);
        set => SetValue(TitleForegroundProperty, value);
    }

    public static readonly DependencyProperty TitleForegroundProperty =
        DependencyProperty.Register(
            nameof(TitleForeground),
            typeof(Brush),
            typeof(QuanWindow),
            new PropertyMetadata());

    #endregion

    #region NonActiveTitleBrush

    public Brush NonActiveTitleBrush
    {
        get => (Brush)GetValue(NonActiveTitleBrushProperty);
        set => SetValue(NonActiveTitleBrushProperty, value);
    }

    public static readonly DependencyProperty NonActiveTitleBrushProperty =
        DependencyProperty.Register(
            nameof(NonActiveTitleBrush),
            typeof(Brush),
            typeof(QuanWindow),
            new PropertyMetadata(Brushes.Gray));

    #endregion

    #region TitleCharacterCasing

    public CharacterCasing TitleCharacterCasing
    {
        get => (CharacterCasing)GetValue(TitleCharacterCasingProperty);
        set => SetValue(TitleCharacterCasingProperty, value);
    }

    public static readonly DependencyProperty TitleCharacterCasingProperty =
        DependencyProperty.Register(nameof(TitleCharacterCasing),
            typeof(CharacterCasing),
            typeof(QuanWindow),
            new FrameworkPropertyMetadata(CharacterCasing.Normal, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper);

    #endregion

    #endregion

    #region Button Related

    #region IsMinButtonEnabled

    public bool IsMinButtonEnabled
    {
        get => (bool)GetValue(IsMinButtonEnabledProperty);
        set => SetValue(IsMinButtonEnabledProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsMinButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(IsMinButtonEnabled),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region IsMaxRestoreButtonEnabled

    public bool IsMaxRestoreButtonEnabled
    {
        get => (bool)GetValue(IsMaxRestoreButtonEnabledProperty);
        set => SetValue(IsMaxRestoreButtonEnabledProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsMaxRestoreButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(IsMaxRestoreButtonEnabled),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region IsCloseButtonEnabled

    public bool IsCloseButtonEnabled
    {
        get => (bool)GetValue(IsCloseButtonEnabledProperty);
        set => SetValue(IsCloseButtonEnabledProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsCloseButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(IsCloseButtonEnabled),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region ShowCloseButton

    public bool ShowCloseButton
    {
        get => (bool)GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowCloseButtonProperty =
        DependencyProperty.Register(
            nameof(ShowCloseButton),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region WindowButtons

    public WindowButtons WindowButtons
    {
        get => (WindowButtons)GetValue(WindowButtonsProperty);
        set => SetValue(WindowButtonsProperty, value);
    }


    public static readonly DependencyProperty WindowButtonsProperty =
        DependencyProperty.Register(
            nameof(WindowButtons),
            typeof(WindowButtons),
            typeof(QuanWindow),
            new PropertyMetadata(null, UpdateLogicalChildren));

    private static void UpdateLogicalChildren(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not QuanWindow window)
        {
            return;
        }

        if (e.OldValue is FrameworkElement oldChild)
        {
            window.RemoveLogicalChild(oldChild);
        }

        if (e.NewValue is FrameworkElement newChild)
        {
            window.AddLogicalChild(newChild);
            // Yes, that's crazy. But we must do this to enable all possible scenarios for setting DataContext
            // in a Window. Without set the DataContext at this point it can happen that e.g. a Flyout
            // doesn't get the same DataContext.
            // So now we can type
            //
            // this.InitializeComponent();
            // this.DataContext = new MainViewModel();
            //
            // or
            //
            // this.DataContext = new MainViewModel();
            // this.InitializeComponent();
            //
            newChild.DataContext = window.DataContext;
        }
    }

    #endregion

    #region LeftWindowCommands

    public WindowCommands LeftWindowCommands
    {
        get => (WindowCommands)GetValue(LeftWindowCommandsProperty);
        set => SetValue(LeftWindowCommandsProperty, value);
    }

    public static readonly DependencyProperty LeftWindowCommandsProperty =
        DependencyProperty.Register(
            nameof(LeftWindowCommands),
            typeof(WindowCommands),
            typeof(QuanWindow),
            new PropertyMetadata(null, OnLeftWindowCommandsPropertyChanged));

    private static void OnLeftWindowCommandsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is WindowCommands windowCommands)
        {
            AutomationProperties.SetName(windowCommands, nameof(LeftWindowCommands));
        }

        UpdateLogicalChildren(d, e);
    }

    #endregion

    #region RightWindowCommands

    public WindowCommands RightWindowCommands
    {
        get => (WindowCommands)GetValue(RightWindowCommandsProperty);
        set => SetValue(RightWindowCommandsProperty, value);
    }

    public static readonly DependencyProperty RightWindowCommandsProperty =
        DependencyProperty.Register(
            nameof(RightWindowCommands),
            typeof(WindowCommands),
            typeof(QuanWindow),
            new PropertyMetadata(null, OnRightWindowCommandsPropertyChanged));

    private static void OnRightWindowCommandsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is WindowCommands windowCommands)
        {
            AutomationProperties.SetName(windowCommands, nameof(RightWindowCommands));
        }

        UpdateLogicalChildren(d, e);
    }

    #endregion

    #region LeftWindowCommandsOverlayBehavior

    /// <summary>
    /// Gets or sets the overlay behavior for the <see cref="WindowCommands"/> host on the left side.
    /// </summary>
    public WindowCommandsOverlayBehavior LeftWindowCommandsOverlayBehavior
    {
        get => (WindowCommandsOverlayBehavior)GetValue(LeftWindowCommandsOverlayBehaviorProperty);
        set => SetValue(LeftWindowCommandsOverlayBehaviorProperty, value);
    }

    public static readonly DependencyProperty LeftWindowCommandsOverlayBehaviorProperty =
        DependencyProperty.Register(
            nameof(LeftWindowCommandsOverlayBehavior),
            typeof(WindowCommandsOverlayBehavior),
            typeof(QuanWindow),
            new PropertyMetadata(WindowCommandsOverlayBehavior.Never, OnShowTitleBarPropertyChangedCallback));

    #endregion

    #region RightWindowCommandsOverlayBehavior

    /// <summary>
    /// Gets or sets the overlay behavior for the <see cref="WindowCommands"/> host on the right side.
    /// </summary>
    public WindowCommandsOverlayBehavior RightWindowCommandsOverlayBehavior
    {
        get => (WindowCommandsOverlayBehavior)GetValue(RightWindowCommandsOverlayBehaviorProperty);
        set => SetValue(RightWindowCommandsOverlayBehaviorProperty, value);
    }

    /// <summary>Identifies the <see cref="RightWindowCommandsOverlayBehavior"/> dependency property.</summary>
    public static readonly DependencyProperty RightWindowCommandsOverlayBehaviorProperty =
        DependencyProperty.Register(
            nameof(RightWindowCommandsOverlayBehavior),
            typeof(WindowCommandsOverlayBehavior),
            typeof(QuanWindow),
            new PropertyMetadata(WindowCommandsOverlayBehavior.Never, OnShowTitleBarPropertyChangedCallback));

    #endregion

    #region WindowButtonsOverlayBehavior

    /// <summary>
    /// Gets or sets the overlay behavior for the <see cref="WindowButtons"/> host.
    /// </summary>
    public OverlayBehavior WindowButtonsOverlayBehavior
    {
        get => (OverlayBehavior)GetValue(WindowButtonsOverlayBehaviorProperty);
        set => SetValue(WindowButtonsOverlayBehaviorProperty, value);
    }

    public static readonly DependencyProperty WindowButtonsOverlayBehaviorProperty =
        DependencyProperty.Register(
            nameof(WindowButtonsOverlayBehavior),
            typeof(OverlayBehavior),
            typeof(QuanWindow),
            new PropertyMetadata(OverlayBehavior.Always, OnShowTitleBarPropertyChangedCallback));

    #endregion

    #region OverrideDefaultWindowCommandsBrush

    /// <summary>
    /// Allows easy handling of <see cref="WindowCommands"/> brush. Theme is also applied based on this brush.
    /// </summary>
    public Brush OverrideDefaultWindowCommandsBrush
    {
        get => (Brush)GetValue(OverrideDefaultWindowCommandsBrushProperty);
        set => SetValue(OverrideDefaultWindowCommandsBrushProperty, value);
    }


    public static readonly DependencyProperty OverrideDefaultWindowCommandsBrushProperty =
        DependencyProperty.Register(
            nameof(OverrideDefaultWindowCommandsBrush),
            typeof(Brush),
            typeof(QuanWindow));

    #endregion

    #endregion

    #region Icon Related

    #region ShowIconOnTitleBar

    public bool ShowIconOnTitleBar
    {
        get => (bool)GetValue(ShowIconOnTitleBarProperty);
        set => SetValue(ShowIconOnTitleBarProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowIconOnTitleBarProperty =
        DependencyProperty.Register(
            nameof(ShowIconOnTitleBar),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnShowIconOnTitleBarPropertyChangedCallback));

    private static void OnShowIconOnTitleBarPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var window = (QuanWindow)d;
        if (e.NewValue != e.OldValue)
        {
            window.UpdateIconVisibility();
        }
    }

    #endregion

    #region IconEdgeMode

    public EdgeMode IconEdgeMode
    {
        get => (EdgeMode)GetValue(IconEdgeModeProperty);
        set => SetValue(IconEdgeModeProperty, value);
    }

    public static readonly DependencyProperty IconEdgeModeProperty =
        DependencyProperty.Register(
            nameof(IconEdgeMode),
            typeof(EdgeMode),
            typeof(QuanWindow),
            new PropertyMetadata(EdgeMode.Aliased));

    #endregion

    #region IconBitmapScalingMode

    public BitmapScalingMode IconBitmapScalingMode
    {
        get => (BitmapScalingMode)GetValue(IconBitmapScalingModeProperty);
        set => SetValue(IconBitmapScalingModeProperty, value);
    }

    public static readonly DependencyProperty IconBitmapScalingModeProperty =
        DependencyProperty.Register(
            nameof(IconBitmapScalingMode),
            typeof(BitmapScalingMode),
            typeof(QuanWindow),
            new PropertyMetadata(BitmapScalingMode.HighQuality));

    #endregion

    #region IconScalingMode

    public MultiFrameImageMode IconScalingMode
    {
        get => (MultiFrameImageMode)GetValue(IconScalingModeProperty);
        set => SetValue(IconScalingModeProperty, value);
    }

    public static readonly DependencyProperty IconScalingModeProperty =
        DependencyProperty.Register(
            nameof(IconScalingMode),
            typeof(MultiFrameImageMode),
            typeof(QuanWindow),
            new FrameworkPropertyMetadata(MultiFrameImageMode.ScaleDownLargerFrame, FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region IconTemplate

    public DataTemplate IconTemplate
    {
        get => (DataTemplate)GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly DependencyProperty IconTemplateProperty =
        DependencyProperty.Register(
            nameof(IconTemplate),
            typeof(DataTemplate),
            typeof(QuanWindow),
            new PropertyMetadata());

    #endregion

    #region IconOverlayBehavior

    public OverlayBehavior IconOverlayBehavior
    {
        get => (OverlayBehavior)GetValue(IconOverlayBehaviorProperty);
        set => SetValue(IconOverlayBehaviorProperty, value);
    }

    public static readonly DependencyProperty IconOverlayBehaviorProperty =
        DependencyProperty.Register(nameof(IconOverlayBehavior),
            typeof(OverlayBehavior),
            typeof(QuanWindow),
            new PropertyMetadata(OverlayBehavior.Never, OnShowTitleBarPropertyChangedCallback));

    #endregion

    #region CloseOnIconDoubleClick

    /// <summary>
    /// Gets or sets the value to close the window if the user double-click on the window icon.
    /// </summary>
    public bool CloseOnIconDoubleClick
    {
        get => (bool)GetValue(CloseOnIconDoubleClickProperty);
        set => SetValue(CloseOnIconDoubleClickProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty CloseOnIconDoubleClickProperty =
        DependencyProperty.Register(
            nameof(CloseOnIconDoubleClick),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #endregion

    #region Flyout Related

    #region ShowFlyoutsOverDialogs

    /// <summary>
    /// Get or sets whether a Flyout will be shown over Dialogs.
    /// </summary>
    public bool ShowFlyoutsOverDialogs
    {
        get => (bool)GetValue(ShowFlyoutsOverDialogsProperty);
        set => SetValue(ShowFlyoutsOverDialogsProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowFlyoutsOverDialogsProperty
        = DependencyProperty.Register(nameof(ShowFlyoutsOverDialogs),
            typeof(bool),
            typeof(QuanWindow),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, null, CoerceValueShowFlyoutsOverDialogs));

    private static object CoerceValueShowFlyoutsOverDialogs(DependencyObject d, object baseValue)
    {
        if (d is QuanWindow { Flyouts: not null } window)
        {
            // It's not allowed to change this if any Flyout is open
            // TODO Find a way to reset the zIndex of any open Flyout if <see cref="ShowFlyoutsOverDialogs"/> is changed by the user 
            var anyFlyoutOpen = window.Flyouts.GetFlyouts().Any(f => f.IsOpen);
            if (anyFlyoutOpen)
            {
                return false;
            }
        }

        return baseValue;
    }

    #endregion

    #region FlyoutOverlayBrush

    public Brush FlyoutOverlayBrush
    {
        get => (Brush)GetValue(FlyoutOverlayBrushProperty);
        set => SetValue(FlyoutOverlayBrushProperty, value);
    }

    public static readonly DependencyProperty FlyoutOverlayBrushProperty =
        DependencyProperty.Register(
            nameof(FlyoutOverlayBrush),
            typeof(Brush),
            typeof(QuanWindow),
            new PropertyMetadata(null));

    #endregion

    #region Flyouts

    /// <summary>
    /// Gets or sets a <see cref="FlyoutsControl"/> host for the <see cref="Flyout"/> controls.
    /// </summary>
    public FlyoutsControl Flyouts
    {
        get => (FlyoutsControl)GetValue(FlyoutsProperty);
        set => SetValue(FlyoutsProperty, value);
    }

    public static readonly DependencyProperty FlyoutsProperty =
        DependencyProperty.Register(
            nameof(Flyouts),
            typeof(FlyoutsControl),
            typeof(QuanWindow),
            new PropertyMetadata(null, UpdateLogicalChildren));

    #endregion

    #endregion

    #region Dialog Related

    #region ShowDialogsOverTitleBar

    public bool ShowDialogsOverTitleBar
    {
        get => (bool)GetValue(ShowDialogsOverTitleBarProperty);
        set => SetValue(ShowDialogsOverTitleBarProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowDialogsOverTitleBarProperty =
        DependencyProperty.Register(
            nameof(ShowDialogsOverTitleBar),
            typeof(bool),
            typeof(QuanWindow),
            new FrameworkPropertyMetadata(
                BooleanBoxes.TrueBox,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region IsAnyDialogOpen

    public bool IsAnyDialogOpen
    {
        get => (bool)GetValue(IsAnyDialogOpenProperty);
        protected set => SetValue(IsAnyDialogOpenPropertyKey, BooleanBoxes.Box(value));
    }

    internal static readonly DependencyPropertyKey IsAnyDialogOpenPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(IsAnyDialogOpen),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    public static readonly DependencyProperty IsAnyDialogOpenProperty = IsAnyDialogOpenPropertyKey.DependencyProperty;

    #endregion

    #region IsCloseButtonEnabledWithDialog

    public bool IsCloseButtonEnabledWithDialog
    {
        get => (bool)GetValue(IsCloseButtonEnabledWithDialogProperty);
        protected set => SetValue(IsCloseButtonEnabledWithDialogPropertyKey, BooleanBoxes.Box(value));
    }

    internal static readonly DependencyPropertyKey IsCloseButtonEnabledWithDialogPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(IsCloseButtonEnabledWithDialog),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    public static readonly DependencyProperty IsCloseButtonEnabledWithDialogProperty = IsCloseButtonEnabledWithDialogPropertyKey.DependencyProperty;

    #endregion

    #region QuanDialogOptions

    /// <summary>
    /// Gets or sets the default settings for the dialogs.
    /// </summary>
    public QuanDialogSettings QuanDialogOptions
    {
        get => (QuanDialogSettings)GetValue(QuanDialogOptionsProperty);
        set => SetValue(QuanDialogOptionsProperty, value);
    }

    /// <summary>Identifies the <see cref="QuanDialogOptions"/> dependency property.</summary>
    public static readonly DependencyProperty QuanDialogOptionsProperty
        = DependencyProperty.Register(nameof(QuanDialogOptions),
            typeof(QuanDialogSettings),
            typeof(QuanWindow),
            new PropertyMetadata(default(QuanDialogSettings)));

    #endregion

    #endregion

    #region NonActiveBorderBrush

    public Brush NonActiveBorderBrush
    {
        get => (Brush)GetValue(NonActiveBorderBrushProperty);
        set => SetValue(NonActiveBorderBrushProperty, value);
    }

    public static readonly DependencyProperty NonActiveBorderBrushProperty =
        DependencyProperty.Register(
            nameof(NonActiveBorderBrush),
            typeof(Brush),
            typeof(QuanWindow),
            new PropertyMetadata(Brushes.Gray));

    #endregion

    #region OverlayBrush

    public Brush OverlayBrush
    {
        get => (Brush)GetValue(OverlayBrushProperty);
        set => SetValue(OverlayBrushProperty, value);
    }

    public static readonly DependencyProperty OverlayBrushProperty =
        DependencyProperty.Register(
            nameof(OverlayBrush),
            typeof(Brush),
            typeof(QuanWindow),
            new PropertyMetadata(null));

    #endregion

    #region OverlayFadeIn

    public static readonly DependencyProperty OverlayFadeInProperty =
        DependencyProperty.Register(
            nameof(OverlayFadeIn),
            typeof(Storyboard),
            typeof(QuanWindow),
            new PropertyMetadata(default(Storyboard)));

    public Storyboard OverlayFadeIn
    {
        get => (Storyboard)GetValue(OverlayFadeInProperty);
        set => SetValue(OverlayFadeInProperty, value);
    }

    #endregion

    #region OverlayFadeOut
    public Storyboard OverlayFadeOut
    {
        get => (Storyboard)GetValue(OverlayFadeOutProperty);
        set => SetValue(OverlayFadeOutProperty, value);
    }

    public static readonly DependencyProperty OverlayFadeOutProperty
        = DependencyProperty.Register(
            nameof(OverlayFadeOut),
            typeof(Storyboard),
            typeof(QuanWindow),
            new PropertyMetadata(default(Storyboard)));

    #endregion

    #region IsWindowDraggable

    public bool IsWindowDraggable
    {
        get => (bool)GetValue(IsWindowDraggableProperty);
        set => SetValue(IsWindowDraggableProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsWindowDraggableProperty =
        DependencyProperty.Register(
            nameof(IsWindowDraggable),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region ShowSystemMenuOnRightClick

    public bool ShowSystemMenuOnRightClick
    {
        get => (bool)GetValue(ShowSystemMenuOnRightClickProperty);
        set => SetValue(ShowSystemMenuOnRightClickProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowSystemMenuOnRightClickProperty =
        DependencyProperty.Register(
            nameof(ShowSystemMenuOnRightClick),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region WindowTransitionsEnabled

    public bool WindowTransitionsEnabled
    {
        get => (bool)GetValue(WindowTransitionsEnabledProperty);
        set => SetValue(WindowTransitionsEnabledProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty WindowTransitionsEnabledProperty =
        DependencyProperty.Register(
            nameof(WindowTransitionsEnabled),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region ShowSystemMenu

    /// <summary>
    /// Gets or sets a value that indicates whether the system menu should popup with left mouse click on the window icon.
    /// </summary>
    public bool ShowSystemMenu
    {
        get => (bool)GetValue(ShowSystemMenuProperty);
        set => SetValue(ShowSystemMenuProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowSystemMenuProperty =
        DependencyProperty.Register(
            nameof(ShowSystemMenu),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

    #endregion

    #region WindowPlacementSettings

    /// <summary>
    ///  Gets or sets the settings to save and load the position and size of the window.
    /// </summary>
    public IWindowPlacementSettings WindowPlacementSettings
    {
        get => (IWindowPlacementSettings)GetValue(WindowPlacementSettingsProperty);
        set => SetValue(WindowPlacementSettingsProperty, value);
    }

    public static readonly DependencyProperty WindowPlacementSettingsProperty =
        DependencyProperty.Register(
            nameof(WindowPlacementSettings),
            typeof(IWindowPlacementSettings),
            typeof(QuanWindow),
            new PropertyMetadata(null));

    #endregion

    #region SaveWindowPosition

    /// <summary>
    /// Gets or sets whether the window will save it's position and size.
    /// </summary>
    public bool SaveWindowPosition
    {
        get => (bool)GetValue(SaveWindowPositionProperty);
        set => SetValue(SaveWindowPositionProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty SaveWindowPositionProperty =
        DependencyProperty.Register(
            nameof(SaveWindowPosition),
            typeof(bool),
            typeof(QuanWindow),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #endregion

    #region Events

    #region WindowTransitionCompleted

    public event RoutedEventHandler WindowTransitionCompleted
    {
        add => AddHandler(WindowTransitionCompletedEvent, value);
        remove => RemoveHandler(WindowTransitionCompletedEvent, value);
    }

    public static readonly RoutedEvent WindowTransitionCompletedEvent =
        EventManager.RegisterRoutedEvent(nameof(WindowTransitionCompleted),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(QuanWindow));

    #endregion

    #region FlyoutsStatusChanged

    public event RoutedEventHandler FlyoutsStatusChanged
    {
        add => AddHandler(FlyoutsStatusChangedEvent, value);
        remove => RemoveHandler(FlyoutsStatusChangedEvent, value);
    }

    /// <summary>Identifies the <see cref="FlyoutsStatusChanged"/> routed event.</summary>
    public static readonly RoutedEvent FlyoutsStatusChangedEvent
        = EventManager.RegisterRoutedEvent(nameof(FlyoutsStatusChanged),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(QuanWindow));

    #endregion

    #endregion

    #region Constructor

    static QuanWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanWindow), new FrameworkPropertyMetadata(typeof(QuanWindow)));

        EventManager.RegisterClassHandler(typeof(QuanWindow), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(OnAccessKeyPressed));

        IconProperty.OverrideMetadata(
            typeof(QuanWindow),
            new FrameworkPropertyMetadata(
                (o, e) =>
                {
                    if (e.NewValue != e.OldValue)
                    {
                        (o as QuanWindow)?.UpdateIconVisibility();
                    }
                }));
    }

    public QuanWindow()
    {
        SetCurrentValue(QuanDialogOptionsProperty, new QuanDialogSettings());

        DataContextChanged += QuanWindow_DataContextChanged;
        Loaded += QuanWindow_Loaded;
    }

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _windowButtonsPresenter = GetTemplateChild(PART_WindowButtons) as ContentPresenter;
        _leftWindowCommandsPresenter = GetTemplateChild(PART_LeftWindowCommands) as ContentPresenter;
        _rightWindowCommandsPresenter = GetTemplateChild(PART_RightWindowCommands) as ContentPresenter;

        WindowButtons ??= new WindowButtons();
        LeftWindowCommands ??= new WindowCommands();
        RightWindowCommands ??= new WindowCommands();

        WindowButtons.SetValue(WindowButtons.ParentWindowPropertyKey, this);
        LeftWindowCommands.SetValue(WindowCommands.ParentWindowPropertyKey, this);
        RightWindowCommands.SetValue(WindowCommands.ParentWindowPropertyKey, this);

        _overlayBox = GetTemplateChild(PART_OverlayBox) as Grid;
        _activeDialogContainer = GetTemplateChild(PART_ActiveDialogsContainer) as Grid;
        _inactiveDialogContainer = GetTemplateChild(PART_InactiveDialogsContainer) as Grid;
        _flyouts = GetTemplateChild(PART_Flyouts) as UIElement;
        _flyoutModal = GetTemplateChild(PART_FlyoutModal) as Rectangle;

        if (_flyoutModal is not null)
        {
            _flyoutModal.PreviewMouseDown += FlyoutsPreviewMouseDown;
        }

        PreviewMouseDown += FlyoutsPreviewMouseDown;

        _icon = GetTemplateChild(PART_Icon) as FrameworkElement;
        _titleBar = GetTemplateChild(PART_TitleBar) as UIElement;
        _titleBarBackground = GetTemplateChild(PART_TitleBarBackground) as UIElement;
        _titleThumb = GetTemplateChild(PART_TitleThumb) as Thumb;
        _flyoutModalDragMoveThumb = GetTemplateChild(PART_FlyoutModalDragMoveThumb) as Thumb;

        UpdateTitleBarElementsVisibility();

        if (GetTemplateChild(PART_Content) is QuanContentControl quanContentControl)
        {
            quanContentControl.TransitionCompleted += (_, _) => RaiseEvent(new RoutedEventArgs(WindowTransitionCompletedEvent));
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new QuanThumbContentControlAutomationPeer(this);
    }

    protected override void InitializeBehaviors()
    {
        base.InitializeBehaviors();

        Interaction.GetBehaviors(this).Add(new WindowsSettingBehavior());
    }

    protected override IEnumerator LogicalChildren
    {
        get
        {
            // cheat, make a list with all logical content and return the enumerator
            var children = new ArrayList();
            if (Content != null)
            {
                children.Add(Content);
            }

            if (LeftWindowCommands != null)
            {
                children.Add(LeftWindowCommands);
            }

            if (RightWindowCommands != null)
            {
                children.Add(RightWindowCommands);
            }

            if (WindowButtons != null)
            {
                children.Add(WindowButtons);
            }

            if (Flyouts != null)
            {
                children.Add(Flyouts);
            }

            return children.GetEnumerator();
        }
    }

    #endregion

    #region Methods

    internal T GetPart<T>(string name)
        where T : class
    {
        return GetTemplateChild(name) as T;
    }

    public virtual IWindowPlacementSettings GetWindowPlacementSettings()
    {
        return WindowPlacementSettings ?? new WindowApplicationSettings(this);
    }

    private static void OnShowTitleBarPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue)
        {
            ((QuanWindow)d).UpdateTitleBarElementsVisibility();
        }
    }

    private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
    {
        if (!e.Handled && sender is QuanWindow { IsAnyDialogOpen: true })
        {
            e.Scope = null;
            e.Target = null;
            e.Handled = true;
        }
    }

    private void QuanWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // Add these controls to the window with AddLogicalChild method.
        // This has the side effect that the DataContext doesn't update, so do this now here.
        if (LeftWindowCommands != null)
        {
            LeftWindowCommands.DataContext = DataContext;
        }

        if (RightWindowCommands != null)
        {
            RightWindowCommands.DataContext = DataContext;
        }

        if (WindowButtons != null)
        {
            WindowButtons.DataContext = DataContext;
        }

        if (Flyouts != null)
        {
            Flyouts.DataContext = DataContext;
        }
    }


    private void QuanWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Flyouts ??= new FlyoutsControl();

        this.ResetAllWindowCommandsBrush();

        ThemeManager.Current.ThemeChanged += HandleThemeManagerThemeChanged;
        Unloaded += (_, _) => ThemeManager.Current.ThemeChanged -= HandleThemeManagerThemeChanged;
    }


    private void HandleThemeManagerThemeChanged(object sender, ThemeChangedEventArgs e)
    {
        this.Invoke(() =>
        {
            var flyouts = Flyouts?.GetFlyouts().ToList() ?? [];

            // since we disabled the ThemeManager OnThemeChanged part, we must change all children flyouts too
            // e.g if the FlyoutsControl is hosted in a UserControl
            var allChildFlyouts = (Content as DependencyObject)
                .FindVisualChildren<FlyoutsControl>(true)
                .SelectMany(flyoutsControl => flyoutsControl.GetFlyouts());
            flyouts.AddRange(allChildFlyouts);

            if (!flyouts.Any())
            {
                // we must update the window command brushes!!!
                this.ResetAllWindowCommandsBrush();
                return;
            }

            var newTheme = ReferenceEquals(e.Target, this)
                ? e.NewTheme
                : ThemeManager.Current.DetectTheme(this);

            if (newTheme is null)
            {
                return;
            }

            foreach (var flyout in flyouts)
            {
                flyout.ChangeFlyoutTheme(newTheme);
            }

            this.HandleWindowCommandsForFlyouts(flyouts);
        });
    }


    private void FlyoutsPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.OriginalSource is DependencyObject element)
        {
            // no preview if we just clicked these elements
            if (element.FindVisualParent<Flyout>() != null
                || Equals(element, _overlayBox)
                || element.FindVisualParent<BaseQuanDialog>() != null
                || Equals(element.FindVisualParent<ContentControl>(), _icon)
                || element.FindVisualParent<WindowCommands>() != null
                || element.FindVisualParent<WindowButtons>() != null)
            {
                return;
            }
        }

        if (Flyouts!.OverrideExternalCloseButton is null)
        {
            foreach (var flyout in Flyouts.GetFlyouts().Where(x => x.IsOpen && x.ExternalCloseButton == e.ChangedButton && (!x.IsPinned || Flyouts.OverrideIsPinned)))
            {
                flyout.SetCurrentValue(Flyout.IsOpenProperty, BooleanBoxes.FalseBox);
            }
        }
        else if (Flyouts.OverrideExternalCloseButton == e.ChangedButton)
        {
            foreach (var flyout in Flyouts.GetFlyouts().Where(x => x.IsOpen && (!x.IsPinned || Flyouts.OverrideIsPinned)))
            {
                flyout.SetCurrentValue(Flyout.IsOpenProperty, BooleanBoxes.FalseBox);
            }
        }
    }

    private void UpdateTitleBarElementsVisibility()
    {
        UpdateIconVisibility();

        var newVisibility = TitleBarHeight > 0 && ShowTitleBar ? Visibility.Visible : Visibility.Collapsed;

        _titleBar?.SetCurrentValue(VisibilityProperty, newVisibility);
        _titleBarBackground?.SetCurrentValue(VisibilityProperty, newVisibility);

        var leftWindowCommandsVisibility = LeftWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) ? Visibility.Visible : newVisibility;
        _leftWindowCommandsPresenter?.SetCurrentValue(VisibilityProperty, leftWindowCommandsVisibility);

        var rightWindowCommandsVisibility = RightWindowCommandsOverlayBehavior.HasFlag(WindowCommandsOverlayBehavior.HiddenTitleBar) ? Visibility.Visible : newVisibility;
        _rightWindowCommandsPresenter?.SetCurrentValue(VisibilityProperty, rightWindowCommandsVisibility);

        var windowButtonsVisibility = WindowButtonsOverlayBehavior.HasFlag(OverlayBehavior.HiddenTitleBar) ? Visibility.Visible : newVisibility;
        _windowButtonsPresenter?.SetCurrentValue(VisibilityProperty, windowButtonsVisibility);

        SetWindowEvents();
    }

    private void SetWindowEvents()
    {
        // clear all event handlers first
        ClearWindowEvents();

        // set mouse down/up for icon
        if (_icon is { Visibility: Visibility.Visible })
        {
            _icon.MouseDown += OnIconMouseLeftButtonDown;
        }

        if (_titleThumb != null)
        {
            _titleThumb.PreviewMouseLeftButtonUp += TitleThumbOnPreviewMouseLeftButtonUp;
            _titleThumb.DragDelta += TitleThumbMoveOnDragDelta;
            _titleThumb.MouseDoubleClick += TitleThumbChangeWindowStateOnMouseDoubleClick;
            _titleThumb.MouseRightButtonUp += TitleThumbSystemMenuOnMouseRightButtonUp;
        }

        if (_titleBar is IQuanThumb thumbContentControl)
        {
            thumbContentControl.PreviewMouseLeftButtonUp += TitleThumbOnPreviewMouseLeftButtonUp;
            thumbContentControl.DragDelta += TitleThumbMoveOnDragDelta;
            thumbContentControl.MouseDoubleClick += TitleThumbChangeWindowStateOnMouseDoubleClick;
            thumbContentControl.MouseRightButtonUp += TitleThumbSystemMenuOnMouseRightButtonUp;
        }

        if (_flyoutModalDragMoveThumb != null)
        {
            _flyoutModalDragMoveThumb.PreviewMouseLeftButtonUp += TitleThumbOnPreviewMouseLeftButtonUp;
            _flyoutModalDragMoveThumb.DragDelta += TitleThumbMoveOnDragDelta;
            _flyoutModalDragMoveThumb.MouseDoubleClick += TitleThumbChangeWindowStateOnMouseDoubleClick;
            _flyoutModalDragMoveThumb.MouseRightButtonUp += TitleThumbSystemMenuOnMouseRightButtonUp;
        }

        // handle size if we have a Grid for the title (e.g. clean window have a centered title)
        if (_titleBar != null && TitleAlignment == HorizontalAlignment.Center)
        {
            SizeChanged += QuanWindow_SizeChanged;
        }
    }


    private void ClearWindowEvents()
    {
        if (_titleThumb != null)
        {
            _titleThumb.PreviewMouseLeftButtonUp -= TitleThumbOnPreviewMouseLeftButtonUp;
            _titleThumb.DragDelta -= TitleThumbMoveOnDragDelta;
            _titleThumb.MouseDoubleClick -= TitleThumbChangeWindowStateOnMouseDoubleClick;
            _titleThumb.MouseRightButtonUp -= TitleThumbSystemMenuOnMouseRightButtonUp;
        }

        if (_titleBar is IQuanThumb thumbContentControl)
        {
            thumbContentControl.PreviewMouseLeftButtonUp -= TitleThumbOnPreviewMouseLeftButtonUp;
            thumbContentControl.DragDelta -= TitleThumbMoveOnDragDelta;
            thumbContentControl.MouseDoubleClick -= TitleThumbChangeWindowStateOnMouseDoubleClick;
            thumbContentControl.MouseRightButtonUp -= TitleThumbSystemMenuOnMouseRightButtonUp;
        }

        if (_flyoutModalDragMoveThumb != null)
        {
            _flyoutModalDragMoveThumb.PreviewMouseLeftButtonUp -= TitleThumbOnPreviewMouseLeftButtonUp;
            _flyoutModalDragMoveThumb.DragDelta -= TitleThumbMoveOnDragDelta;
            _flyoutModalDragMoveThumb.MouseDoubleClick -= TitleThumbChangeWindowStateOnMouseDoubleClick;
            _flyoutModalDragMoveThumb.MouseRightButtonUp -= TitleThumbSystemMenuOnMouseRightButtonUp;
        }

        if (_icon != null)
        {
            _icon.MouseLeftButtonDown -= OnIconMouseLeftButtonDown;
        }

        SizeChanged -= QuanWindow_SizeChanged;
    }

    private void OnIconMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2 && CloseOnIconDoubleClick)
        {
            Close();
        }
        else if (ShowSystemMenu)
        {
#pragma warning disable 618
            ControlzEx.SystemCommands.ShowSystemMenuPhysicalCoordinates(this, PointToScreen(new Point(BorderThickness.Left, TitleBarHeight + BorderThickness.Top)));
#pragma warning restore 618
        }
    }

    private void QuanWindow_SizeChanged(object sender, RoutedEventArgs e)
    {
        // this all works only for centered title
        if (TitleAlignment != HorizontalAlignment.Center
            || _titleBar is null)
        {
            return;
        }

        // Half of this QuanWindow
        var halfDistance = ActualWidth / 2;
        // Distance between center and left/right
        var margin = (Thickness)_titleBar.GetValue(MarginProperty);
        var distanceToCenter = (_titleBar.DesiredSize.Width - margin.Left - margin.Right) / 2;

        var iconWidth = _icon?.ActualWidth ?? 0;
        var leftWindowCommandsWidth = LeftWindowCommands?.ActualWidth ?? 0;
        var rightWindowCommandsWidth = RightWindowCommands?.ActualWidth ?? 0;
        var windowButtonsWith = WindowButtons?.ActualWidth ?? 0;

        // Distance between right edge from LeftWindowCommands to left window side
        var distanceFromLeft = iconWidth + leftWindowCommandsWidth;
        // Distance between left edge from RightWindowCommands to right window side
        var distanceFromRight = rightWindowCommandsWidth + windowButtonsWith;
        // Margin
        const double horizontalMargin = 5.0;

        var dLeft = distanceFromLeft + distanceToCenter + horizontalMargin;
        var dRight = distanceFromRight + distanceToCenter + horizontalMargin;
        if (dLeft < halfDistance && dRight < halfDistance)
        {
            _titleBar.SetCurrentValue(MarginProperty, default(Thickness));
            Grid.SetColumn(_titleBar, 0);
            Grid.SetColumnSpan(_titleBar, 3);
        }
        else
        {
            _titleBar.SetCurrentValue(MarginProperty, new Thickness(leftWindowCommandsWidth, 0, rightWindowCommandsWidth, 0));
            Grid.SetColumn(_titleBar, 1);
            Grid.SetColumnSpan(_titleBar, 1);
        }
    }

    private void UpdateIconVisibility()
    {
        var isVisible = (Icon is not null || IconTemplate is not null)
                        && (IconOverlayBehavior.HasFlag(OverlayBehavior.HiddenTitleBar) && !ShowTitleBar || ShowIconOnTitleBar && ShowTitleBar);
        _icon?.SetCurrentValue(VisibilityProperty, isVisible ? Visibility.Visible : Visibility.Collapsed);
    }

    private void TitleThumbOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        DoTitleThumbOnPreviewMouseLeftButtonUp(this, e);
    }

    private void TitleThumbMoveOnDragDelta(object sender, DragDeltaEventArgs dragDeltaEventArgs)
    {
        DoTitleThumbMoveOnDragDelta(sender as IQuanThumb, this, dragDeltaEventArgs);
    }

    private void TitleThumbChangeWindowStateOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
    {
        DoTitleThumbChangeWindowStateOnMouseDoubleClick(this, mouseButtonEventArgs);
    }

    private void TitleThumbSystemMenuOnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
        DoTitleThumbSystemMenuOnMouseRightButtonUp(this, e);
    }

    internal static void DoTitleThumbOnPreviewMouseLeftButtonUp(QuanWindow window, MouseButtonEventArgs mouseButtonEventArgs)
    {
        if (mouseButtonEventArgs.Source == mouseButtonEventArgs.OriginalSource)
        {
            Mouse.Capture(null);
        }
    }

    internal static void DoTitleThumbMoveOnDragDelta(IQuanThumb thumb, QuanWindow window, DragDeltaEventArgs dragDeltaEventArgs)
    {
        if (thumb is null)
        {
            throw new ArgumentNullException(nameof(thumb));
        }

        if (window is null)
        {
            throw new ArgumentNullException(nameof(window));
        }

        // drag only if IsWindowDraggable is set to true
        if (!window.IsWindowDraggable ||
            !(Math.Abs(dragDeltaEventArgs.HorizontalChange) > 2) && !(Math.Abs(dragDeltaEventArgs.VerticalChange) > 2))
        {
            return;
        }

        // This was taken from DragMove internal code
        window.VerifyAccess();

        // if the window is maximized dragging is only allowed on title bar (also if not visible)
        var windowIsMaximized = window.WindowState == WindowState.Maximized;
        var isMouseOnTitlebar = Mouse.GetPosition(thumb).Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
        if (!isMouseOnTitlebar && windowIsMaximized)
        {
            return;
        }

        // for the touch usage
        PInvoke.ReleaseCapture();

        if (windowIsMaximized)
        {
            EventHandler onWindowStateChanged = null;
            onWindowStateChanged = (sender, args) =>
            {
                window.StateChanged -= onWindowStateChanged;

                if (window.WindowState == WindowState.Normal)
                {
                    Mouse.Capture(thumb, CaptureMode.Element);
                }
            };

            window.StateChanged -= onWindowStateChanged;
            window.StateChanged += onWindowStateChanged;
        }

        // these lines are from DragMove
        // NativeMethods.SendMessage(criticalHandle, WM.SYSCOMMAND, (IntPtr)SC.MOUSEMOVE, IntPtr.Zero);
        // NativeMethods.SendMessage(criticalHandle, WM.LBUTTONUP, IntPtr.Zero, IntPtr.Zero);

        var wpfPoint = window.PointToScreen(Mouse.GetPosition(window));
        var x = (int)wpfPoint.X;
        var y = (int)wpfPoint.Y;
        PInvoke.SendMessage(new HWND(window.CriticalHandle), PInvoke.WM_NCLBUTTONDOWN, new WPARAM((nuint)HT.CAPTION), new IntPtr(x | y << 16));
    }

    internal static void DoTitleThumbChangeWindowStateOnMouseDoubleClick(QuanWindow window, MouseButtonEventArgs mouseButtonEventArgs)
    {
        // restore/maximize only with left button
        if (mouseButtonEventArgs.ChangedButton == MouseButton.Left)
        {
            // we can maximize or restore the window if the title bar height is set (also if title bar is hidden)
            var canResize = window.ResizeMode == ResizeMode.CanResizeWithGrip || window.ResizeMode == ResizeMode.CanResize;
            var mousePos = Mouse.GetPosition(window);
            var isMouseOnTitlebar = mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0;
            if (canResize && isMouseOnTitlebar)
            {
#pragma warning disable 618
                if (window.WindowState == WindowState.Normal)
                {
                    ControlzEx.SystemCommands.MaximizeWindow(window);
                }
                else
                {
                    ControlzEx.SystemCommands.RestoreWindow(window);
                }
#pragma warning restore 618
                mouseButtonEventArgs.Handled = true;
            }
        }
    }

    internal static void DoTitleThumbSystemMenuOnMouseRightButtonUp(QuanWindow window, MouseButtonEventArgs e)
    {
        if (window.ShowSystemMenuOnRightClick)
        {
            // show menu only if mouse pos is on title bar or if we have a window with none style and no title bar
            var mousePos = e.GetPosition(window);
            if (mousePos.Y <= window.TitleBarHeight && window.TitleBarHeight > 0 || window.WindowStyle == WindowStyle.None && window.TitleBarHeight <= 0)
            {
#pragma warning disable 618
                ControlzEx.SystemCommands.ShowSystemMenuPhysicalCoordinates(window, window.PointToScreen(mousePos));
#pragma warning restore 618
            }
        }
    }

    internal void HandleFlyoutStatusChange(Flyout flyout, IList<Flyout> visibleFlyouts)
    {
        var zIndexOfFlyoutsControl = _flyouts != null ? Panel.GetZIndex(_flyouts) : 2;

        // Checks a recently opened flyout's position.
        var zIndex = flyout.IsOpen ? Panel.GetZIndex(flyout) + zIndexOfFlyoutsControl + 1 : visibleFlyouts.Count + zIndexOfFlyoutsControl;

        //if the corresponding behavior has the right flag, set the window commands' and icon zIndex to a number that is higher than the Flyout's.
        _icon?.SetValue(Panel.ZIndexProperty, flyout.IsModal && flyout.IsOpen ? 0 : IconOverlayBehavior.HasFlag(OverlayBehavior.Flyouts) ? zIndex : 1);
        _leftWindowCommandsPresenter?.SetValue(Panel.ZIndexProperty, flyout is { IsModal: true, IsOpen: true } ? 0 : 1);
        _rightWindowCommandsPresenter?.SetValue(Panel.ZIndexProperty, flyout is { IsModal: true, IsOpen: true } ? 0 : 1);
        _windowButtonsPresenter?.SetValue(Panel.ZIndexProperty, flyout is { IsModal: true, IsOpen: true } ? 0 : WindowButtonsOverlayBehavior.HasFlag(OverlayBehavior.Flyouts) ? zIndex : 1);

        this.HandleWindowCommandsForFlyouts(visibleFlyouts);

        if (_flyoutModal != null)
        {
            _flyoutModal.Visibility = visibleFlyouts.Any(x => x.IsModal) ? Visibility.Visible : Visibility.Hidden;
        }

        RaiseEvent(new FlyoutStatusChangedRoutedEventArgs(FlyoutsStatusChangedEvent, this) { ChangedFlyout = flyout });
    }

    #endregion

}