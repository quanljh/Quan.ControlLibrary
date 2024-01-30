using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using Quan.ControlLibrary.Helpers.Boxes;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

/// <summary>
/// Represents the base class for an icon UI element.
/// </summary>
public abstract class IconElement : Control
{
    #region Private Members

    private bool _isForegroundPropertyDefaultOrInherited = true;

    #endregion

    #region InheritsForegroundFromVisualParent

    /// <summary>
    /// Gets whether that this element inherits the <see cref="Control.Foreground"/> form the <see cref="Visual.VisualParent"/>.
    /// </summary>
    public bool InheritsForegroundFromVisualParent
    {
        get => (bool)GetValue(InheritsForegroundFromVisualParentProperty);
        protected set => SetValue(InheritsForegroundFromVisualParentPropertyKey, BooleanBoxes.Box(value));
    }

    internal static readonly DependencyPropertyKey InheritsForegroundFromVisualParentPropertyKey
        = DependencyProperty.RegisterReadOnly(nameof(InheritsForegroundFromVisualParent),
            typeof(bool),
            typeof(IconElement),
            new PropertyMetadata(BooleanBoxes.FalseBox, (sender, e) => ((IconElement)sender).OnInheritsForegroundFromVisualParentPropertyChanged(e)));

    public static readonly DependencyProperty InheritsForegroundFromVisualParentProperty = InheritsForegroundFromVisualParentPropertyKey.DependencyProperty;

    #endregion

    #region VisualParentForeground

    protected Brush VisualParentForeground
    {
        get => (Brush)GetValue(VisualParentForegroundProperty);
        set => SetValue(VisualParentForegroundProperty, value);
    }

    private static readonly DependencyProperty VisualParentForegroundProperty =
        DependencyProperty.Register(
            nameof(VisualParentForeground),
            typeof(Brush),
            typeof(IconElement),
            new PropertyMetadata(default(Brush), (sender, e) => ((IconElement)sender).OnVisualParentForegroundPropertyChanged(e)));



    protected virtual void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
    }

    #endregion

    #region Constructors

    static IconElement()
    {
        ForegroundProperty.OverrideMetadata(
            typeof(IconElement),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits,
                (sender, e) => ((IconElement)sender).OnForegroundPropertyChanged(e)));
    }

    #endregion

    #region Overrides

    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);
        UpdateInheritsForegroundFromVisualParent();
    }

    #endregion

    #region Methods

    protected void OnForegroundPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        var baseValueSource = DependencyPropertyHelper.GetValueSource(this, e.Property).BaseValueSource;
        _isForegroundPropertyDefaultOrInherited = baseValueSource <= BaseValueSource.Inherited;
        UpdateInheritsForegroundFromVisualParent();
    }

    private void UpdateInheritsForegroundFromVisualParent()
    {
        InheritsForegroundFromVisualParent
            = _isForegroundPropertyDefaultOrInherited
              && Parent != null
              && VisualParent != null
              && Parent != VisualParent;
    }


    protected virtual void OnInheritsForegroundFromVisualParentPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue != e.NewValue)
        {
            if (e.NewValue is true)
            {
                SetBinding(VisualParentForegroundProperty,
                    new Binding
                    {
                        Path = new PropertyPath(TextElement.ForegroundProperty),
                        Source = VisualParent
                    });
            }
            else
            {
                ClearValue(VisualParentForegroundProperty);
            }
        }
    }

    #endregion
}