using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Quan.ControlLibrary.Helpers;

public static class VisualTreeExtensions
{
    internal static DependencyObject GetVisualTreeRoot(this DependencyObject d)
    {
        var current = d;
        var result = d;

        while (current != null)
        {
            result = current;
            if (current is Visual or Visual3D)
            {
                break;
            }

            // If we're in Logical Land then we must walk up the logical tree
            // until we find a Visual/Visual3D to get us back to Visual Land.
            current = LogicalTreeHelper.GetParent(current);
        }

        return result;
    }

    /// <summary>
    /// Gets all visual parents of a given element on its visual tree
    /// </summary>
    /// <param name="dp">The source element</param>
    /// <returns>All visual parents of <paramref name="dp"/> element</returns>
    public static IEnumerable<DependencyObject> GetVisualParents(this DependencyObject dp)
    {
        var parent = VisualTreeHelper.GetParent(dp);
        while (parent != null)
        {
            yield return parent;
            parent = VisualTreeHelper.GetParent(parent);
        }
    }

    /// <summary>
    /// Find the first parent element within specific type from source element's visual tree
    /// </summary>
    /// <typeparam name="T">The type of parent element</typeparam>
    /// <param name="obj">The source element</param>
    /// <param name="name">The name of parent element</param>
    /// <returns></returns>
    public static T FindVisualParent<T>(this DependencyObject obj, string name = null) where T : FrameworkElement
    {
        var parent = VisualTreeHelper.GetParent(obj.GetVisualTreeRoot());
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

        var visualTreeRoot = d.GetVisualTreeRoot();
        var currentVisual = VisualTreeHelper.GetParent(visualTreeRoot);

        while (currentVisual != null && itemSearchType != null)
        {
            var currentVisualType = currentVisual.GetType();
            if (currentVisualType == itemSearchType || currentVisualType.IsSubclassOf(itemSearchType))
            {
                if (currentVisual is TreeViewItem || itemsControl.ItemContainerGenerator.IndexFromContainer(currentVisual) != -1)
                {
                    return currentVisual;
                }
            }

            // current visual type is parent of search type
            if (itemContainerSearchType.IsAssignableFrom(currentVisualType))
            {
                // Ok, we found an ItemsControl (maybe an empty) and return
                return null;
            }

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

        var visualTreeRoot = d.GetVisualTreeRoot();
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
    /// Finds a Child of a given item in the visual tree. 
    /// </summary>
    /// <param name="parent">A direct parent of the queried item.</param>
    /// <typeparam name="T">The type of the queried item.</typeparam>
    /// <param name="childName">x:Name or Name of child. </param>
    /// <returns>The first parent item that matches the submitted type parameter. 
    /// If not matching item can be found, 
    /// a null parent is being returned.</returns>
    public static T FindVisualChild<T>(this DependencyObject parent, string childName = null) where T : DependencyObject
    {
        // Confirm parent and childName are valid. 
        if (parent is null)
        {
            return null;
        }

        T foundChild = null;

        var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < childrenCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // If the child is not of the request child type child
            if (child is not T currentChild)
            {
                // recursively drill down the tree
                foundChild = FindVisualChild<T>(child, childName);
                // If the child is found, break, so we do not overwrite the found child. 
                if (foundChild is not null)
                {
                    break;
                }
            }
            else if (!string.IsNullOrEmpty(childName))
            {
                // If the child's name is set for search
                if (currentChild is IFrameworkInputElement frameworkInputElement && frameworkInputElement.Name == childName)
                {
                    // if the child's name is of the request name
                    foundChild = currentChild;
                    break;
                }

                // recursively drill down the tree
                foundChild = FindVisualChild<T>(currentChild, childName);
                // If the child is found, break, so we do not overwrite the found child. 
                if (foundChild is not null)
                {
                    break;
                }
            }
            else
            {
                // child element found.
                foundChild = currentChild;
                break;
            }
        }

        return foundChild;
    }


    /// <summary>
    /// Analyzes both visual and logical tree in order to find all elements of a given
    /// type that are descendants of the <paramref name="source"/> item.
    /// </summary>
    /// <typeparam name="T">The type of the queried items.</typeparam>
    /// <param name="source">The root element that marks the source of the search. If the
    /// source is already of the requested type, it will not be included in the result.</param>
    /// <param name="forceUsingTheVisualTreeHelper">Sometimes it's better to search in the VisualTree (e.g. in tests)</param>
    /// <returns>All descendants of <paramref name="source"/> that match the requested type.</returns>
    public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject source, bool forceUsingTheVisualTreeHelper = false) where T : DependencyObject
    {
        if (source is not null)
        {
            var childObjects = GetChildObjects(source, forceUsingTheVisualTreeHelper);
            foreach (var child in childObjects)
            {
                // analyze if children match the requested type
                if (child is T childToFind)
                {
                    yield return childToFind;
                }

                // recurse tree
                foreach (T descendant in FindVisualChildren<T>(child, forceUsingTheVisualTreeHelper))
                {
                    yield return descendant;
                }
            }
        }
    }

    /// <summary>
    /// This method is an alternative to WPF's
    /// <see cref="VisualTreeHelper.GetChild"/> method, which also
    /// supports content elements. Keep in mind that for content elements,
    /// this method falls back to the logical tree of the element.
    /// </summary>
    /// <param name="parent">The item to be processed.</param>
    /// <param name="forceUsingTheVisualTreeHelper">Sometimes it's better to search in the VisualTree (e.g. in tests)</param>
    /// <returns>The submitted item's child elements, if available.</returns>
    public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject parent, bool forceUsingTheVisualTreeHelper = false)
    {
        if (parent is not null)
        {
            if (!forceUsingTheVisualTreeHelper && parent is ContentElement or FrameworkElement)
            {
                // use the logical tree for content / framework elements
                foreach (var obj in LogicalTreeHelper.GetChildren(parent))
                {
                    if (obj is DependencyObject dependencyObject)
                    {
                        yield return dependencyObject;
                    }
                }
            }
            else if (parent is Visual or Visual3D)
            {
                // use the visual tree per default
                var count = VisualTreeHelper.GetChildrenCount(parent);
                for (var i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(parent, i);
                }
            }
        }
    }
}