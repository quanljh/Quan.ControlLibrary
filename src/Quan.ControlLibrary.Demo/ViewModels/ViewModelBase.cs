using Prism.Mvvm;
using System;
using System.Reactive.Disposables;

namespace Quan.ControlLibrary
{
    public class ViewModelBase : BindableBase, IDisposable
    {
        #region Fields

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        #endregion

        #region Constructor

        public ViewModelBase()
        {

        }

        #endregion

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
