using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Quan.ControlLibrary
{

    public static class VisualTreeExtensions
    {
        internal static DependencyObject FindVisualTreeRoot(this DependencyObject d)
        {
            var current = d;
            var result = d;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                    break;
                // If we're in Logical Land then we must walk up the logical tree
                // until we find a Visual/Visual3D to get us back to Visual Land.
                current = LogicalTreeHelper.GetParent(current);
            }

            return result;
        }

        /// <summary>
        /// Find the first parent element within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="T">The type of parent element</typeparam>
        /// <param name="obj">The source element</param>
        /// <returns></returns>
        public static T FindVisualParent<T>(this DependencyObject obj) where T : class
        {
            var parent = VisualTreeHelper.GetParent(obj.FindVisualTreeRoot());
            while (parent != null)
            {
                if (parent is T element)
                    return element;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Find the first parent element within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="T">The type of parent element</typeparam>
        /// <param name="obj">The source element</param>
        /// <param name="name">The name of parent element</param>
        /// <returns></returns>
        public static T FindVisualParentByName<T>(this DependencyObject obj, string name = null) where T : FrameworkElement
        {
            var parent = VisualTreeHelper.GetParent(obj.FindVisualTreeRoot());
            while (parent != null)
            {
                if (parent is T element && (element.Name == name || string.IsNullOrEmpty(name)))
                {
                    return element;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        /// <summary>
        /// Find the first parent element from source element's visual tree by type
        /// </summary>
        /// <param name="d">The source control</param>
        /// <param name="itemSearchType">The parent type</param>
        /// <param name="itemsControl">The items Control</param>
        /// <param name="itemContainerSearchType">The ItemContainer type of the ItemsControl</param>
        /// <returns>Return Null if we not found</returns>
        public static DependencyObject FindVisualParent(this DependencyObject d, Type itemSearchType,
             ItemsControl itemsControl, Type itemContainerSearchType)
        {
            if (itemsControl == null) throw new ArgumentNullException(nameof(itemsControl));
            if (itemContainerSearchType == null) throw new ArgumentNullException(nameof(itemContainerSearchType));

            var visualTreeRoot = d.FindVisualTreeRoot();
            var currentVisual = VisualTreeHelper.GetParent(visualTreeRoot);

            while (currentVisual != null && itemSearchType != null)
            {
                var currentVisualType = currentVisual.GetType();
                if (currentVisualType == itemSearchType || currentVisualType.IsSubclassOf(itemSearchType))
                {
                    if (currentVisual is TreeViewItem || itemsControl.ItemContainerGenerator.IndexFromContainer(currentVisual) != -1)
                        return currentVisual;
                }

                // current visual type is parent of search type
                if (itemContainerSearchType.IsAssignableFrom(currentVisualType))
                    // Ok, we found an ItemsControl (maybe an empty) and return
                    return null;

                currentVisual = VisualTreeHelper.GetParent(currentVisual);
            }

            return null;
        }

        /// <summary>
        /// Find the visual ancestor by type and go through the visual tree until the given itemsControl will be found
        /// </summary>
        public static DependencyObject FindVisualParent(this DependencyObject d, Type itemSearchType, ItemsControl itemsControl)
        {
            if (itemsControl == null) throw new ArgumentNullException(nameof(itemsControl));

            var visualTreeRoot = d.FindVisualTreeRoot();
            var currentVisual = VisualTreeHelper.GetParent(visualTreeRoot);
            DependencyObject lastFoundItemByType = null;

            while (currentVisual != null && itemSearchType != null)
            {
                if (currentVisual == itemsControl)
                {
                    return lastFoundItemByType;
                }
                var currentVisualType = currentVisual.GetType();
                if ((currentVisualType == itemSearchType || currentVisualType.IsSubclassOf(itemSearchType))
                    && (itemsControl.ItemContainerGenerator.IndexFromContainer(currentVisual) != -1))
                {
                    lastFoundItemByType = currentVisual;
                }
                currentVisual = VisualTreeHelper.GetParent(currentVisual);
            }

            return lastFoundItemByType;
        }


        /// <summary>
        /// Find the first child element within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="T">The child element's type</typeparam>
        /// <param name="obj">The source element</param>
        /// <returns>The first Child element</returns>
        public static T FindVisualChild<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child is T item)
                    return item;

                T childOfChild = FindVisualChild<T>(child);

                if (childOfChild != null) return childOfChild;
            }
            return null;
        }


        /// <summary>
        /// Find all child elements within specific type from source element's visual tree
        /// </summary>
        /// <typeparam name="T">The child element's type</typeparam>
        /// <param name="obj">The source element</param>
        /// <returns>The Child elements</returns>
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject obj) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);

                // If child matches the type...
                if (child is T matchElement)
                    yield return matchElement;

                // match type from child's children
                foreach (var matchItem in FindVisualChildren<T>(child))
                {
                    yield return matchItem;
                }
            }
        }
    }
}
