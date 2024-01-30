using System.Collections;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ControlzEx.Theming;
using Quan.ControlLibrary.AttachedProperties;
using Quan.ControlLibrary.Automations;
using Quan.ControlLibrary.Enums;
using Quan.ControlLibrary.Helpers;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

/// <summary>
/// The base class for dialogs.
///
/// You probably don't want to use this class, if you want to add arbitrary content to your dialog,
/// use the <see cref="CustomDialog"/> class.
/// </summary>
[TemplatePart(Name = PART_Top, Type = typeof(ContentPresenter))]
[TemplatePart(Name = PART_Content, Type = typeof(ContentPresenter))]
[TemplatePart(Name = PART_Bottom, Type = typeof(ContentPresenter))]
public abstract class BaseQuanDialog : ContentControl
{
    private const string PART_Top = "PART_Top";
    private const string PART_Content = "PART_Content";
    private const string PART_Bottom = "PART_Bottom";

    public QuanDialogSettings DialogSettings { get; private set; } = null!;

    internal SizeChangedEventHandler SizeChangedHandler { get; set; }

    #region DependencyProperties

    #region ColorScheme

    public QuanDialogColorScheme ColorScheme
    {
        get => (QuanDialogColorScheme)GetValue(ColorSchemeProperty);
        set => SetValue(ColorSchemeProperty, value);
    }

    /// <summary>Identifies the <see cref="ColorScheme"/> dependency property.</summary>
    public static readonly DependencyProperty ColorSchemeProperty =
        DependencyProperty.Register(
            nameof(ColorScheme),
            typeof(QuanDialogColorScheme),
            typeof(BaseQuanDialog),
            new PropertyMetadata(QuanDialogColorScheme.Theme));

    #endregion

    #region DialogContentMargin

    /// <summary>
    /// Gets or sets the left and right margin for the dialog content.
    /// </summary>
    public GridLength DialogContentMargin
    {
        get => (GridLength)GetValue(DialogContentMarginProperty);
        set => SetValue(DialogContentMarginProperty, value);
    }

    public static readonly DependencyProperty DialogContentMarginProperty =
        DependencyProperty.Register(
            nameof(DialogContentMargin),
            typeof(GridLength),
            typeof(BaseQuanDialog),
            new PropertyMetadata(new GridLength(25, GridUnitType.Star)));

    #endregion

    #region DialogContentWidth

    /// <summary>
    /// Gets or sets the width for the dialog content.
    /// </summary>
    public GridLength DialogContentWidth
    {
        get => (GridLength)GetValue(DialogContentWidthProperty);
        set => SetValue(DialogContentWidthProperty, value);
    }

    public static readonly DependencyProperty DialogContentWidthProperty =
        DependencyProperty.Register(
            nameof(DialogContentWidth),
            typeof(GridLength),
            typeof(BaseQuanDialog),
            new PropertyMetadata(new GridLength(50, GridUnitType.Star)));

    #endregion

    #region Title

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    public object Title
    {
        get => (object)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(object),
            typeof(BaseQuanDialog),
            new PropertyMetadata(default(object)));

    #endregion

    #region DialogTop

    /// <summary>
    /// Gets or sets the content above the dialog.
    /// </summary>
    public object DialogTop
    {
        get => GetValue(DialogTopProperty);
        set => SetValue(DialogTopProperty, value);
    }

    /// <summary>Identifies the <see cref="DialogTop"/> dependency property.</summary>
    public static readonly DependencyProperty DialogTopProperty
        = DependencyProperty.Register(nameof(DialogTop),
            typeof(object),
            typeof(BaseQuanDialog),
            new PropertyMetadata(null, UpdateLogicalChild));

    #endregion

    #region DialogBottom

    /// <summary>
    /// Gets or sets the content below the dialog.
    /// </summary>
    public object DialogBottom
    {
        get => GetValue(DialogBottomProperty);
        set => SetValue(DialogBottomProperty, value);
    }

    /// <summary>Identifies the <see cref="DialogBottom"/> dependency property.</summary>
    public static readonly DependencyProperty DialogBottomProperty
        = DependencyProperty.Register(nameof(DialogBottom),
            typeof(object),
            typeof(BaseQuanDialog),
            new PropertyMetadata(null, UpdateLogicalChild));

    #endregion

    #region DialogTitleFontSize

    /// <summary>
    /// Gets or sets the font size of the dialog title.
    /// </summary>
    public double DialogTitleFontSize
    {
        get => (double)GetValue(DialogTitleFontSizeProperty);
        set => SetValue(DialogTitleFontSizeProperty, value);
    }


    /// <summary>Identifies the <see cref="DialogTitleFontSize"/> dependency property.</summary>
    public static readonly DependencyProperty DialogTitleFontSizeProperty
        = DependencyProperty.Register(nameof(DialogTitleFontSize),
            typeof(double),
            typeof(BaseQuanDialog),
            new PropertyMetadata(26D));


    #endregion

    #region DialogMessageFontSize

    /// <summary>
    /// Gets or sets the font size of the dialog message text.
    /// </summary>
    public double DialogMessageFontSize
    {
        get => (double)GetValue(DialogMessageFontSizeProperty);
        set => SetValue(DialogMessageFontSizeProperty, value);
    }


    /// <summary>Identifies the <see cref="DialogMessageFontSize"/> dependency property.</summary>
    public static readonly DependencyProperty DialogMessageFontSizeProperty
        = DependencyProperty.Register(nameof(DialogMessageFontSize),
            typeof(double),
            typeof(BaseQuanDialog),
            new PropertyMetadata(15D));


    #endregion

    #region DialogButtonFontSize

    /// <summary>
    /// Gets or sets the font size of any dialog buttons.
    /// </summary>
    public double DialogButtonFontSize
    {
        get => (double)GetValue(DialogButtonFontSizeProperty);
        set => SetValue(DialogButtonFontSizeProperty, value);
    }

    public static readonly DependencyProperty DialogButtonFontSizeProperty
        = DependencyProperty.Register(nameof(DialogButtonFontSize),
            typeof(double),
            typeof(BaseQuanDialog),
            new PropertyMetadata(SystemFonts.MessageFontSize));

    #endregion

    #region Icon

    public object Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly DependencyProperty IconProperty
        = DependencyProperty.Register(nameof(Icon),
            typeof(object),
            typeof(BaseQuanDialog),
            new PropertyMetadata());

    #endregion

    #region IconTemplate

    public DataTemplate IconTemplate
    {
        get => (DataTemplate)GetValue(IconTemplateProperty);
        set => SetValue(IconTemplateProperty, value);
    }

    public static readonly DependencyProperty IconTemplateProperty
        = DependencyProperty.Register(nameof(IconTemplate),
            typeof(DataTemplate),
            typeof(BaseQuanDialog));

    #endregion

    #endregion DependencyProperties

    #region Constructor

    static BaseQuanDialog()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseQuanDialog), new FrameworkPropertyMetadata(typeof(BaseQuanDialog)));
    }

    /// <summary>
    /// Initializes a new <see cref="BaseQuanDialog"/>.
    /// </summary>
    /// <param name="owningWindow">The window that is the parent of the dialog.</param>
    /// <param name="settings">The settings for the message dialog.</param>
    protected BaseQuanDialog(QuanWindow owningWindow, QuanDialogSettings settings)
    {
        Initialize(owningWindow, settings);
    }

    /// <summary>
    /// Initializes a new <see cref="BaseQuanDialog"/>.
    /// </summary>
    protected BaseQuanDialog()
        : this(null, new QuanDialogSettings())
    {
    }

    #endregion Constructor

    #region Overrides
    protected override IEnumerator LogicalChildren
    {
        get
        {
            // cheat, make a list with all logical content and return the enumerator
            var children = new ArrayList();
            if (DialogTop != null)
            {
                children.Add(DialogTop);
            }

            if (Content != null)
            {
                children.Add(Content);
            }

            if (DialogBottom != null)
            {
                children.Add(DialogBottom);
            }

            return children.GetEnumerator();
        }
    }

    #endregion

    #region Methods

    private static void UpdateLogicalChild(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        if (dependencyObject is not BaseQuanDialog dialog)
        {
            return;
        }

        if (e.OldValue is FrameworkElement oldChild)
        {
            dialog.RemoveLogicalChild(oldChild);
        }

        if (e.NewValue is FrameworkElement newChild)
        {
            dialog.AddLogicalChild(newChild);
            newChild.DataContext = dialog.DataContext;
        }
    }


    /// <summary>
    /// With this method it's possible to return your own settings in a custom dialog.
    /// </summary>
    /// <param name="settings">
    /// Settings from the <see cref="QuanWindow.QuanDialogOptions"/> or from constructor.
    /// The default is a new created settings.
    /// </param>
    /// <returns></returns>
    protected virtual QuanDialogSettings ConfigureSettings(QuanDialogSettings settings)
    {
        return settings;
    }

    private void Initialize(QuanWindow owningWindow, QuanDialogSettings settings)
    {
        AccessKeyHelper.SetIsAccessKeyScope(this, true);

        OwningWindow = owningWindow;
        DialogSettings = ConfigureSettings(settings ?? owningWindow?.QuanDialogOptions ?? new QuanDialogSettings());

        if (DialogSettings.CustomResourceDictionary is not null)
        {
            Resources.MergedDictionaries.Add(DialogSettings.CustomResourceDictionary);
        }

        SetCurrentValue(ColorSchemeProperty, DialogSettings.ColorScheme);

        SetCurrentValue(IconProperty, DialogSettings.Icon);
        SetCurrentValue(IconTemplateProperty, DialogSettings.IconTemplate);

        HandleThemeChange();

        DataContextChanged += BaseQuanDialogDataContextChanged;
        Loaded += BaseQuanDialogLoaded;
        Unloaded += BaseQuanDialogUnloaded;
    }

    private void BaseQuanDialogDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        // Add these content presenter to the dialog with AddLogicalChild method.
        // This has the side effect that the DataContext doesn't update, so do this now here.
        if (DialogTop is FrameworkElement elementTop)
        {
            elementTop.DataContext = DataContext;
        }

        if (DialogBottom is FrameworkElement elementBottom)
        {
            elementBottom.DataContext = DataContext;
        }
    }

    private void BaseQuanDialogLoaded(object sender, RoutedEventArgs e)
    {
        ThemeManager.Current.ThemeChanged -= HandleThemeManagerThemeChanged;
        ThemeManager.Current.ThemeChanged += HandleThemeManagerThemeChanged;
        OnLoaded();
    }

    private void BaseQuanDialogUnloaded(object sender, RoutedEventArgs e)
    {
        ThemeManager.Current.ThemeChanged -= HandleThemeManagerThemeChanged;
    }

    private void HandleThemeManagerThemeChanged(object sender, ThemeChangedEventArgs e)
    {
        this.Invoke(HandleThemeChange);
    }

    private static object TryGetResource(Theme theme, string key)
    {
        return theme?.Resources[key];
    }

    internal void HandleThemeChange()
    {
        var theme = DetectTheme(this);

        if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)
            || theme is null)
        {
            return;
        }

        switch (DialogSettings.ColorScheme)
        {
            case QuanDialogColorScheme.Theme:
                ThemeManager.Current.ChangeTheme(this, Resources, theme);
                SetCurrentValue(BackgroundProperty, TryGetResource(theme, "Quan.Brushes.Dialog.Background"));
                SetCurrentValue(ForegroundProperty, TryGetResource(theme, "Quan.Brushes.Dialog.Foreground"));
                break;

            case QuanDialogColorScheme.Inverted:
                theme = ThemeManager.Current.GetInverseTheme(theme);
                if (theme is null)
                {
                    throw new InvalidOperationException("The inverse dialog theme only works if the window theme abides the naming convention. " +
                                                        "See ThemeManager.GetInverseAppTheme for more infos");
                }

                ThemeManager.Current.ChangeTheme(this, Resources, theme);
                SetCurrentValue(BackgroundProperty, TryGetResource(theme, "Quan.Brushes.Dialog.Background"));
                SetCurrentValue(ForegroundProperty, TryGetResource(theme, "Quan.Brushes.Dialog.Foreground"));
                break;

            case QuanDialogColorScheme.Accented:
                ThemeManager.Current.ChangeTheme(this, Resources, theme);
                SetCurrentValue(BackgroundProperty, TryGetResource(theme, "Quan.Brushes.Dialog.Background.Accent"));
                SetCurrentValue(ForegroundProperty, TryGetResource(theme, "Quan.Brushes.Dialog.Foreground.Accent"));
                break;
        }
    }

    /// <summary>
    /// This is called in the loaded event.
    /// </summary>
    protected virtual void OnLoaded()
    {
        // nothing here
    }

    private static Theme DetectTheme(BaseQuanDialog dialog)
    {
        if (dialog is null)
        {
            return null;
        }

        // first look for owner
        var window = dialog.OwningWindow ?? dialog.FindVisualParent<QuanWindow>();
        var theme = window != null ? ThemeManager.Current.DetectTheme(window) : null;
        if (theme != null)
        {
            return theme;
        }

        // second try, look for main window and then for current application
        if (Application.Current != null)
        {
            theme = Application.Current.MainWindow is null
                ? ThemeManager.Current.DetectTheme(Application.Current)
                : ThemeManager.Current.DetectTheme(Application.Current.MainWindow);
            if (theme != null)
            {
                return theme;
            }
        }

        return null;
    }

    private RoutedEventHandler dialogOnLoaded;

    /// <summary>
    /// Waits for the dialog to become ready for interaction.
    /// </summary>
    /// <returns>A task that represents the operation and it's status.</returns>
    public Task WaitForLoadAsync()
    {
        Dispatcher.VerifyAccess();

        if (IsLoaded)
        {
            return Task.CompletedTask;
        }

        var tcs = new TaskCompletionSource<object>();

        if (DialogSettings.AnimateShow != true)
        {
            SetCurrentValue(OpacityProperty, 1.0); // skip the animation
        }

        dialogOnLoaded = (_, _) =>
        {
            Loaded -= dialogOnLoaded;

            Focus();

            tcs.TrySetResult(null!);
        };

        Loaded += dialogOnLoaded;

        return tcs.Task;
    }

    internal void FireOnShown()
    {
        OnShown();
    }

    protected virtual void OnShown()
    {
    }

    internal void FireOnClose()
    {
        OnClose();
    }

    protected virtual void OnClose()
    {
    }

    /// <summary>
    /// Gets the window that owns the current Dialog IF AND ONLY IF the dialog is shown inside of a window.
    /// </summary>
    protected QuanWindow OwningWindow { get; private set; }

    /// <summary>
    /// Waits until this dialog gets unloaded.
    /// </summary>
    /// <returns></returns>
    public Task WaitUntilUnloadedAsync()
    {
        var tcs = new TaskCompletionSource<object>();

        Unloaded += (_, _) => { tcs.TrySetResult(null!); };

        return tcs.Task;
    }

    private EventHandler closingStoryboardOnCompleted;

    public Task WaitForCloseAsync()
    {
        var tcs = new TaskCompletionSource<object>();

        if (DialogSettings.AnimateHide)
        {
            if (TryFindResource("Quan.Storyboard.Dialogs.Close") is not Storyboard closingStoryboard)
            {
                throw new InvalidOperationException("Unable to find the dialog closing storyboard. Did you forget to add BaseQuanDialog.xaml to your merged dictionaries?");
            }

            closingStoryboard = closingStoryboard.Clone();

            closingStoryboardOnCompleted = (_, _) =>
            {
                closingStoryboard.Completed -= closingStoryboardOnCompleted;

                tcs.TrySetResult(null!);
            };

            closingStoryboard.Completed += closingStoryboardOnCompleted;

            closingStoryboard.Begin(this);
        }
        else
        {
            SetCurrentValue(OpacityProperty, 0.0);
            tcs.TrySetResult(null!); //skip the animation
        }

        return tcs.Task;
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new QuanDialogAutomationPeer(this);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e is { Key: Key.System, SystemKey: Key.LeftAlt or Key.RightAlt or Key.F10 })
        {
            if (ReferenceEquals(e.Source, this))
            {
                // Try to look if there is a main menu inside the dialog.
                // If no main menu exists then handle the Alt-Key and F10-Key
                // to prevent focusing the first menu item at the main menu (window).
                var menu = this.FindVisualChildren<Menu>().FirstOrDefault(m => m.IsMainMenu);
                if (menu is null)
                {
                    e.Handled = true;
                }
            }
        }

        base.OnKeyDown(e);
    }
    #endregion
}