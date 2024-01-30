using Prism.Mvvm;
using Prism.Regions;

namespace Quan.ControlLibrary.Demo.ViewModels.Controls;

public class QuanExpanderViewModel : BindableBase, INavigationAware
{
    #region Constructor

    public QuanExpanderViewModel()
    {

    }

    #endregion

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
}