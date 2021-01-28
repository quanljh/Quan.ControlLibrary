using System;
using System.Collections.Generic;
using System.Text;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo
{
    public class QuanTextBoxViewModel : BindableBase, INavigationAware
    {
        #region Public Properties

        public ReactiveProperty<string> UserName { get; }

        #endregion

        #region Constructor

        public QuanTextBoxViewModel()
        {

            //UserName = new ReactiveProperty<string>(mode: ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError)
            //    .SetValidateNotifyError(x => string.IsNullOrEmpty(x) ? "Email or Username is empty!" : null);

            UserName = new ReactiveProperty<string>();
        }

        #endregion

        #region Navigation

        /// <inheritdoc />
        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion
    }
}
