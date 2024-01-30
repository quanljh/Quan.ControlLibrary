using Prism.Regions;

namespace Quan.ControlLibrary.Demo.ViewModels.Controls;

public class QuanTimePickerViewModel : ViewModelBase, INavigationAware
{
    #region Properties


    #endregion

    #region Constructor

    public QuanTimePickerViewModel()
    {

        //UserName = new ReactiveProperty<string>()
        //    ;
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