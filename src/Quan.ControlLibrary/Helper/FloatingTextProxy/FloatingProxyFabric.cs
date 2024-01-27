using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Quan.ControlLibrary;

public static partial class FloatingProxyFabric
{
    private sealed class FloatingProxyBuilder(Func<Control, bool> canBuild, Func<Control, IFloatingProxy> build)
    {
        private readonly Func<Control, bool> _canBuild = canBuild ?? throw new ArgumentNullException(nameof(canBuild));
        private readonly Func<Control, IFloatingProxy> _build = build ?? throw new ArgumentNullException(nameof(build));

        public bool CanBuild(Control control) => _canBuild(control);
        public IFloatingProxy Build(Control control) => _build(control);
    }

    private static readonly List<FloatingProxyBuilder> Builders = [];

    static FloatingProxyFabric()
    {
        Builders.Add(new FloatingProxyBuilder(c => c is TextBox, c => new TextBoxFloatingProxy((TextBox)c)));
    }

    public static void RegisterBuilder(Func<Control, bool> canBuild, Func<Control, IFloatingProxy> build)
        => Builders.Add(new FloatingProxyBuilder(canBuild, build));


    public static IFloatingProxy Get(Control control)
    {
        var builder = Builders.FirstOrDefault(o => o.CanBuild(control));

        return builder is null ? throw new NotImplementedException() : builder.Build(control!);
    }

}