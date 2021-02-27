using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace Quan.ControlLibrary.Demo
{
    public class MainWindowViewModel : BindableBase
    {
        #region Fields

        private readonly IRegionManager _regionManager;

        private readonly IControlDemo _controlDemo;

        #endregion

        #region Properties

        public ObservableCollection<Demo> ControlDemoCollection { get; private set; } = new ObservableCollection<Demo>();

        public ReactiveProperty<Demo?> SelectedControlDemo { get; }

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public MainWindowViewModel(IRegionManager regionManager, IControlDemo controlDemo)
        {
            _regionManager = regionManager;
            _controlDemo = controlDemo;

            SelectedControlDemo = new ReactiveProperty<Demo?>(default, ReactivePropertyMode.DistinctUntilChanged);

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

        private void OnSelectedControlDemoChanged(Demo? demo)
        {
            if (demo == null)
                return;
            _regionManager.RequestNavigate(ViewNameConstants.MainWindowContent, demo.ViewName);
        }

        #endregion
    }
}
