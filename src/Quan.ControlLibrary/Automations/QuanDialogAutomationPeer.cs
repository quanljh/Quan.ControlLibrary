using System.Windows.Automation.Peers;
using Quan.ControlLibrary.Controls;

namespace Quan.ControlLibrary.Automations;

public class QuanDialogAutomationPeer(BaseQuanDialog owner) : FrameworkElementAutomationPeer(owner)
{
    protected override string GetClassNameCore()
    {
        return this.Owner.GetType().Name;
    }

    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Custom;
    }

    protected override string GetNameCore()
    {
        var nameCore = base.GetNameCore();
        if (string.IsNullOrEmpty(nameCore) && ((BaseQuanDialog)this.Owner).Title is string title)
        {
            nameCore = title;
        }

        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = ((BaseQuanDialog)this.Owner).Name;
        }

        if (string.IsNullOrEmpty(nameCore))
        {
            nameCore = this.GetClassNameCore();
        }

        return nameCore!;
    }
}