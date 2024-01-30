using System.Windows;
using System.Windows.Data;

namespace Quan.ControlLibrary.Helpers.StyleSelector;

public class DataGridGroupSelector : System.Windows.Controls.StyleSelector
{
    public Style GroupHeaderStyle { get; set; }

    public Style NoGroupHeaderStyle { get; set; }

    public override Style SelectStyle(object item, DependencyObject container)
    {
        if (item is not CollectionViewGroup { Name: not null } group)
        {
            return NoGroupHeaderStyle;
        }

        return group.Name.ToString() == "" ? NoGroupHeaderStyle : GroupHeaderStyle;
    }
}