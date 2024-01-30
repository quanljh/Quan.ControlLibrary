using System.Windows;
using System.Windows.Automation.Peers;
using Quan.ControlLibrary.Controls;

namespace Quan.ControlLibrary.Automations;

public class WindowCommandsAutomationPeer(WindowCommands owner) : FrameworkElementAutomationPeer(owner)
{
    protected override string GetClassNameCore()
    {
        return "WindowCommands";
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.ToolBar;
    }

    protected override string GetNameCore()
    {
        var nameCore = base.GetNameCore();

        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = ((WindowCommands)Owner).Name;
        }

        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = GetClassNameCore();
        }

        return nameCore!;
    }

    protected override bool IsOffscreenCore()
    {
        return !((WindowCommands)Owner).HasItems || base.IsOffscreenCore();
    }

    protected override Point GetClickablePointCore()
    {
        if (!((WindowCommands)Owner).HasItems)
        {
            return new Point(double.NaN, double.NaN);
        }

        return base.GetClickablePointCore();
    }
}