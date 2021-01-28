using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Members

        private readonly IRegionManager _regionManager;

        private readonly IControlDemo _controlDemo;

        #endregion

        #region Public Properties

        public ObservableCollection<Demo> ControlDemoCollection { get; private set; } = new ObservableCollection<Demo>();

        public ReactiveProperty<Demo> SelectedControlDemo { get; }

        #endregion

        #region Commands

        #endregion

        #region Constructor

        public MainWindowViewModel(IRegionManager regionManager, IControlDemo controlDemo)
        {
            _regionManager = regionManager;
            _controlDemo = controlDemo;

            SelectedControlDemo = new ReactiveProperty<Demo>();

            Load();

            SelectedControlDemo.Skip(1).Subscribe(OnSelectedControlDemoChanged);
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

        private void OnSelectedControlDemoChanged(Demo demo)
        {
            _regionManager.RequestNavigate(ViewNameConstants.MainWindowContent, demo.ViewName);
        }

        #endregion
    }
}
