using System.Windows;
using System.Windows.Controls;
using Point = System.Drawing.Point;

namespace Quan.ControlLibrary
{
    [TemplateVisualState(GroupName = ContentStatesGroupName, Name = FloatingPositionName)]
    [TemplateVisualState(GroupName = ContentStatesGroupName, Name = OriginalPositionName)]
    public class QuanFloatingText : Control
    {
        #region Constants

        public const string ContentStatesGroupName = "ContentStates";

        public const string FloatingPositionName = "FloatingPosition";
        public const string OriginalPositionName = "OriginalPosition";

        #endregion

        #region Dependency Properties

        public bool IsUseFloating
        {
            get => (bool)GetValue(IsUseFloatingProperty);
            set => SetValue(IsUseFloatingProperty, value);
        }

        public static readonly DependencyProperty IsUseFloatingProperty =
            DependencyProperty.Register("IsUseFloating", typeof(bool), typeof(QuanFloatingText), new PropertyMetadata(false));


        public object FloatingText
        {
            get => GetValue(FloatingTextProperty);
            set => SetValue(FloatingTextProperty, value);
        }

        public static readonly DependencyProperty FloatingTextProperty =
            DependencyProperty.Register("FloatingText", typeof(object), typeof(QuanFloatingText), new PropertyMetadata(null));


        public double FloatingScale
        {
            get => (double)GetValue(FloatingScaleProperty);
            set => SetValue(FloatingScaleProperty, value);
        }

        public static readonly DependencyProperty FloatingScaleProperty =
            DependencyProperty.Register("FloatingScale", typeof(double), typeof(QuanFloatingText), new PropertyMetadata(.75));



        public Point FloatingOffset
        {
            get => (Point)GetValue(FloatingOffsetProperty);
            set => SetValue(FloatingOffsetProperty, value);
        }

        public static readonly DependencyProperty FloatingOffsetProperty =
            DependencyProperty.Register("FloatingOffset", typeof(Point), typeof(QuanFloatingText), new PropertyMetadata(new Point(0, -15)));


        public double FloatingOpacity
        {
            get => (double)GetValue(FloatingOpacityProperty);
            set => SetValue(FloatingOpacityProperty, value);
        }

        public static readonly DependencyProperty FloatingOpacityProperty =
            DependencyProperty.Register("FloatingOpacity", typeof(double), typeof(QuanFloatingText), new PropertyMetadata(.46));

        #endregion

        #region Constructor

        static QuanFloatingText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanFloatingText), new FrameworkPropertyMetadata(typeof(QuanFloatingText)));
        }

        public QuanFloatingText()
        {

        }

        #endregion
    }
}
