
using System.Windows;

namespace Quan.ControlLibrary.Controls;

public class QuanScaleHost : FrameworkElement
{
    public double Scale
    {
        get => (double)GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
    }

    public static readonly DependencyProperty ScaleProperty =
        DependencyProperty.Register(nameof(Scale), typeof(double), typeof(QuanScaleHost), new PropertyMetadata(0.0));


}