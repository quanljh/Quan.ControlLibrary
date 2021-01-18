using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class QuanTextBox : TextBox
    {
        #region Dependency Properties

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
