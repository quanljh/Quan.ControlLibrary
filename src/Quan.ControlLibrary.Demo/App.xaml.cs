using System.Reactive.Concurrency;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;
using Prism.Regions;
using Reactive.Bindings;
using Unity;

namespace Quan.ControlLibrary.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region Navigation

            containerRegistry.RegisterForNavigation<QuanTextBoxView>();
            containerRegistry.RegisterForNavigation<QuanButtonView>();
            containerRegistry.RegisterForNavigation<QuanExpanderView>();

            #endregion

            #region Services

            containerRegistry.Register<IControlDemo, ControlDemoService>();

            #endregion
        }

        /// <inheritdoc />
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
            ViewModelLocationProvider.Register<QuanTextBoxView, QuanTextBoxViewModel>();
            ViewModelLocationProvider.Register<QuanButtonView, QuanButtonViewModel>();
            ViewModelLocationProvider.Register<QuanExpanderView, QuanExpanderViewModel>();
        }

        /// <inheritdoc />
        protected override Window CreateShell()
        {
            var mainWin = Container.Resolve<MainWindow>();
            return mainWin;
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            base.OnInitialized();
            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RequestNavigate(ViewNameConstants.MainWindowContent, ViewNameConstants.QuanTextBoxView);
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ReactivePropertyScheduler.SetDefault(ImmediateScheduler.Instance);
        }
    }
}
