using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    [TemplateVisualState(GroupName = ActivationStatesGroupName, Name = ActiveStateName)]
    [TemplateVisualState(GroupName = ActivationStatesGroupName, Name = InactiveStateName)]
    public class QuanRippleLine : Control
    {
        #region Contants

        public const string ActivationStatesGroupName = "ActivationStates";

        public const string ActiveStateName = "Active";
        public const string InactiveStateName = "Inactive";

        #endregion

        #region Dependency Properties

        #region IsActive

        public bool IsActive
        {
            get => (bool)GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, BooleanBoxes.Box(value));
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(QuanRippleLine), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsRender, IsActive_OnPropertyChangedCallback));

        private static void IsActive_OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((QuanRippleLine)d).GotoVisualState(true);
        }

        #endregion

        #region CornerRadius

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(QuanRippleLine), new FrameworkPropertyMetadata(new CornerRadius(0), FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #endregion

        #region Constructor

        static QuanRippleLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanRippleLine), new FrameworkPropertyMetadata(typeof(QuanRippleLine)));
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            GotoVisualState(false);
        }

        #endregion

        #region Methods

        private void GotoVisualState(bool useTransitions) =>
            VisualStateManager.GoToState(this, IsActive ? ActiveStateName : InactiveStateName, useTransitions);

        #endregion
    }
}
