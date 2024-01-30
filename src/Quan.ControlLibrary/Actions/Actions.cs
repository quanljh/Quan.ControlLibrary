using Quan.ControlLibrary.Controls;
using Quan.ControlLibrary.Helpers;
using Quan.ControlLibrary.Helpers.Boxes;

namespace Quan.ControlLibrary.Actions;

public class CloseFlyoutAction : CommandTriggerAction
{
    private Flyout _associatedFlyout;

    private Flyout AssociatedFlyout => _associatedFlyout ??= AssociatedObject.FindVisualParent<Flyout>();

    protected override void Invoke(object parameter)
    {
        if (AssociatedObject is null or { IsEnabled: false })
        {
            return;
        }

        var command = Command;
        if (command != null)
        {
            var commandParameter = GetCommandParameter();
            if (command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }
        else
        {
            AssociatedFlyout?.SetCurrentValue(Flyout.IsOpenProperty, BooleanBoxes.FalseBox);
        }
    }

    protected override object GetCommandParameter()
    {
        var parameter = CommandParameter;
        if (parameter is null && PassAssociatedObjectToCommand)
        {
            parameter = AssociatedFlyout;
        }

        return parameter;
    }
}