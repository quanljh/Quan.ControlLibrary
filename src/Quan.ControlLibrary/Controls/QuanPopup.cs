using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using Quan.ControlLibrary.AttachedProperties;
using Quan.ControlLibrary.Helpers;
using Quan.ControlLibrary.Helpers.Boxes;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Quan.ControlLibrary.Controls;

public class QuanPopup : Popup
{
    #region Fields

    private Window _hostWindow;
    private ScrollViewer _scrollViewer;

    private static readonly IntPtr HWND_TOPMOST = new(-1);
    private static readonly IntPtr HWND_NOTOPMOST = new(-2);
    private static readonly IntPtr HWND_TOP = new(0);
    private static readonly IntPtr HWND_BOTTOM = new(1);

    #endregion

    #region Dependency Porperties

    #region CloseOnMouseLeftButtonDown

    /// <summary>
    /// Gets or sets whether if the popup can be closed by left mouse button down.
    /// </summary>
    public bool CloseOnMouseLeftButtonDown
    {
        get => (bool)GetValue(CloseOnMouseLeftButtonDownProperty);
        set => SetValue(CloseOnMouseLeftButtonDownProperty, BooleanBoxes.Box(value));
    }

    public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty
        = DependencyProperty.Register(
            nameof(CloseOnMouseLeftButtonDown),
            typeof(bool),
            typeof(QuanPopup),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #region AdornedElement

    /// <summary>
    /// Gets or sets the <see cref="UIElement" /> that this <see cref="Popup" /> object is reserving space for.
    /// </summary>
    public UIElement AdornedElement
    {
        get => (UIElement)GetValue(AdornedElementProperty);
        set => SetValue(AdornedElementProperty, value);
    }

    public static readonly DependencyProperty AdornedElementProperty
        = DependencyProperty.Register(
            nameof(AdornedElement),
            typeof(UIElement),
            typeof(QuanPopup),
            new PropertyMetadata(default(UIElement)));

    #endregion

    #region CanShow

    /// <summary>
    /// Gets whether the popup can be shown (useful for transitions).
    /// </summary>
    public bool CanShow
    {
        get => (bool)GetValue(CanShowPropertyKey.DependencyProperty);
        protected set => SetValue(CanShowPropertyKey, BooleanBoxes.Box(value));
    }

    private static readonly DependencyPropertyKey CanShowPropertyKey
        = DependencyProperty.RegisterReadOnly(
            nameof(CanShow),
            typeof(bool),
            typeof(QuanPopup),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    #endregion

    #endregion

    public QuanPopup()
    {
        Loaded += CustomValidationPopup_Loaded;
        Opened += CustomValidationPopup_Opened;
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (CloseOnMouseLeftButtonDown)
        {
            IsOpen = false;
        }
        else
        {
            var adornedElement = AdornedElement;
            if (adornedElement != null && ValidationHelper.GetCloseOnMouseLeftButtonDown(adornedElement))
            {
                IsOpen = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }

    private void CustomValidationPopup_Loaded(object sender, RoutedEventArgs e)
    {
        var adornedElement = AdornedElement;
        if (adornedElement is null)
        {
            return;
        }

        _hostWindow = Window.GetWindow(adornedElement);
        if (_hostWindow is null)
        {
            return;
        }

        CanShow = false;

        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
        }

        _scrollViewer = adornedElement.FindVisualParent<ScrollViewer>();
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        _hostWindow.LocationChanged -= OnSizeOrLocationChanged;
        _hostWindow.LocationChanged += OnSizeOrLocationChanged;
        _hostWindow.SizeChanged -= OnSizeOrLocationChanged;
        _hostWindow.SizeChanged += OnSizeOrLocationChanged;
        _hostWindow.StateChanged -= OnHostWindowStateChanged;
        _hostWindow.StateChanged += OnHostWindowStateChanged;
        _hostWindow.Activated -= OnHostWindowActivated;
        _hostWindow.Activated += OnHostWindowActivated;
        _hostWindow.Deactivated -= OnHostWindowDeactivated;
        _hostWindow.Deactivated += OnHostWindowDeactivated;

        if (PlacementTarget is FrameworkElement frameworkElement)
        {
            frameworkElement.SizeChanged -= OnSizeOrLocationChanged;
            frameworkElement.SizeChanged += OnSizeOrLocationChanged;
        }

        RefreshPosition();
        CanShow = true;

        OnLoaded();

        Unloaded -= CustomValidationPopup_Unloaded;
        Unloaded += CustomValidationPopup_Unloaded;
    }

    private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        RefreshPosition();

        if (e.VerticalChange > 0 || e.VerticalChange < 0 || e.HorizontalChange > 0 || e.HorizontalChange < 0)
        {
            if (IsElementVisible(AdornedElement as FrameworkElement, _scrollViewer))
            {
                var adornedElement = AdornedElement;
                IsOpen = Validation.GetHasError(adornedElement!) && adornedElement!.IsKeyboardFocusWithin;
            }
            else
            {
                IsOpen = false;
            }
        }
    }

    private static bool IsElementVisible(FrameworkElement element, FrameworkElement container)
    {
        if (element is null || container is null || !element.IsVisible)
        {
            return false;
        }

        var bounds = element.TransformToAncestor(container)
                            .TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
        var rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
        return rect.IntersectsWith(bounds);
    }

    private void CustomValidationPopup_Opened(object sender, EventArgs e)
    {
        SetTopmostState(true);
    }

    private void OnHostWindowActivated(object sender, EventArgs e)
    {
        SetTopmostState(true);
    }

    private void OnHostWindowDeactivated(object sender, EventArgs e)
    {
        SetTopmostState(false);
    }

    private void CustomValidationPopup_Unloaded(object sender, RoutedEventArgs e)
    {
        OnUnLoaded();

        if (PlacementTarget is FrameworkElement frameworkElement)
        {
            frameworkElement.SizeChanged -= OnSizeOrLocationChanged;
        }

        if (_hostWindow != null)
        {
            _hostWindow.LocationChanged -= OnSizeOrLocationChanged;
            _hostWindow.SizeChanged -= OnSizeOrLocationChanged;
            _hostWindow.StateChanged -= OnHostWindowStateChanged;
            _hostWindow.Activated -= OnHostWindowActivated;
            _hostWindow.Deactivated -= OnHostWindowDeactivated;
        }

        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
        }

        Unloaded -= CustomValidationPopup_Unloaded;
        Opened -= CustomValidationPopup_Opened;
        _hostWindow = null;
    }

    protected virtual void OnLoaded()
    {
    }

    protected virtual void OnUnLoaded()
    {
    }

    private void OnHostWindowStateChanged(object sender, EventArgs e)
    {
        if (_hostWindow != null && _hostWindow.WindowState != WindowState.Minimized)
        {
            var adornedElement = AdornedElement;
            if (adornedElement != null)
            {
                PopupAnimation = PopupAnimation.None;
                IsOpen = false;
                var errorTemplate = adornedElement.GetValue(Validation.ErrorTemplateProperty);
                adornedElement.SetValue(Validation.ErrorTemplateProperty, null);
                adornedElement.SetValue(Validation.ErrorTemplateProperty, errorTemplate);
            }
        }
    }

    private void OnSizeOrLocationChanged(object sender, EventArgs e)
    {
        RefreshPosition();
    }

    private void RefreshPosition()
    {
        var offset = HorizontalOffset;
        // "bump" the offset to cause the popup to reposition itself on its own
        SetCurrentValue(HorizontalOffsetProperty, offset + 1);
        SetCurrentValue(HorizontalOffsetProperty, offset);
    }

    private bool? _appliedTopMost;

    private unsafe void SetTopmostState(bool isTop)
    {
        // Don't apply state if it's the same as incoming state
        if (_appliedTopMost == isTop)
        {
            return;
        }

        if (Child is null)
        {
            return;
        }

        if (PresentationSource.FromVisual(Child) is not HwndSource hwndSource)
        {
            return;
        }

        var handle = new HWND(hwndSource.Handle);

        var rect = new RECT();
        PInvoke.GetWindowRect(handle, &rect);
        if (rect is { left: 0, top: 0, right: 0, bottom: 0 })
        {
            return;
        }
        //Debug.WriteLine("setting z-order " + isTop);

        const SET_WINDOW_POS_FLAGS SWP_TOPMOST = SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE | SET_WINDOW_POS_FLAGS.SWP_NOOWNERZORDER | SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOMOVE | SET_WINDOW_POS_FLAGS.SWP_NOREDRAW | SET_WINDOW_POS_FLAGS.SWP_NOSENDCHANGING;

        var left = rect.left;
        var top = rect.top;
        var width = rect.GetWidth();
        var height = rect.GetHeight();
        if (isTop)
        {
            PInvoke.SetWindowPos(handle, HWND.HWND_TOPMOST, left, top, width, height, SWP_TOPMOST);
        }
        else
        {
            // Z-Order would only get refreshed/reflected if clicking the
            // the titlebar (as opposed to other parts of the external
            // window) unless I first set the popup to HWND_BOTTOM
            // then HWND_TOP before HWND_NOTOPMOST
            PInvoke.SetWindowPos(handle, HWND.HWND_BOTTOM, left, top, width, height, SWP_TOPMOST);
            PInvoke.SetWindowPos(handle, HWND.HWND_TOP, left, top, width, height, SWP_TOPMOST);
            PInvoke.SetWindowPos(handle, HWND.HWND_NOTOPMOST, left, top, width, height, SWP_TOPMOST);
        }

        _appliedTopMost = isTop;
    }
}