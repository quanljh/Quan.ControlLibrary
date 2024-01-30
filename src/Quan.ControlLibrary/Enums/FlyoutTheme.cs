using ControlzEx.Theming;
using Quan.ControlLibrary.Controls;

namespace Quan.ControlLibrary.Enums;

public enum FlyoutTheme
{
    /// <summary>
    /// Adapts the <see cref="Flyout"/> theme to the theme of the host window or application.
    /// </summary>
    Adapt,

    /// <summary>
    /// Adapts the <see cref="Flyout"/> theme to the theme of the host window or application, but inverted.
    /// </summary>
    /// <remarks>
    /// This theme can only be applied if the host window's theme abides the "Dark" and "Light" affix convention.
    /// (see <see cref="ThemeManager.GetInverseTheme"/> for more infos.
    /// </remarks>
    Inverse,

    /// <summary>
    /// Use the dark theme for the <see cref="Flyout"/>. This is the default theme.
    /// </summary>
    Dark,

    /// <summary>
    /// Use the light theme for the <see cref="Flyout"/>.
    /// </summary>
    Light
}