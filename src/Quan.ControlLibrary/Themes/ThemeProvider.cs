using System.Collections;
using ControlzEx.Theming;

namespace Quan.ControlLibrary.Themes;

public class ThemeProvider : LibraryThemeProvider
{
    public static readonly ThemeProvider DefaultInstance = new();

    /// <inheritdoc cref="LibraryThemeProvider" />
    public ThemeProvider() : base(true)
    {
    }

    protected override bool IsPotentialThemeResourceDictionary(DictionaryEntry dictionaryEntry)
    {
        return dictionaryEntry.Key is string stringKey && stringKey.StartsWith("themes/Styles/Colors", StringComparison.OrdinalIgnoreCase);
    }

    public override void FillColorSchemeValues(Dictionary<string, string> values, RuntimeThemeColorValues colorValues)
    {
        values.Add("Quan.Colors.AccentBase", colorValues.AccentBaseColor.ToString());
        values.Add("Quan.Colors.Accent", colorValues.AccentColor80.ToString());
        values.Add("Quan.Colors.Accent2", colorValues.AccentColor60.ToString());
        values.Add("Quan.Colors.Accent3", colorValues.AccentColor40.ToString());
        values.Add("Quan.Colors.Accent4", colorValues.AccentColor20.ToString());

        values.Add("Quan.Colors.Highlight", colorValues.HighlightColor.ToString());
        values.Add("Quan.Colors.ForegroundHighlight", colorValues.IdealForegroundColor.ToString());
    }
}