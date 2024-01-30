using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using ControlzEx;
using ControlzEx.Theming;
using Quan.ControlLibrary.AttachedProperties;
using Quan.ControlLibrary.Automations;
using Quan.ControlLibrary.Helpers;
using Quan.ControlLibrary.Helpers.Boxes;

namespace Quan.ControlLibrary.Controls;

[StyleTypedProperty(Property = nameof(ItemContainerStyle), StyleTargetType = typeof(WindowCommands))]
public class WindowCommands : ToolBar
{
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
            typeof(WindowCommands),
            new PropertyMetadata(ThemeManager.BaseColorLight, OnThemePropertyChanged));

    private static void OnThemePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue && e.NewValue is string baseColor)
        {
            var windowCommands = (WindowCommands)d;

            switch (baseColor)
            {
                case ThemeManager.BaseColorLightConst:
                    {
                        if (windowCommands.LightTemplate != null)
                        {
                            windowCommands.SetValue(TemplateProperty, windowCommands.LightTemplate);
                        }

                        break;
                    }
                case ThemeManager.BaseColorDarkConst:
                    {
                        if (windowCommands.DarkTemplate != null)
                        {
                            windowCommands.SetValue(TemplateProperty, windowCommands.DarkTemplate);
                        }

                        break;
                    }
            }
        }
    }

    #endregion

    #region LightTemplate

    public ControlTemplate LightTemplate
    {
        get => (ControlTemplate)GetValue(LightTemplateProperty);
        set => SetValue(LightTemplateProperty, value);
    }

    public static readonly DependencyProperty LightTemplateProperty =
        DependencyProperty.Register(
            nameof(LightTemplate),
            typeof(ControlTemplate),
            typeof(WindowCommands),
            new PropertyMetadata(null));

    #endregion

    #region DarkTemplate

    public ControlTemplate DarkTemplate
    {
        get => (ControlTemplate)GetValue(DarkTemplateProperty);
        set => SetValue(DarkTemplateProperty, value);
    }

    public static readonly DependencyProperty DarkTemplateProperty =
        DependencyProperty.Register(
            nameof(DarkTemplate),
            typeof(ControlTemplate),
            typeof(WindowCommands),
            new PropertyMetadata(null));

    #endregion

    #region ShowSeparators

    public bool ShowSeparators
    {
        get => (bool)GetValue(ShowSeparatorsProperty);
        set => SetValue(ShowSeparatorsProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowSeparatorsProperty =
        DependencyProperty.Register(
            nameof(ShowSeparators),
            typeof(bool),
            typeof(WindowCommands),
            new FrameworkPropertyMetadata(
                BooleanBoxes.TrueBox,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                OnShowSeparatorsPropertyChanged));

    private static void OnShowSeparatorsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue)
        {
            ((WindowCommands)d).ResetSeparators();
        }
    }

    #endregion

    #region ShowLastSeparator

    public bool ShowLastSeparator
    {
        get => (bool)GetValue(ShowLastSeparatorProperty);
        set => SetValue(ShowLastSeparatorProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty ShowLastSeparatorProperty =
        DependencyProperty.Register(
            nameof(ShowLastSeparator),
            typeof(bool),
            typeof(WindowCommands),
            new FrameworkPropertyMetadata(
                BooleanBoxes.TrueBox,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender,
                OnShowLastSeparatorPropertyChanged));

    private static void OnShowLastSeparatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue != e.OldValue)
        {
            ((WindowCommands)d).ResetSeparators(false);
        }
    }

    #endregion

    #region SeparatorHeight

    [TypeConverter(typeof(LengthConverter))]
    public double SeparatorHeight
    {
        get => (double)GetValue(SeparatorHeightProperty);
        set => SetValue(SeparatorHeightProperty, value);
    }

    public static readonly DependencyProperty SeparatorHeightProperty =
        DependencyProperty.Register(
            nameof(SeparatorHeight),
            typeof(double),
            typeof(WindowCommands),
            new FrameworkPropertyMetadata(
                15d,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region ParentWindow

    public QuanWindow ParentWindow
    {
        get => (QuanWindow)GetValue(ParentWindowProperty);
        protected set => SetValue(ParentWindowPropertyKey, value);
    }

    internal static readonly DependencyPropertyKey ParentWindowPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(ParentWindow),
            typeof(QuanWindow),
            typeof(WindowCommands),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ParentWindowProperty = ParentWindowPropertyKey.DependencyProperty;

    #endregion

    #region Constructors

    static WindowCommands()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommands), new FrameworkPropertyMetadata(typeof(WindowCommands)));
    }

    public WindowCommands()
    {
        Loaded += WindowCommandsLoaded;
    }

    #endregion

    #region Overrides

    protected override DependencyObject GetContainerForItemOverride()
    {
        return new WindowCommandsItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is WindowCommandsItem;
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (!(element is WindowCommandsItem windowCommandsItem))
        {
            return;
        }

        var frameworkElement = item as FrameworkElement;
        if (item is not FrameworkElement)
        {
            windowCommandsItem.ApplyTemplate();
            frameworkElement = windowCommandsItem.ContentTemplate?.LoadContent() as FrameworkElement;
        }

        frameworkElement?.SetBinding(ControlsHelper.ContentCharacterCasingProperty,
                                     new Binding { Source = this, Path = new PropertyPath(ControlsHelper.ContentCharacterCasingProperty) });

        AttachVisibilityHandler(windowCommandsItem, item as UIElement);
        ResetSeparators();
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
        base.ClearContainerForItemOverride(element, item);

        if (item is FrameworkElement frameworkElement)
        {
            BindingOperations.ClearBinding(frameworkElement, ControlsHelper.ContentCharacterCasingProperty);
        }

        DetachVisibilityHandler(element as WindowCommandsItem);
        ResetSeparators(false);
    }

    protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);

        ResetSeparators();
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new WindowCommandsAutomationPeer(this);
    }

    #endregion

    #region Methods

    private void AttachVisibilityHandler(WindowCommandsItem container, UIElement item)
    {
        if (container == null)
        {
            return;
        }

        if (item is null)
        {
            // if item is not a UIElement then maybe the ItemsSource binds to a collection of objects
            // and an ItemTemplate is set, so let's try to solve this
            container.ApplyTemplate();
            if (!(container.ContentTemplate?.LoadContent() is UIElement))
            {
                // no UIElement was found, so don't show this container
                container.Visibility = Visibility.Collapsed;
            }

            return;
        }

        container.Visibility = item.Visibility;
        var isVisibilityNotifier = new PropertyChangeNotifier(item, VisibilityProperty);
        isVisibilityNotifier.ValueChanged += VisibilityPropertyChanged;
        container.VisibilityPropertyChangeNotifier = isVisibilityNotifier;
    }

    private void DetachVisibilityHandler(WindowCommandsItem container)
    {
        if (container != null)
        {
            container.VisibilityPropertyChangeNotifier = null;
        }
    }

    private void VisibilityPropertyChanged(object sender, EventArgs e)
    {
        if (sender is UIElement item)
        {
            var container = GetWindowCommandsItem(item);
            if (container != null)
            {
                container.Visibility = item.Visibility;
                ResetSeparators();
            }
        }
    }

    private void ResetSeparators(bool reset = true)
    {
        if (Items.Count == 0)
        {
            return;
        }

        var windowCommandsItems = GetWindowCommandsItems().ToList();

        if (reset)
        {
            foreach (var windowCommandsItem in windowCommandsItems)
            {
                windowCommandsItem.IsSeparatorVisible = ShowSeparators;
            }
        }

        var lastContainer = windowCommandsItems.LastOrDefault(i => i.IsVisible);
        if (lastContainer != null)
        {
            lastContainer.IsSeparatorVisible = ShowSeparators && ShowLastSeparator;
        }
    }

    private WindowCommandsItem GetWindowCommandsItem(object item)
    {
        if (item is WindowCommandsItem windowCommandsItem)
        {
            return windowCommandsItem;
        }

        return (WindowCommandsItem)ItemContainerGenerator.ContainerFromItem(item);
    }

    private IEnumerable<WindowCommandsItem> GetWindowCommandsItems()
    {
        foreach (var item in Items)
        {
            var windowCommandsItem = GetWindowCommandsItem(item);
            if (windowCommandsItem != null)
            {
                yield return windowCommandsItem;
            }
        }
    }

    private void WindowCommandsLoaded(object sender, RoutedEventArgs e)
    {
        Loaded -= WindowCommandsLoaded;

        var contentPresenter = this.FindVisualParent<ContentPresenter>();
        if (contentPresenter != null)
        {
            SetCurrentValue(DockPanel.DockProperty, contentPresenter.GetValue(DockPanel.DockProperty));
        }

        if (null == ParentWindow)
        {
            var window = this.FindVisualParent<Window>();
            SetValue(ParentWindowPropertyKey, window);
        }
    }
    #endregion
}