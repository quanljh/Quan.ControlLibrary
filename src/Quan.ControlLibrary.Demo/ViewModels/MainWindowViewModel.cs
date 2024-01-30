using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;
using Prism.Regions;
using Quan.ControlLibrary.Demo.Constants;
using Quan.ControlLibrary.Demo.Service.Interface;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo.ViewModels;

public class MainWindowViewModel : BindableBase
{
    #region Fields

    private readonly IRegionManager _regionManager;

    private readonly IControlDemo _controlDemo;

    #endregion

    #region Properties

    public ObservableCollection<Models.Demo> ControlDemoCollection { get; set; } = new ObservableCollection<Models.Demo>();

    public ReactiveProperty<Models.Demo> SelectedControlDemo { get; }

    #endregion

    #region Commands

    #endregion

    #region Constructor

    public MainWindowViewModel(IRegionManager regionManager, IControlDemo controlDemo)
    {
        _regionManager = regionManager;
        _controlDemo = controlDemo;

        SelectedControlDemo = new ReactiveProperty<Models.Demo>(default, ReactivePropertyMode.DistinctUntilChanged);

        Load();

        SelectedControlDemo.Subscribe(OnSelectedControlDemoChanged);
    }

    #endregion

    #region Methods

    private void Load()
    {
        var list = _controlDemo.GetControlDemos();
        foreach (var item in list)
            ControlDemoCollection.Add(item);

        SelectedControlDemo.Value = ControlDemoCollection.FirstOrDefault();
    }

    private void OnSelectedControlDemoChanged(Models.Demo demo)
    {
        if (demo == null)
            return;
        _regionManager.RequestNavigate(ViewNameConstants.MainWindowContent, demo.ViewName);
    }

    #endregion
}