using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;

namespace Quan.ControlLibrary
{
    [TemplatePart(Name = PART_OuterBorder, Type = typeof(Border))]
    [TemplatePart(Name = PART_OuterDraggingBorder, Type = typeof(Border))]
    [TemplatePart(Name = PART_Icon, Type = typeof(UIElement))]
    public class QuanWindow : Window
    {
        #region Constants

        private const string PART_OuterBorder = "PART_OuterBorder";
        private const string PART_OuterDraggingBorder = "PART_OuterDraggingBorder";
        private const string PART_Icon = "PART_Icon";

        #endregion

        #region Private Members

        /// <summary>
        /// The outer border with the drop shadow margin
        /// </summary>
        private Border _windowOuterBorder;

        /// <summary>
        /// The outer dragging border
        /// </summary>
        private Border _windowOuterDraggingBorder;

        /// <summary>
        /// The window resizer helper that keeps the window size correct in various states
        /// </summary>
        private WindowResizer _windowResizer;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private CornerRadius _windowCornerRadius;

        #endregion

        #region Public Properties

        public CornerRadius WindowCornerRadius
        {
            get => (CornerRadius)GetValue(WindowCornerRadiusProperty);
            set
            {
                SetValue(WindowCornerRadiusProperty, value);
                if (!new CornerRadius(0).Equals(value))
                    _windowCornerRadius = new CornerRadius(value.TopLeft, value.TopRight, value.BottomRight, value.BottomLeft);
            }
        }

        public int TitleBarHeight
        {
            get => (int)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        public DataTemplate IconTemplate
        {
            get => (DataTemplate)GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }
        public bool ShowIconOnTitleBar
        {
            get => (bool)GetValue(ShowIconOnTitleBarProperty);
            set => SetValue(ShowIconOnTitleBarProperty, value);
        }

        public DataTemplate TitleTemplate
        {
            get => (DataTemplate)GetValue(TitleTemplateProperty);
            set => SetValue(TitleTemplateProperty, value);
        }

        public Thickness ResizeBorderThickness
        {
            get => (Thickness)GetValue(ResizeBorderThicknessProperty);
            set => SetValue(ResizeBorderThicknessProperty, value);
        }

        public bool IsMinButtonEnabled
        {
            get => (bool)GetValue(IsMinButtonEnabledProperty);
            set => SetValue(IsMinButtonEnabledProperty, value);
        }

        public bool IsMaxButtonEnabled
        {
            get => (bool)GetValue(IsMaxButtonEnabledProperty);
            set => SetValue(IsMaxButtonEnabledProperty, value);
        }

        public bool IsCloseButtonEnabled
        {
            get => (bool)GetValue(IsCloseButtonEnabledProperty);
            set => SetValue(IsCloseButtonEnabledProperty, value);
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty WindowCornerRadiusProperty = DependencyProperty.Register("WindowCornerRadius", typeof(CornerRadius), typeof(QuanWindow), new PropertyMetadata(new CornerRadius(10)));

        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register("TitleBarHeight", typeof(int), typeof(QuanWindow), new PropertyMetadata(35));

        public static readonly DependencyProperty ShowIconOnTitleBarProperty = DependencyProperty.Register("ShowIconOnTitleBar", typeof(bool), typeof(QuanWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(QuanWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(QuanWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register("ResizeBorderThickness", typeof(Thickness), typeof(QuanWindow), new PropertyMetadata(new Thickness(4)));

        public static readonly DependencyProperty IsMinButtonEnabledProperty = DependencyProperty.Register("IsMinButtonEnabled", typeof(bool), typeof(QuanWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty IsMaxButtonEnabledProperty = DependencyProperty.Register("IsMaxButtonEnabled", typeof(bool), typeof(QuanWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty IsCloseButtonEnabledProperty = DependencyProperty.Register("IsCloseButtonEnabled", typeof(bool), typeof(QuanWindow), new PropertyMetadata(true));

        #endregion

        #region Constructor

        static QuanWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanWindow), new FrameworkPropertyMetadata(typeof(QuanWindow)));
        }

        public QuanWindow()
        {
            // Fix window resize issue
            _windowResizer = new WindowResizer(this);

            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, args) => SystemCommands.MinimizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, args) => SystemCommands.MaximizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, args) => SystemCommands.RestoreWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, args) => SystemCommands.CloseWindow(this)));

            var chrome = new WindowChrome
            {
                CornerRadius = new CornerRadius(),
                GlassFrameThickness = new Thickness(0, 0, 0, 0),
                UseAeroCaptionButtons = false
            };

            BindingOperations.SetBinding(chrome, WindowChrome.CaptionHeightProperty, new Binding(TitleBarHeightProperty.Name) { Source = this });
            BindingOperations.SetBinding(chrome, WindowChrome.ResizeBorderThicknessProperty, new Binding(ResizeBorderThicknessProperty.Name) { Source = this });
            WindowChrome.SetWindowChrome(this, chrome);
        }

        #endregion

        #region Initialize

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _windowOuterBorder = GetTemplateChild(PART_OuterBorder) as Border;
            _windowOuterDraggingBorder = GetTemplateChild(PART_OuterDraggingBorder) as Border;

            if (!new CornerRadius(0).Equals(WindowCornerRadius))
                _windowCornerRadius = new CornerRadius(WindowCornerRadius.TopLeft, WindowCornerRadius.TopRight, WindowCornerRadius.BottomRight, WindowCornerRadius.BottomLeft);

            SetWindowEvents();
        }

        #endregion

        #region Methods

        private void ClearWindowEvents()
        {
            StateChanged -= OnStateChanged;
            _windowResizer.WindowDockChanged -= WindowResizerOnWindowDockChanged;
            _windowResizer.WindowStartedMove -= WindowResizerOnWindowStartedMove;
            _windowResizer.WindowFinishedMove -= WindowResizerOnWindowFinishedMove;
        }

        private void SetWindowEvents()
        {
            // Clear all event handlers first.
            ClearWindowEvents();

            StateChanged += OnStateChanged;

            // Listen out for dock changes
            _windowResizer.WindowDockChanged += WindowResizerOnWindowDockChanged;

            // On window being moved/dragged
            _windowResizer.WindowStartedMove += WindowResizerOnWindowStartedMove;

            // Fix dropping an undocked window at top which should be positioned at the
            // very top of screen
            _windowResizer.WindowFinishedMove += WindowResizerOnWindowFinishedMove;
        }

        private void WindowResizerOnWindowStartedMove()
        {
            _windowOuterDraggingBorder.BorderThickness = new Thickness(2);

        }

        private void WindowResizerOnWindowFinishedMove()
        {
            _windowOuterDraggingBorder.BorderThickness = new Thickness(0);

            // Check for moved to top of window and not at an edge
            if (mDockPosition == WindowDockPosition.Undocked && Math.Abs(Top - _windowResizer.CurrentScreenSize.Top) < 0.01)
                // If so, move it to the true top (the border size)
                Top = -_windowOuterBorder.BorderThickness.Top;
        }

        private void WindowResizerOnWindowDockChanged(WindowDockPosition dock)
        {
            // Store last position
            mDockPosition = dock;

            // Fire off resize events
            WindowResized();
        }

        private void OnStateChanged(object sender, EventArgs e) => WindowResized();


        private void WindowResized()
        {
            if (WindowState == WindowState.Maximized)
            {
                _windowOuterBorder.Padding = _windowResizer.CurrentMonitorMargin;
                _windowOuterBorder.BorderThickness = new Thickness(0);
                WindowCornerRadius = new CornerRadius(0);
                ResizeBorderThickness = new Thickness(_windowOuterBorder.Padding.Left, _windowOuterBorder.Padding.Top, _windowOuterBorder.Padding.Right, _windowOuterBorder.Padding.Bottom);
            }
            else
            {
                // if the window shouldn't have border thickness
                if (mDockPosition != WindowDockPosition.Undocked)
                {
                    _windowOuterBorder.Padding = new Thickness(0);
                    _windowOuterBorder.BorderThickness = new Thickness(1);
                    WindowCornerRadius = new CornerRadius(0);
                }
                else
                {
                    _windowOuterBorder.Padding = new Thickness(5);
                    _windowOuterBorder.BorderThickness = new Thickness(0);
                    WindowCornerRadius = new CornerRadius(_windowCornerRadius.TopLeft, _windowCornerRadius.TopRight, _windowCornerRadius.BottomRight, _windowCornerRadius.BottomLeft);
                }

                ResizeBorderThickness = new Thickness(
                    _windowOuterBorder.Padding.Left + 4,
                    _windowOuterBorder.Padding.Top + 4,
                    _windowOuterBorder.Padding.Right + 4,
                    _windowOuterBorder.Padding.Bottom + 4);

            }

        }

        #endregion

    }
}
