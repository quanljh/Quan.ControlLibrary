﻿using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using Quan.ControlLibrary.Helpers.Boxes;

namespace Quan.ControlLibrary.Actions;

/// <summary>
/// This CommandTriggerAction can be used to bind any event on any FrameworkElement to an <see cref="ICommand" />.
/// This trigger can only be attached to a FrameworkElement or a class deriving from FrameworkElement.
/// 
/// This class is inspired from Laurent Bugnion and his EventToCommand.
/// <web>http://www.mvvmlight.net</web>
/// <license> See license.txt in this solution or http://www.galasoft.ch/license_MIT.txt </license>
/// </summary>
public class CommandTriggerAction : TriggerAction<FrameworkElement>
{
    #region Private members

    /// <summary>
    /// Specifies whether the AssociatedObject should be passed to the bound RelayCommand.
    /// This happens only if the <see cref="CommandParameter"/> is not set.
    /// </summary>
    public bool PassAssociatedObjectToCommand { get; set; } = true;

    #endregion

    #region Command

    /// <summary>
    /// Gets or sets the command that this trigger is bound to.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="Command" /> dependency property
    /// </summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(CommandTriggerAction),
            new PropertyMetadata(null, (s, e) => OnCommandChanged(s as CommandTriggerAction, e)));

    #endregion

    #region CommandParameter

    /// <summary>
    /// Gets or sets an object that will be passed to the <see cref="Command" /> attached to this trigger.
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    /// <summary>
    /// Identifies the <see cref="CommandParameter" /> dependency property
    /// </summary>
    public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.Register(
            nameof(CommandParameter),
            typeof(object),
            typeof(CommandTriggerAction),
            new PropertyMetadata(
                null,
                (s, e) =>
                {
                    var sender = s as CommandTriggerAction;
                    if (sender?.AssociatedObject != null)
                    {
                        sender.EnableDisableElement();
                    }
                }));

    #endregion

    #region Overrides

    protected override void OnAttached()
    {
        base.OnAttached();
        EnableDisableElement();
    }

    protected override void Invoke(object parameter)
    {
        if (AssociatedObject is null || (AssociatedObject != null && !AssociatedObject.IsEnabled))
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
    }

    #endregion

    #region Methods

    private static void OnCommandChanged(CommandTriggerAction action, DependencyPropertyChangedEventArgs e)
    {
        if (action is null)
        {
            return;
        }

        if (e.OldValue is ICommand oldCommand)
        {
            oldCommand.CanExecuteChanged -= action.OnCommandCanExecuteChanged;
        }

        if (e.NewValue is ICommand newCommand)
        {
            newCommand.CanExecuteChanged += action.OnCommandCanExecuteChanged;
        }

        action.EnableDisableElement();
    }

    protected virtual object GetCommandParameter()
    {
        var parameter = CommandParameter;
        if (parameter is null && PassAssociatedObjectToCommand)
        {
            parameter = AssociatedObject;
        }

        return parameter;
    }

    private void EnableDisableElement()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        var command = Command;
        AssociatedObject.SetCurrentValue(UIElement.IsEnabledProperty, BooleanBoxes.Box(command is null || command.CanExecute(GetCommandParameter())));
    }

    private void OnCommandCanExecuteChanged(object sender, EventArgs e)
    {
        EnableDisableElement();
    }

    #endregion
}