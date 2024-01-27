using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary;

public class QuanExpander : Expander
{
    #region Constructor

    static QuanExpander()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanExpander), new FrameworkPropertyMetadata(typeof(QuanExpander)));
    }

    #endregion
}