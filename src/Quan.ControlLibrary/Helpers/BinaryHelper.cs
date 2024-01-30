using System.Diagnostics.CodeAnalysis;

namespace Quan.ControlLibrary.Helpers;

internal static class BinaryHelper
{
    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public static int LOWORD(int i)
    {
        return (short)(i & 0xFFFF);
    }
}