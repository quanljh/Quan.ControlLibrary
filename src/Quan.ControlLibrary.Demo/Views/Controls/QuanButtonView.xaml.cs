using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo;

/// <summary>
/// QuanButtonView.xaml の相互作用ロジック
/// </summary>
public partial class QuanButtonView : UserControl
{
    public QuanButtonView()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var latestValue = "";
        var rp = new ReactiveProperty<string>();
        rp.PropertyChanged += (s, e) =>
        {
            latestValue = rp.Value;
        };
        rp.Value = "okazuki";
        Debug.WriteLine($"{rp.Value}, {latestValue}");
        rp.Value = "xin9le";
        Debug.WriteLine($"{rp.Value}, {latestValue}");
    }
}