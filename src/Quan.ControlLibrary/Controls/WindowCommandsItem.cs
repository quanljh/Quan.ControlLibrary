using System.Windows;
using System.Windows.Controls;
using ControlzEx;
using Quan.ControlLibrary.Helpers.Boxes;

namespace Quan.ControlLibrary.Controls;

[TemplatePart(Name = PART_ContentPresenter, Type = typeof(UIElement))]
[TemplatePart(Name = PART_Separator, Type = typeof(UIElement))]
public class WindowCommandsItem : ContentControl
{
    private const string PART_ContentPresenter = "PART_ContentPresenter";
    private const string PART_Separator = "PART_Separator";

    internal PropertyChangeNotifier VisibilityPropertyChangeNotifier { get; set; }

    #region IsSeparatorVisible

    public bool IsSeparatorVisible
    {
        get => (bool)GetValue(IsSeparatorVisibleProperty);
        set => SetValue(IsSeparatorVisibleProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty IsSeparatorVisibleProperty =
        DependencyProperty.Register(
            nameof(IsSeparatorVisible),
            typeof(bool),
            typeof(WindowCommandsItem),
            new FrameworkPropertyMetadata(
                BooleanBoxes.TrueBox,
                FrameworkPropertyMetadataOptions.Inherits
                | FrameworkPropertyMetadataOptions.AffectsArrange
                | FrameworkPropertyMetadataOptions.AffectsMeasure
                | FrameworkPropertyMetadataOptions.AffectsRender));

    #endregion

    #region ParentWindowCommands

    public WindowCommands ParentWindowCommands
    {
        get => (WindowCommands)GetValue(ParentWindowCommandsProperty);
        protected set => SetValue(ParentWindowCommandsPropertyKey, value);
    }

    private static readonly DependencyPropertyKey ParentWindowCommandsPropertyKey =
        DependencyProperty.RegisterReadOnly(
            nameof(ParentWindowCommands),
            typeof(WindowCommands),
            typeof(WindowCommandsItem),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ParentWindowCommandsProperty = ParentWindowCommandsPropertyKey.DependencyProperty;

    #endregion

    #region Constructors

    static WindowCommandsItem()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCommandsItem), new FrameworkPropertyMetadata(typeof(WindowCommandsItem)));
    }

    #endregion

    #region Overrides

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var windowCommands = ItemsControl.ItemsControlFromItemContainer(this) as WindowCommands;
        SetValue(ParentWindowCommandsPropertyKey, windowCommands);
    }

    #endregion
}