using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ControlzEx;
using Quan.ControlLibrary.Helpers.Boxes;

// ReSharper disable once CheckNamespace
namespace Quan.ControlLibrary.Controls;

public class ContentControlEx : ContentControl
{
    #region ContentCharacterCasing

    public CharacterCasing ContentCharacterCasing
    {
        get => (CharacterCasing)GetValue(ContentCharacterCasingProperty);
        set => SetValue(ContentCharacterCasingProperty, value);
    }

    public static readonly DependencyProperty ContentCharacterCasingProperty =
        DependencyProperty.Register(
            nameof(ContentCharacterCasing),
            typeof(CharacterCasing),
            typeof(ContentControlEx),
            new FrameworkPropertyMetadata(
                CharacterCasing.Normal,
                FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper);

    #endregion

    #region RecognizesAccessKey

    public bool RecognizesAccessKey
    {
        get => (bool)GetValue(RecognizesAccessKeyProperty);
        set => SetValue(RecognizesAccessKeyProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty RecognizesAccessKeyProperty
        = DependencyProperty.Register(
            nameof(RecognizesAccessKey),
            typeof(bool),
            typeof(ContentControlEx),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    static ContentControlEx()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControlEx), new FrameworkPropertyMetadata(typeof(ContentControlEx)));
    }

    protected override void OnContentChanged(object oldContent, object newContent)
    {
        if (oldContent is IInputElement and DependencyObject oldInputElement)
        {
            BindingOperations.ClearBinding(oldInputElement, WindowChrome.IsHitTestVisibleInChromeProperty);
        }

        base.OnContentChanged(oldContent, newContent);

        if (newContent is IInputElement and DependencyObject newInputElement)
        {
            BindingOperations.SetBinding(
                newInputElement,
                WindowChrome.IsHitTestVisibleInChromeProperty,
                new Binding
                {
                    Path = new PropertyPath(WindowChrome.IsHitTestVisibleInChromeProperty),
                    Source = this
                });
        }
    }
}