using Prism.Ioc;
using Prism.Unity;
using System.Windows;
using Prism.Mvvm;
using Quan.ControlLibrary;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ICusomerStore, DbCustomerStore>();
        }

        /// <inheritdoc />
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }

        /// <inheritdoc />
        protected override Window CreateShell()
        {
            var mainWin = Container.Resolve<MainWindow>();
            return mainWin;
        }
    }
}
