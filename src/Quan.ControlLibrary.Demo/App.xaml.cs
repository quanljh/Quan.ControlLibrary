using System.Reactive.Concurrency;
using System.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;
using Quan.ControlLibrary.Demo.Constants;
using Quan.ControlLibrary.Demo.Service;
using Quan.ControlLibrary.Demo.Service.Interface;
using Quan.ControlLibrary.Demo.Views;
using Quan.ControlLibrary.Demo.Views.Controls;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo;

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
        containerRegistry.RegisterForNavigation<QuanTimePickerView>();

        #endregion

        #region Services

        containerRegistry.Register<IControlDemo, ControlDemoService>();

        #endregion
    }

    /// <inheritdoc />
    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
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