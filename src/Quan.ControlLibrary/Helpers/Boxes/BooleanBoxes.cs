﻿namespace Quan.ControlLibrary.Helpers.Boxes;

/// <summary>
/// Helps boxing Boolean values to prevent boxing and unboxing.
/// </summary>
public static class BooleanBoxes
{
    /// <summary>
    /// Gets a boxed representation for Boolean's "true" value.
    /// </summary>
    public static readonly object TrueBox = true;

    /// <summary>
    /// Gets a boxed representation for Boolean's "false" value.
    /// </summary>
    public static readonly object FalseBox = false;

    /// <summary>
    /// Returns a boxed representation for the specified Boolean value.
    /// </summary>
    /// <param name="value">The value to box.</param>
    /// <returns></returns>
    public static object Box(bool value) => value ? TrueBox : FalseBox;
}