using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Quan.ControlLibrary.Constants;
using Quan.ControlLibrary.Controls;
using Quan.ControlLibrary.Helpers.Boxes;

namespace Quan.ControlLibrary.AttachedProperties;

public static class ControlsHelper
{
    #region CornerRadius

    /// <summary>
    /// DependencyProperty for <see cref="CornerRadius" /> property.
    /// </summary>
    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.RegisterAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(ControlsHelper),
            new FrameworkPropertyMetadata(new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

    /// <summary> 
    /// The CornerRadius property allows users to control the roundness of the button corners independently by 
    /// setting a radius value for each corner. Radius values that are too large are scaled so that they
    /// smoothly blend from corner to corner. (Can be used e.g. at QuanButton style)
    /// Description taken from original Microsoft description :-D
    /// </summary>
    [Category(AppName.QuanApp)]
    public static CornerRadius GetCornerRadius(UIElement element)
    {
        return (CornerRadius)element.GetValue(CornerRadiusProperty);
    }

    [Category(AppName.QuanApp)]
    public static void SetCornerRadius(UIElement element, CornerRadius value)
    {
        element.SetValue(CornerRadiusProperty, value);
    }

    #endregion

    #region ContentCharacterCasing

    /// <summary>
    /// The DependencyProperty for the CharacterCasing property.
    /// The Controls content will be converted to upper or lower case if it is a string.
    /// </summary>
    public static readonly DependencyProperty ContentCharacterCasingProperty =
        DependencyProperty.RegisterAttached(
            "ContentCharacterCasing",
            typeof(CharacterCasing),
            typeof(ControlsHelper),
            new FrameworkPropertyMetadata(
                CharacterCasing.Normal,
                FrameworkPropertyMetadataOptions.AffectsMeasure),
            value => CharacterCasing.Normal <= (CharacterCasing)value && (CharacterCasing)value <= CharacterCasing.Upper);

    /// <summary>
    /// Gets the character casing of the control
    /// </summary>
    [Category(AppName.QuanApp)]
    [AttachedPropertyBrowsableForType(typeof(ContentControl))]
    [AttachedPropertyBrowsableForType(typeof(WindowCommands))]
    public static CharacterCasing GetContentCharacterCasing(UIElement element)
    {
        return (CharacterCasing)element.GetValue(ContentCharacterCasingProperty);
    }

    #endregion

    #region RecognizesAccessKey

    public static readonly DependencyProperty RecognizesAccessKeyProperty
        = DependencyProperty.RegisterAttached(
            "RecognizesAccessKey",
            typeof(bool),
            typeof(ControlsHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));


    /// <summary> 
    /// Gets the value if the inner ContentPresenter use AccessText in its style.
    /// </summary> 
    [Category(AppName.QuanApp)]
    [AttachedPropertyBrowsableForType(typeof(ContentControl))]
    public static bool GetRecognizesAccessKey(UIElement element)
    {
        return (bool)element.GetValue(RecognizesAccessKeyProperty);
    }

    /// <summary> 
    /// Sets the value if the inner ContentPresenter should use AccessText in its style.
    /// </summary> 
    [Category(AppName.QuanApp)]
    [AttachedPropertyBrowsableForType(typeof(ContentControl))]
    public static void SetRecognizesAccessKey(UIElement element, bool value)
    {
        element.SetValue(RecognizesAccessKeyProperty, BooleanBoxes.Box(value));
    }

    #endregion
}