using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Quan.ControlLibrary
{
    public class BorderElement
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(BorderElement),
            new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.Inherits));

        public static CornerRadius GetCornerRadius(DependencyObject element) => (CornerRadius)element.GetValue(CornerRadiusProperty);
        public static void SetCornerRadius(DependencyObject element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Creates a clipping region from the parent <see cref="Border"/> <see cref="CornerRadius"/>
    /// </summary>
    public class ClipFromBorderProperty : BaseAttachedProperty<ClipFromBorderProperty, bool>
    {

        #region Private Members

        /// <summary>
        /// Called when the parent border first loads
        /// </summary>
        private RoutedEventHandler mBorder_Loaded;

        /// <summary>
        /// Called when the border size changed
        /// </summary>
        private SizeChangedEventHandler mBorder_SizeChanged;

        #endregion

        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is FrameworkElement element))
                return;

            //Check we have a parent border
            if (!(element.Parent is Border border))
            {
                Debugger.Break();
                return;
            }

            // Setup loaded event
            mBorder_Loaded = (ss, ee) => Border_OnChange(ss, ee, element);

            // Setup Size changed event 
            mBorder_SizeChanged = (ss, ee) => Border_OnChange(ss, ee, element);

            // If true,hook into events
            if ((bool)e.NewValue)
            {
                border.Loaded += mBorder_Loaded;
                border.SizeChanged += mBorder_SizeChanged;
            }
            // Otherwise, unhook
            else
            {
                border.Loaded -= mBorder_Loaded;
                border.SizeChanged -= mBorder_SizeChanged;
            }
        }

        /// <summary>
        /// Called when the border is loaded and changed size
        /// </summary>
        /// <param name="sender">The border</param>
        /// <param name="e"></param>
        /// <param name="child">The child element (our selves)</param>
        private void Border_OnChange(object sender, RoutedEventArgs e, FrameworkElement child)
        {
            //Get Border
            var border = (Border)sender;

            // Check we have an actual size
            if (border.ActualWidth == 0 && border.ActualHeight == 0)
                return;

            // Setup the new child clipping area
            var rect = new RectangleGeometry();

            // Match the corner radius with the borders corner radius
            rect.RadiusX = rect.RadiusY = Math.Max(0, border.CornerRadius.TopLeft - (border.BorderThickness.Left * 0.5));

            // Set rectangle size to match child's actual size
            rect.Rect = new Rect(child.RenderSize);

            // Assign clipping area to the child
            child.Clip = rect;
        }
    }

}
