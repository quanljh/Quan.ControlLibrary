using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Quan.ControlLibrary
{
    public class ExpanderRowDetailAttachedProperty : BaseAttachedProperty<ExpanderRowDetailAttachedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            base.OnValueChanged(sender, e);

            if (!(sender is ToggleButton toggle))
                return;

            if ((bool)e.NewValue)
                toggle.PreviewMouseLeftButtonUp += ToggleOnPreviewMouseLeftButtonUp;
            else
                toggle.PreviewMouseLeftButtonUp -= ToggleOnPreviewMouseLeftButtonUp;
        }

        private void ToggleOnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ToggleButton toggle))
                return;

            var dataGridRow = toggle.FindVisualParent<DataGridRow>();

            if (dataGridRow == null) return;

            if (!dataGridRow.IsSelected)
                dataGridRow.IsSelected = true;
        }
    }
}
