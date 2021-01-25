using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Quan.ControlLibrary
{
    public static partial class FloatingProxyFabric
    {
        private sealed class FloatingProxyBuider
        {
            private readonly Func<Control, bool> _canBuild;
            private readonly Func<Control, IFloatingProxy> _build;

            public FloatingProxyBuider(Func<Control, bool> canBuild, Func<Control, IFloatingProxy> build)
            {
                _canBuild = canBuild ?? throw new ArgumentNullException(nameof(canBuild));
                _build = build ?? throw new ArgumentNullException(nameof(build));
            }

            public bool CanBuid(Control control) => _canBuild(control);
            public IFloatingProxy Build(Control control) => _build(control);
        }

        private static readonly List<FloatingProxyBuider> Buiders = new List<FloatingProxyBuider>();

        static FloatingProxyFabric()
        {
            Buiders.Add(new FloatingProxyBuider(c => c is TextBox, c => new TextBoxFloatingProxy((TextBox)c)));
        }

        public static void RegisterBuilder(Func<Control, bool> canBuild, Func<Control, IFloatingProxy> build)
            => Buiders.Add(new FloatingProxyBuider(canBuild, build));


        public static IFloatingProxy Get(Control control)
        {
            var builder = Buiders.FirstOrDefault(o => o.CanBuid(control));

            if (builder is null) throw new NotImplementedException();

            return builder.Build(control);
        }

    }
}
