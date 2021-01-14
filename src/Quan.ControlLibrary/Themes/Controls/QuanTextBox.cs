using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class QuanTextBox : TextBox
    {

        #region Dependency Properties


        public string GuideText
        {
            get => (string)GetValue(GuideTextProperty);
            set => SetValue(GuideTextProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="GuideText"/> dependency property to display some guide text when TextBox is empty
        /// </summary>
        public static readonly DependencyProperty GuideTextProperty =
            DependencyProperty.Register("GuideText", typeof(string), typeof(QuanTextBox), new PropertyMetadata(default(string)));



        #endregion

        #region Constructor

        static QuanTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanTextBox), new FrameworkPropertyMetadata(typeof(QuanTextBox)));
        }

        #endregion
    }
}
