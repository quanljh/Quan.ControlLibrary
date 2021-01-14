using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// A
    /// </summary>
    public static class ExpanderAttachedProperty
    {
        /// <summary>
        /// Gets the toggle button style used for the ExpanderDirection left.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static Style GetExpanderLeftStyle(UIElement element)
        {
            return (Style)element.GetValue(ExpanderLeftStyleProperty);
        }

        /// <summary>
        /// Sets the toggle button style used for the ExpanderDirection left.
        /// </summary>
        public static void SetExpanderLeftStyle(UIElement element, Style value)
        {
            element.SetValue(ExpanderLeftStyleProperty, value);
        }


        public static readonly DependencyProperty ExpanderLeftStyleProperty = DependencyProperty.RegisterAttached("ExpanderLeftStyle", typeof(Style), typeof(ExpanderAttachedProperty), new FrameworkPropertyMetadata((Style)null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// Gets the toggle button style used for the ExpanderDirection right.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static Style GetExpanderRightStyle(UIElement element)
        {
            return (Style)element.GetValue(ExpanderRightStyleProperty);
        }

        /// <summary>
        /// Sets the toggle button style used for the ExpanderDirection right.
        /// </summary>
        public static void SetExpanderRightStyle(UIElement element, Style value)
        {
            element.SetValue(ExpanderRightStyleProperty, value);
        }


        public static readonly DependencyProperty ExpanderRightStyleProperty = DependencyProperty.RegisterAttached("ExpanderRightStyle", typeof(Style), typeof(ExpanderAttachedProperty), new FrameworkPropertyMetadata((Style)null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// Gets the toggle button style used for the ExpanderDirection left.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static Style GetExpanderUpStyle(UIElement element)
        {
            return (Style)element.GetValue(ExpanderUpStyleProperty);
        }

        /// <summary>
        /// Sets the toggle button style used for the ExpanderDirection left.
        /// </summary>
        public static void SetExpanderUpStyle(UIElement element, Style value)
        {
            element.SetValue(ExpanderUpStyleProperty, value);
        }


        public static readonly DependencyProperty ExpanderUpStyleProperty = DependencyProperty.RegisterAttached("ExpanderUpStyle", typeof(Style), typeof(ExpanderAttachedProperty), new FrameworkPropertyMetadata((Style)null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));


        /// <summary>
        /// Gets the toggle button style used for the ExpanderDirection left.
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Expander))]
        public static Style GetExpanderDownStyle(UIElement element)
        {
            return (Style)element.GetValue(ExpanderDownStyleProperty);
        }

        /// <summary>
        /// Sets the toggle button style used for the ExpanderDirection left.
        /// </summary>
        public static void SetExpanderDownStyle(UIElement element, Style value)
        {
            element.SetValue(ExpanderDownStyleProperty, value);
        }


        public static readonly DependencyProperty ExpanderDownStyleProperty = DependencyProperty.RegisterAttached("ExpanderDownStyle", typeof(Style), typeof(ExpanderAttachedProperty), new FrameworkPropertyMetadata((Style)null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));
    }
}
