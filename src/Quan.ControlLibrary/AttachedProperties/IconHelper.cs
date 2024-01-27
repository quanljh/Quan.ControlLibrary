using System.Windows;
using System.Windows.Media;

namespace Quan.ControlLibrary;

public static class IconHelper
{
    public static readonly DependencyProperty GeometryProperty = DependencyProperty.RegisterAttached(
        "Geometry",
        typeof(Geometry),
        typeof(IconHelper),
        new PropertyMetadata(default(Geometry)));

    public static Geometry GetGeometry(DependencyObject element) => (Geometry)element.GetValue(GeometryProperty);

    public static void SetGeometry(DependencyObject element, Geometry value) => element.SetValue(GeometryProperty, value);


    public static readonly DependencyProperty HeightProperty = DependencyProperty.RegisterAttached(
        "Height",
        typeof(double),
        typeof(IconHelper),
        new PropertyMetadata(double.NaN));

    public static double GetHeight(DependencyObject element) => (double)element.GetValue(HeightProperty);

    public static void SetHeight(DependencyObject element, double value) => element.SetValue(HeightProperty, value);

    public static readonly DependencyProperty WidthProperty = DependencyProperty.RegisterAttached(
        "Width",
        typeof(double),
        typeof(IconHelper),
        new PropertyMetadata(double.NaN));

    public static double GetWidth(DependencyObject element) => (double)element.GetValue(WidthProperty);

    public static void SetWidth(DependencyObject element, double value) => element.SetValue(WidthProperty, value);

}