using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class QuanButton : Button
    {
        #region Dependency Properties

        #region CornerRaidus

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(QuanButton), new PropertyMetadata(default(CornerRadius)));

        #endregion

        #endregion

        #region Constructor

        static QuanButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanButton), new FrameworkPropertyMetadata(typeof(QuanButton)));
        }

        #endregion
    }
}
