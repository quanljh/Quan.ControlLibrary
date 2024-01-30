using System.Windows;

namespace Quan.ControlLibrary.Helpers;

public class ResourceHelper
{
    /// <summary>
    /// Get system string resource
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetString(string key) => Application.Current.TryFindResource(key) as string;

    /// <summary>
    /// Get resource
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T GetResource<T>(string key)
    {
        if (Application.Current.TryFindResource(key) is T resource)
        {
            return resource;
        }

        return default;
    }
}