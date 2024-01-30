using Quan.ControlLibrary.Controls;

namespace Quan.ControlLibrary.Helpers.Boxes;

/// <summary>
/// Helps boxing <see cref="QuanTextBox.DisplayType"/> values to prevent boxing and unboxing.
/// </summary>
public static class QuanTextBoxDisplayTypeBoxes

{
    /// <summary>
    /// Gets a boxed representation for QuanTextBox DisplayType's "Normal" value.
    /// </summary>
    public static readonly object NormalBox = QuanTextBox.DisplayType.Normal;

    /// <summary>
    /// Gets a boxed representation for QuanTextBox DisplayType's "Flat" value.
    /// </summary>
    public static readonly object FloatingBox = QuanTextBox.DisplayType.Floating;


    /// <summary>
    /// Returns a boxed representation for the specified <see cref="QuanTextBox.DisplayType"/> value.
    /// </summary>
    /// <param name="value">The value to box.</param>
    /// <returns></returns>
    public static object Box(QuanTextBox.DisplayType value)
    {
        return value == QuanTextBox.DisplayType.Normal ? NormalBox : FloatingBox;
    }
}