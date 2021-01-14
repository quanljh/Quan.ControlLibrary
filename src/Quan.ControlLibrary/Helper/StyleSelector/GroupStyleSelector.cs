using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Quan.ControlLibrary
{

    public class DataGridGroupSelector : StyleSelector
    {
        public Style GroupHeaderStyle { get; set; }

        public Style NoGroupHeaderStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (!(item is CollectionViewGroup group && group.Name != null))
                return NoGroupHeaderStyle;
            if (group.Name.ToString() == "")
                return NoGroupHeaderStyle;
            return GroupHeaderStyle;
        }
    }
}
