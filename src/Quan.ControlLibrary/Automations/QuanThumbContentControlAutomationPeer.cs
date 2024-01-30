using System.Windows;
using System.Windows.Automation.Peers;

namespace Quan.ControlLibrary.Automations;

public class QuanThumbContentControlAutomationPeer(FrameworkElement owner) : FrameworkElementAutomationPeer(owner)
{
    protected override AutomationControlType GetAutomationControlTypeCore()
    {
        return AutomationControlType.Custom;
    }

    protected override string GetClassNameCore()
    {
        return "QuanThumbContentControl";
    }
}