// Copy from Modern WPF https://github.com/Kinnara/ModernWpf

using MS.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Quan.ControlLibrary
{
    /// <summary>
    /// Fixes the issue with window padding when window maximized
    /// </summary>
    internal class MaximizedWindowFixer
    {
        #region Constants

        private const int MONITORINFOF_PRIMARY = 0x00000001;
        private const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        #endregion

        #region Enum

        private enum Win32Message
        {
            WM_SETTINGCHANGE = 0x001A,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
        }

        private enum ABEdge
        {
            ABE_LEFT = 0,
            ABE_TOP = 1,
            ABE_RIGHT = 2,
            ABE_BOTTOM = 3
        }

        private enum ABMsg
        {
            ABM_GETSTATE = 4,
            ABM_GETTASKBARPOS = 5,
        }

        #endregion

        #region Struct

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public bool lParam;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// The window to handle the resizing for
        /// </summary>
        private Window _window;

        /// <summary>
        /// The maximized window border
        /// </summary>
        private Thickness? _maximizedWindowBorder;


        private HwndSource _hwndSource;

        private IntPtr _hwnd;


        private bool _isWindowPosAdjusted;

        private bool IsWindowPosAdjusted
        {
            get => _isWindowPosAdjusted;
            set
            {
                if (_isWindowPosAdjusted != value)
                {
                    _isWindowPosAdjusted = value;
                    InvalidateMaximizedWindowBorder();
                }
            }
        }

        #endregion

        #region Dependency Property

        public static readonly DependencyProperty MaximizedWindowFixerProperty =
            DependencyProperty.RegisterAttached(
                "MaximizedWindowFixer",
                typeof(MaximizedWindowFixer),
                typeof(MaximizedWindowFixer),
                new PropertyMetadata(OnMaximizedWindowFixerChanged));

        public static MaximizedWindowFixer GetMaximizedWindowFixer(Window window)
        {
            return (MaximizedWindowFixer)window.GetValue(MaximizedWindowFixerProperty);
        }

        public static void SetMaximizedWindowFixer(Window window, MaximizedWindowFixer value)
        {
            window.SetValue(MaximizedWindowFixerProperty, value);
        }

        private static void OnMaximizedWindowFixerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MaximizedWindowFixer oldValue)
            {
                oldValue.UnsetWindow();
            }

            if (e.NewValue is MaximizedWindowFixer newValue)
            {
                newValue.SetWindow((Window)d);
            }
        }

        #endregion

        #region DLL Imports

        [DllImport("shell32", CallingConvention = CallingConvention.StdCall)]
        private static extern uint SHAppBarMessage(ABMsg dwMessage, ref APPBARDATA pData);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        #endregion

        #region Initialize

        /// <summary>
        /// Initialize and hook into the windows message pump
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SourceInitialized(object sender, System.EventArgs e)
        {
            // Get the handle of this window
            var handle = (new WindowInteropHelper(_window)).Handle;
            var handleSource = HwndSource.FromHwnd(handle);

            // If not found, end
            if (handleSource == null)
                return;

            // Hook into it's Windows messages
            handleSource.AddHook(WindowProc);
        }

        #endregion

        #region Windows Message Listener

        /// <summary>
        /// Listens out for all windows messages for this window
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                // Handle the system window setting changed
                case (int)Win32Message.WM_SETTINGCHANGE:
                    InvalidateMaximizedWindowBorder();
                    UpdateWindowPadding();
                    break;

                // Once the window position has changing
                case (int)Win32Message.WM_WINDOWPOSCHANGING:
                    OnWindowPosChanging(lParam);
                    break;

                // Once the window has finished being moved
                case (int)Win32Message.WM_WINDOWPOSCHANGED:
                    if (_maximizedWindowBorder == null)
                        UpdateWindowPadding();
                    break;
            }

            return IntPtr.Zero;
        }

        #endregion

        #region Methods

        private void SetWindow(Window window)
        {
            UnsubscribeWindowEvents();

            _window = window;
            _hwnd = new WindowInteropHelper(window).Handle;

            _window.StateChanged += Window_StateChanged;
#if !(NET46 || NET461)
            _window.DpiChanged += Window_DpiChanged;
#endif
            _window.Closed += Window_Closed;

            if (_hwnd != IntPtr.Zero)
            {
                WindowSourceInitialized(null, null);
            }
            else
            {
                _window.SourceInitialized += WindowSourceInitialized;
            }
        }

        private void UnsetWindow()
        {
            UnsubscribeWindowEvents();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            UnsetWindow();
        }

        private void UnsubscribeWindowEvents()
        {
            if (_window != null)
            {
                _window.SourceInitialized -= WindowSourceInitialized;
                _window.StateChanged -= Window_StateChanged;
#if !(NET46 || NET461)
                _window.DpiChanged -= Window_DpiChanged;
#endif
                _window.Closed -= Window_Closed;
                _window.ClearValue(Control.PaddingProperty);
                _window = null;
            }

            if (_hwndSource != null)
            {
                _hwndSource.RemoveHook(new HwndSourceHook(WindowProc));
                _hwndSource = null;
            }

            _hwnd = IntPtr.Zero;
            _maximizedWindowBorder = null;
            _isWindowPosAdjusted = false;
        }

        private void WindowSourceInitialized(object sender, EventArgs e)
        {
            _hwnd = new WindowInteropHelper(_window).Handle;
            _hwndSource = HwndSource.FromHwnd(_hwnd);
            _hwndSource?.AddHook(new HwndSourceHook(WindowProc));

            UpdateWindowPadding();
        }

        private void UpdateWindowPadding()
        {
            if (_hwndSource == null || _hwndSource.IsDisposed || _hwndSource.CompositionTarget == null)
            {
                return;
            }

            if (_window.WindowState == WindowState.Maximized)
            {
                _window.Padding = _maximizedWindowBorder ?? GetMaximizedWindowBorder();
            }
            else
            {
                _window.ClearValue(Control.PaddingProperty);
            }
        }

        private void InvalidateMaximizedWindowBorder()
        {
            _maximizedWindowBorder = null;
        }

        private Thickness GetMaximizedWindowBorder()
        {
            if (IsWindowPosAdjusted)
            {
                return new Thickness();
            }

            double dpiScaleX, dpiScaleY;
#if !(NET46 || NET461)
            DpiScale dpi = VisualTreeHelper.GetDpi(_window);
            dpiScaleX = dpi.DpiScaleX;
            dpiScaleY = dpi.DpiScaleY;
#else
            Matrix transformToDevice = _hwndSource.CompositionTarget.TransformToDevice;
            dpiScaleX = transformToDevice.M11;
            dpiScaleY = transformToDevice.M22;
#endif

            int frameWidth = NativeMethods.GetSystemMetrics(SM.CXSIZEFRAME);
            int frameHeight = NativeMethods.GetSystemMetrics(SM.CYSIZEFRAME);
            int borderPadding = NativeMethods.GetSystemMetrics(SM.CXPADDEDBORDER);
            Size borderSize = new Size(frameWidth + borderPadding, frameHeight + borderPadding);
            Size borderSizeInDips = DpiHelper.DeviceSizeToLogical(borderSize, dpiScaleX, dpiScaleY);

            return new Thickness(borderSizeInDips.Width, borderSizeInDips.Height, borderSizeInDips.Width, borderSizeInDips.Height);
        }

        private static bool GetTaskbarAutoHide(out ABEdge edge)
        {
            IntPtr trayWnd = FindWindow("Shell_TrayWnd", null);
            if (trayWnd != IntPtr.Zero)
            {
                APPBARDATA abd = new APPBARDATA();
                abd.cbSize = Marshal.SizeOf(abd);
                abd.hWnd = trayWnd;
                SHAppBarMessage(ABMsg.ABM_GETTASKBARPOS, ref abd);
                bool autoHide = Convert.ToBoolean(SHAppBarMessage(ABMsg.ABM_GETSTATE, ref abd));
                edge = autoHide ? GetEdge(abd.rc) : default;
                return autoHide;
            }

            edge = default;
            return false;
        }

        private static ABEdge GetEdge(RECT rc)
        {
            if (rc.Top == rc.Left && rc.Bottom > rc.Right)
            {
                return ABEdge.ABE_LEFT;
            }
            else if (rc.Top == rc.Left && rc.Bottom < rc.Right)
            {
                return ABEdge.ABE_TOP;
            }
            else if (rc.Top > rc.Left)
            {
                return ABEdge.ABE_BOTTOM;
            }
            else
            {
                return ABEdge.ABE_RIGHT;
            }
        }

        private static void AdjustWindowPosForTaskbarAutoHide(ref WINDOWPOS pos, ABEdge edge)
        {
            pos.cx += pos.x * 2;
            pos.cy += pos.y * 2;
            pos.x = 0;
            pos.y = 0;

            switch (edge)
            {
                case ABEdge.ABE_LEFT:
                    pos.x = 2;
                    pos.cx -= 2;
                    break;
                case ABEdge.ABE_TOP:
                    pos.y = 2;
                    pos.cy -= 2;
                    break;
                case ABEdge.ABE_RIGHT:
                    pos.cx -= 2;
                    break;
                case ABEdge.ABE_BOTTOM:
                    pos.cy -= 2;
                    break;
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Monitors for whether the window is restored, minimized, or maximized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            UpdateWindowPadding();
        }


#if !(NET46 || NET461)
        private void Window_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            InvalidateMaximizedWindowBorder();
            UpdateWindowPadding();
        }
#endif

        private void OnWindowPosChanging(IntPtr lParam)
        {
            var pos = Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));

            if (pos == null)
                return;

            var winPos = (WINDOWPOS)pos;

            if ((winPos.flags & (int)SWP.NOSIZE) == 0)
            {
                bool windowPosAdjusted = false;

                WINDOWPLACEMENT placement = NativeMethods.GetWindowPlacement(winPos.hwnd);
                if (placement.showCmd == SW.MAXIMIZE)
                {
                    if (GetTaskbarAutoHide(out ABEdge edge))
                    {
                        var rect = new MS.Win32.NativeMethods.RECT(winPos.x, winPos.y, winPos.x + winPos.cx, winPos.y + winPos.cy);
                        IntPtr monitor = SafeNativeMethods.MonitorFromRect(ref rect, MONITOR_DEFAULTTONEAREST);
                        if (monitor != IntPtr.Zero)
                        {
                            MONITORINFO info = NativeMethods.GetMonitorInfo(monitor);
                            bool primary = (info.dwFlags & MONITORINFOF_PRIMARY) != 0;
                            if (primary)
                            {
                                if (winPos.x < 0 &&
                                    winPos.y < 0 &&
                                    winPos.cx > info.rcMonitor.Width &&
                                    winPos.cy > info.rcMonitor.Height)
                                {
                                    AdjustWindowPosForTaskbarAutoHide(ref winPos, edge);
                                    Marshal.StructureToPtr(winPos, lParam, true);
                                    windowPosAdjusted = true;
                                }
                                else if (winPos.x == 0 && winPos.y == 0)
                                {
                                    windowPosAdjusted = true;
                                }
                            }
                        }
                    }
                }

                IsWindowPosAdjusted = windowPosAdjusted;
            }
        }

        #endregion
    }

}
