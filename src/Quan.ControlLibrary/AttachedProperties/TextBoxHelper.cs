using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Quan.ControlLibrary;

public static class TextBoxHelper
{
    #region IsMonitoring

    public static readonly DependencyProperty IsMonitoringProperty =
        DependencyProperty.RegisterAttached(
            "IsMonitoring",
            typeof(bool),
            typeof(TextBoxHelper),
            new UIPropertyMetadata(BooleanBoxes.FalseBox, OnIsMonitoringChanged));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetIsMonitoring(DependencyObject element) => (bool)element.GetValue(IsMonitoringProperty);

    public static void SetIsMonitoring(DependencyObject element, bool value) => element.SetValue(IsMonitoringProperty, BooleanBoxes.Box(value));

    private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBox textBox)
        {
            if ((bool)e.NewValue)
            {
                textBox.TextChanged += TextBox_OnTextChanged;
                textBox.GotFocus += TextBox_OnGotFocus;
            }
            else if (!(bool)e.NewValue)
            {
                textBox.TextChanged -= TextBox_OnTextChanged;
                textBox.GotFocus -= TextBox_OnGotFocus;
            }
        }
    }


    #endregion

    #region HasText

    public static readonly DependencyProperty HasTextProperty =
        DependencyProperty.RegisterAttached(
            "HasText",
            typeof(bool),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetHasText(DependencyObject element) => (bool)element.GetValue(HasTextProperty);

    public static void SetHasText(DependencyObject element, bool value) => element.SetValue(HasTextProperty, BooleanBoxes.Box(value));

    #endregion

    #region SelectAllOnFocus

    public static readonly DependencyProperty SelectAllOnFocusProperty = DependencyProperty.RegisterAttached(
        "SelectAllOnFocus",
        typeof(bool),
        typeof(TextBoxHelper),
        new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetSelectAllOnFocus(DependencyObject element) => (bool)element.GetValue(SelectAllOnFocusProperty);

    public static void SetSelectAllOnFocus(DependencyObject element, bool value) => element.SetValue(SelectAllOnFocusProperty, BooleanBoxes.Box(value));

    #endregion

    #region GuideText

    /// <summary>
    /// Uses to display some guide text when TextBox is empty
    /// </summary>
    public static readonly DependencyProperty GuideTextProperty =
        DependencyProperty.RegisterAttached("GuideText",
            typeof(string),
            typeof(TextBoxHelper),
            new PropertyMetadata(default(string)));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static string GetGuideText(DependencyObject element) => (string)element.GetValue(GuideTextProperty);

    public static void SetGuideText(DependencyObject element, string value) => element.SetValue(GuideTextProperty, value);

    #endregion

    #region GuideTextOpacity

    public static readonly DependencyProperty GuideTextOpacityProperty =
        DependencyProperty.RegisterAttached("GuideTextOpacity",
            typeof(double),
            typeof(TextBoxHelper),
            new PropertyMetadata(.46));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static double GetGuideTextOpacity(DependencyObject element) => (double)element.GetValue(GuideTextOpacityProperty);

    public static void SetGuideTextOpacity(DependencyObject element, double value) => element.SetValue(GuideTextOpacityProperty, value);

    #endregion

    #region IsShowClearButton

    public static readonly DependencyProperty IsShowClearButtonProperty =
        DependencyProperty.RegisterAttached(
            "IsShowClearButton",
            typeof(bool),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetIsShowClearButton(DependencyObject element) => (bool)element.GetValue(IsShowClearButtonProperty);

    public static void SetIsShowClearButton(DependencyObject element, bool value) => element.SetValue(IsShowClearButtonProperty, BooleanBoxes.Box(value));

    #endregion

    #region IsUseClearButton

    public static readonly DependencyProperty IsUseClearButtonProperty =
        DependencyProperty.RegisterAttached(
            "IsUseClearButton",
            typeof(bool),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(false, IsUseClearButton_OnChanged));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetIsUseClearButton(DependencyObject element) => (bool)element.GetValue(IsUseClearButtonProperty);

    public static void SetIsUseClearButton(DependencyObject element, bool value) => element.SetValue(IsUseClearButtonProperty, value);

    private static void IsUseClearButton_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is Button button))
            return;

        if (e.OldValue != e.NewValue)
        {
            button.Click -= ClearButton_OnClick;
            if ((bool)e.NewValue)
                button.Click += ClearButton_OnClick;
        }
    }

    private static void ClearButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!(sender is Button button))
            return;

        var parent = button.GetVisualParents()
            .FirstOrDefault(o => o is RichTextBox || o is TextBox || o is PasswordBox || o is ComboBox);

        if (parent == null)
            return;

        var clearCommand = GetClearButtonCommand(parent);
        var clearButtonCommandParameter = GetClearButtonCommandParmeter(parent) ?? parent;
        if (clearCommand != null && clearCommand.CanExecute(clearButtonCommandParameter))
        {
            if (parent is TextBox textBox)
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            clearCommand.Execute(clearButtonCommandParameter);
        }

        if (GetIsShowClearButton(parent))
        {
            if (parent is TextBox textBox)
            {
                textBox.Clear();
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }
        }
    }

    #endregion

    #region ClearButtonCommand

    public static readonly DependencyProperty ClearButtonCommandProperty =
        DependencyProperty.RegisterAttached("ClearButtonCommand",
            typeof(ICommand),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(null));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static ICommand GetClearButtonCommand(DependencyObject element) => (ICommand)element.GetValue(ClearButtonCommandProperty);

    public static void SetClearButtonCommand(DependencyObject element, ICommand value) => element.SetValue(ClearButtonCommandProperty, value);

    #endregion

    #region ClearButtonCommandParameter

    public static readonly DependencyProperty ClearButtonCommandParmeterProperty =
        DependencyProperty.RegisterAttached("ClearButtonCommandParmeter",
            typeof(object),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(null));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static object GetClearButtonCommandParmeter(DependencyObject element) => (object)element.GetValue(ClearButtonCommandParmeterProperty);

    public static void SetClearButtonCommandParmeter(DependencyObject element, object value) => element.SetValue(ClearButtonCommandParmeterProperty, value);

    #endregion

    #region IsShowFunctionButton

    /// <summary>
    /// Uses to display a helper button in the text box, such as clear button, guide button etc...
    /// </summary>
    public static readonly DependencyProperty IsShowFunctionButtonProperty =
        DependencyProperty.RegisterAttached("IsShowFunctionButton",
            typeof(bool),
            typeof(TextBoxHelper),
            new PropertyMetadata(BooleanBoxes.FalseBox));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetIsShowFunctionButton(DependencyObject element) => (bool)element.GetValue(IsShowFunctionButtonProperty);

    public static void SetIsShowFunctionButton(DependencyObject element, bool value) => element.SetValue(IsShowFunctionButtonProperty, BooleanBoxes.Box(value));

    #endregion

    #region IsUseFunctionButton

    public static readonly DependencyProperty IsUseFunctionButtonProperty =
        DependencyProperty.RegisterAttached(
            "IsUseFunctionButton",
            typeof(bool),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, IsUseFunctionButton_OnChanged));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static bool GetIsUseFunctionButton(DependencyObject element) => (bool)element.GetValue(IsUseFunctionButtonProperty);

    public static void SetIsUseFunctionButton(DependencyObject element, bool value) => element.SetValue(IsUseFunctionButtonProperty, BooleanBoxes.Box(value));

    private static void IsUseFunctionButton_OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (!(d is Button button))
            return;

        if (e.OldValue != e.NewValue)
        {
            button.Click -= FunctionButton_OnClick;
            if ((bool)e.NewValue)
                button.Click += FunctionButton_OnClick;
        }
    }

    private static void FunctionButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!(sender is Button button))
            return;

        var parent = button.GetVisualParents()
            .FirstOrDefault(o => o is RichTextBox || o is TextBox || o is PasswordBox || o is ComboBox);

        if (parent == null)
            return;

        var functionCommand = GetFunctionButtonCommand(parent);
        var functionButtonCommandParameter = GetFunctionButtonCommandParameter(parent) ?? parent;
        if (functionCommand != null && functionCommand.CanExecute(functionButtonCommandParameter))
        {
            if (parent is TextBox textBox)
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            functionCommand.Execute(functionButtonCommandParameter);
        }
    }

    #endregion

    #region FunctionButtonContent

    /// <summary>
    /// Uses to display a helper button in the text box, such as clear button, guide button etc...
    /// </summary>
    public static readonly DependencyProperty FunctionButtonContentProperty =
        DependencyProperty.RegisterAttached("FunctionButtonContent",
            typeof(object),
            typeof(TextBoxHelper),
            new PropertyMetadata());

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static object GetFunctionButtonContent(DependencyObject element) => (object)element.GetValue(FunctionButtonContentProperty);

    public static void SetFunctionButtonContent(DependencyObject element, object value) => element.SetValue(FunctionButtonContentProperty, value);

    #endregion

    #region FunctionButtonCommand

    public static readonly DependencyProperty FunctionButtonCommandProperty =
        DependencyProperty.RegisterAttached("FunctionButtonCommand",
            typeof(ICommand),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(null));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static ICommand GetFunctionButtonCommand(DependencyObject element) => (ICommand)element.GetValue(FunctionButtonCommandProperty);

    public static void SetFunctionButtonCommand(DependencyObject element, ICommand value) => element.SetValue(FunctionButtonCommandProperty, value);

    #endregion

    #region FunctionButtonCommandParameter

    public static readonly DependencyProperty FunctionButtonCommandParameterProperty =
        DependencyProperty.RegisterAttached("FunctionButtonCommandParameter",
            typeof(object),
            typeof(TextBoxHelper),
            new FrameworkPropertyMetadata(null));

    [Category(Constants.QUAN_APP)]
    [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
    [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
    [AttachedPropertyBrowsableForType(typeof(ComboBox))]
    public static object GetFunctionButtonCommandParameter(DependencyObject element) => (object)element.GetValue(FunctionButtonCommandParameterProperty);

    public static void SetFunctionButtonCommandParameter(DependencyObject element, object value) => element.SetValue(FunctionButtonCommandParameterProperty, value);

    #endregion

    #region Methods

    private static void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        SetTextLength(sender as TextBox, textBox => textBox.Text.Length);
    }

    private static void SetTextLength<TDependencyObject>(TDependencyObject sender, Func<TDependencyObject, int> funcTextLength)
        where TDependencyObject : DependencyObject
    {
        if (sender != null)
        {
            var value = funcTextLength(sender);
            sender.SetValue(HasTextProperty, value >= 1);
        }
    }

    private static void TextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        ControlGotFocus(sender as TextBoxBase, textBox => textBox.SelectAll());
    }

    private static void ControlGotFocus<TDependencyObject>(TDependencyObject sender, Action<TDependencyObject> action)
        where TDependencyObject : DependencyObject
    {
        if (sender != null && GetSelectAllOnFocus(sender))
            sender.Dispatcher.InvokeAsync(() => action, DispatcherPriority.Loaded);
    }

    #endregion
}