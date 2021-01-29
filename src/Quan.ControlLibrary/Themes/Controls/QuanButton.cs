using System.Windows;
using System.Windows.Controls;

namespace Quan.ControlLibrary
{
    public class QuanButton : Button
    {

        #region Constructor

        static QuanButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanButton), new FrameworkPropertyMetadata(typeof(QuanButton)));
        }

        #endregion
    }
}
