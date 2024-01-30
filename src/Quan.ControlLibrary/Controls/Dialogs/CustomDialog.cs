// ReSharper disable once CheckNamespace

namespace Quan.ControlLibrary.Controls;

/// <summary>
/// An implementation of BaseQuanDialog allowing arbitrary content.
/// </summary>
public class CustomDialog(QuanWindow parentWindow, QuanDialogSettings settings) : BaseQuanDialog(parentWindow, settings)
{
    public CustomDialog() : this(null, null)
    {
    }

    public CustomDialog(QuanWindow parentWindow) : this(parentWindow, null)
    {
    }

    public CustomDialog(QuanDialogSettings settings) : this(null, settings)
    {
    }
}