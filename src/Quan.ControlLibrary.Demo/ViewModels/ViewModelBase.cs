using System;
using System.Reactive.Disposables;
using Prism.Mvvm;

namespace Quan.ControlLibrary.Demo.ViewModels;

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