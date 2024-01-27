using System.ComponentModel;
using System.Windows;

namespace Quan.ControlLibrary;

public class DesignerHelper
{
    private static bool? _isInDesignMode;

    public static bool IsInDesignMode
    {
        get
        {
            _isInDesignMode
                ??= (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
            return _isInDesignMode.Value;
        }
    }
}