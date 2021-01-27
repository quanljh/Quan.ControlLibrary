using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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



        #endregion

        #region Constructor

        static QuanRippleLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(QuanRippleLine), new FrameworkPropertyMetadata(typeof(QuanRippleLine)));
        }

        #endregion

    }
}
