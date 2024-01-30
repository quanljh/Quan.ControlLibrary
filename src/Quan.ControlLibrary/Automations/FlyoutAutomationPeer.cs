using System.Windows.Automation.Peers;
using Quan.ControlLibrary.Controls;

namespace Quan.ControlLibrary.Automations;

public class FlyoutAutomationPeer(Flyout owner) : FrameworkElementAutomationPeer(owner)
{
    protected override string GetClassNameCore()
    {
        return "Flyout";
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Custom;
    }

    protected override string GetNameCore()
    {
        string? nameCore = base.GetNameCore();
        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = ((Flyout)this.Owner).Header as string;
        }

        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = ((Flyout)this.Owner).Name;
        }

        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = this.GetClassNameCore();
        }

        return nameCore!;
    }
}