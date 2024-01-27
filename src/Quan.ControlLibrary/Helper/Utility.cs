using System.Diagnostics.CodeAnalysis;

namespace Quan.ControlLibrary;

internal static partial class Utility
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int LOWORD(int i)
    {
        return (short)(i & 0xFFFF);
    }
}