﻿using System.Windows;

namespace Quan.ControlLibrary.AttachedProperties;

public static class TransitionHelper
{
    /// <summary>
    /// Allows transitions to be disabled where supported.  Note this is an inheritable property.
    /// </summary>
    public static readonly DependencyProperty DisableTransitionsProperty =
        DependencyProperty.RegisterAttached(
        "DisableTransitions",
        typeof(bool),
        typeof(TransitionHelper),
        new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Allows transitions to be disabled where supported.  Note this is an inheritable property.
    /// </summary>
    public static void SetDisableTransitions(DependencyObject element, bool value)
    {
        element.SetValue(DisableTransitionsProperty, value);
    }

    /// <summary>
    /// Allows transitions to be disabled where supported.  Note this is an inheritable property.
    /// </summary>
    public static bool GetDisableTransitions(DependencyObject element)
    {
        return (bool)element.GetValue(DisableTransitionsProperty);
    }
}