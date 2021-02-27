using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Quan.ControlLibrary
{
    [TemplatePart(Name = PART_Icon, Type = typeof(Image))]
    [TemplatePart(Name = PART_MainContentBorder, Type = typeof(Border))]
    public class QuanWindow : Window
    {
        #region Constants

        private const string PART_Icon = "PART_Icon";
        private const string PART_MainContentBorder = "PART_MainContentBorder";

        #endregion

        #region Private Members

        private Image? _icon;
        private Border? _mainContentBorder;

        #endregion

        #region Public Properties

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

        public static readonly DependencyProperty ShowIconOnTitleBarProperty = DependencyProperty.Register("ShowIconOnTitleBar", typeof(bool), typeof(QuanWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(QuanWindow), new PropertyMetadata(null));

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
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, (sender, args) => SystemCommands.MinimizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, (sender, args) => SystemCommands.MaximizeWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, (sender, args) => SystemCommands.RestoreWindow(this)));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, (sender, args) => SystemCommands.CloseWindow(this)));
        }

        #endregion

        #region Initialize

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _icon = (Image)GetTemplateChild(PART_Icon);
            _mainContentBorder = (Border)GetTemplateChild(PART_MainContentBorder);
            SubscribeEvents();
        }



        #endregion

        #region Methods

        private void SubscribeEvents()
        {
            UnSubscribeEvents();

            // set mouse down/up for icon
            if (_icon != null && _icon.Visibility == Visibility.Visible)
            {
                _icon.MouseDown += Icon_OnMouseDown;
            }
        }

        private void Icon_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_mainContentBorder == null)
                return;

            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    Close();
                }
                else if (ShowIconOnTitleBar)
                {
                    var pointToWindow = _mainContentBorder.TranslatePoint(new Point(0, 0), this);
                    var pointToScreen = PointToScreen(pointToWindow);

#if !(NET46 || NET461)
                    var dpi = VisualTreeHelper.GetDpi(this);
                    var dpiScaleX = dpi.DpiScaleX;
                    var dpiScaleY = dpi.DpiScaleY;
#else
                    var source = PresentationSource.FromVisual(this);

                    if (source?.CompositionTarget == null)
                        return;

                    var dpiScaleX = source.CompositionTarget.TransformFromDevice.M11;
                    var dpiScaleY = source.CompositionTarget.TransformFromDevice.M22;
#endif
                    SystemCommands.ShowSystemMenu(this, DpiHelper.DevicePixelsToLogical(pointToScreen, dpiScaleX, dpiScaleY));
                }
            }
        }

        private void UnSubscribeEvents()
        {
            if (_icon != null)
            {
                _icon.MouseDown -= Icon_OnMouseDown;
            }
        }

        #endregion

    }
}
