namespace Quan.ControlLibrary.Helpers;

public static class ParseHelper
{
    public static int ToInt(this string value)
    {
        return int.TryParse(value, out var number) ? number : 0;
    }

    public static int? ToIntOrNull(this string value)
    {
        if (int.TryParse(value, out var number))
        {
            return number;
        }

        return null;
    }
}