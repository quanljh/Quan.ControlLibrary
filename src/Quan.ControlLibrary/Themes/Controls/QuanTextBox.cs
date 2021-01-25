using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class QuanTextBox : TextBox
    {
        #region Enum

        public enum DisplayType
        {
            Normal,
            Floating
        }

        #endregion

        #region Dependency Properties

        #region DisplayMode

        public DisplayType DisplayMode
        {
            get => (DisplayType)GetValue(DisplayModeProperty);
            set => SetValue(DisplayModeProperty, value);
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(DisplayType), typeof(QuanTextBox), new PropertyMetadata(default(DisplayType)));

        #endregion

        #endregion

        #region Constructor

        static QuanTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanTextBox), new FrameworkPropertyMetadata(typeof(QuanTextBox)));
        }

        #endregion

        public string GuideText
        {
            get => (string)GetValue(TextBoxHelper.GuideTextProperty);
            set => SetValue(TextBoxHelper.GuideTextProperty, value);
        }
    }
}
