using System;
using System.Windows;

namespace Quan.ControlLibrary
{
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

        /// <summary>
        /// Get skin by skin type
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static ResourceDictionary GetSkin(SkinType skin) => new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/Quan.ControlLibrary;component/Themes/Skin{skin.ToString()}.xaml")
        };
    }
}
