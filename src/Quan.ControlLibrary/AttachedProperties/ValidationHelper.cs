using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Quan.ControlLibrary
{
    public static class ValidationHelper
    {
        public enum DisplayMode
        {
            Text,
            Popup
        }

        #region Background

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.RegisterAttached(
            "Background",
            typeof(Brush),
            typeof(ValidationHelper),
            new PropertyMetadata(default(Brush)));

        public static Brush GetBackground(DependencyObject element) => (Brush)element.GetValue(BackgroundProperty);
        public static void SetBackground(DependencyObject element, Brush value) => element.SetValue(BackgroundProperty, value);

        #endregion

        #region FontSize

        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize",
                typeof(double),
                typeof(ValidationHelper),
                new PropertyMetadata(15.0));

        public static double GetFontSize(DependencyObject element) => (double)element.GetValue(FontSizeProperty);
        public static void SetFontSize(DependencyObject element, double value) => element.SetValue(FontSizeProperty, value);

        #endregion

        #region HorizontalAlignment

        public static readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.RegisterAttached("HorizontalAlignment",
                typeof(HorizontalAlignment),
                typeof(ValidationHelper),
                new PropertyMetadata(default(HorizontalAlignment)));

        public static HorizontalAlignment GetHorizontalAlignment(DependencyObject element) => (HorizontalAlignment)element.GetValue(HorizontalAlignmentProperty);
        public static void SetHorizontalAlignment(DependencyObject element, HorizontalAlignment value) => element.SetValue(HorizontalAlignmentProperty, value);

        #endregion

        #region CloseOnMouseLeftButtonDown

        public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty =
            DependencyProperty.RegisterAttached("CloseOnMouseLeftButtonDown",
                typeof(bool),
                typeof(ValidationHelper),
                new PropertyMetadata(default(bool)));

        public static bool GetCloseOnMouseLeftButtonDown(DependencyObject element) => (bool)element.GetValue(CloseOnMouseLeftButtonDownProperty);
        public static void SetCloseOnMouseLeftButtonDown(DependencyObject element, bool value) => element.SetValue(CloseOnMouseLeftButtonDownProperty, value);

        #endregion

        #region ShowValidationErrorOnMouseOver

        public static readonly DependencyProperty ShowValidationErrorOnMouseOverProperty =
            DependencyProperty.RegisterAttached("ShowValidationErrorOnMouseOver",
                typeof(bool),
                typeof(ValidationHelper),
                new PropertyMetadata(default(bool)));

        public static bool GetShowValidationErrorOnMouseOver(DependencyObject element) => (bool)element.GetValue(ShowValidationErrorOnMouseOverProperty);
        public static void SetShowValidationErrorOnMouseOver(DependencyObject element, bool value) => element.SetValue(ShowValidationErrorOnMouseOverProperty, value);

        #endregion

        #region DisplayMode

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.RegisterAttached("DisplayMode",
                typeof(DisplayMode),
                typeof(ValidationHelper),
                new PropertyMetadata(default(DisplayMode)));

        public static DisplayMode GetDisplayMode(DependencyObject element) => (DisplayMode)element.GetValue(DisplayModeProperty);
        public static void SetDisplayMode(DependencyObject element, DisplayMode value) => element.SetValue(DisplayModeProperty, value);

        #endregion

        #region PopupPlacement

        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.RegisterAttached(
            "PopupPlacement",
            typeof(PlacementMode),
            typeof(ValidationHelper),
            new FrameworkPropertyMetadata(PlacementMode.Right, FrameworkPropertyMetadataOptions.Inherits));

        public static PlacementMode GetPopupPlacement(DependencyObject element) => (PlacementMode)element.GetValue(PopupPlacementProperty);

        public static void SetPopupPlacement(DependencyObject element, PlacementMode value) => element.SetValue(PopupPlacementProperty, value);

        #endregion
    }
}
