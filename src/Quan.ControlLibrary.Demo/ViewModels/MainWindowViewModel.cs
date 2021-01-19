using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo
{
    public class MainWindowViewModel : BindableBase
    {
        #region Private Members

        private ICusomerStore _customerStore;

        #endregion

        #region Public Properties

        private string _selectedCustomer;

        public string SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value, () =>
            {
                Debug.WriteLine(_selectedCustomer ?? "no customer selected");
            });
        }


        public ObservableCollection<string> Customers { get; private set; } = new ObservableCollection<string>();

        public ReactiveProperty<string> UserName { get; }

        #endregion

        #region Commands

        private DelegateCommand _loadCommand;
        public DelegateCommand LoadCommand => _loadCommand ??= new DelegateCommand(Load);

        #endregion

        #region Constructor

        public MainWindowViewModel(ICusomerStore cusomerStore)
        {
            _customerStore = cusomerStore;

            UserName = new ReactiveProperty<string>(mode: ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateNotifyError(x => string.IsNullOrEmpty(x) ? "Email or Username is empty!" : null);
        }

        #endregion

        #region Methods

        private void Load()
        {
            Customers.Clear();
            List<string> list = _customerStore.GetAll();
            foreach (string item in list)
                Customers.Add(item);
        }

        #endregion
    }
}
