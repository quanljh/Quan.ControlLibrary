using Prism.Regions;
using Reactive.Bindings;

namespace Quan.ControlLibrary.Demo.ViewModels.Controls;

public class QuanTextBoxViewModel : ViewModelBase, INavigationAware
{
    #region Properties

    private ReactiveProperty<string> _text1;
    public ReactiveProperty<string> Text1
        => _text1 ??= new ReactiveProperty<string>(mode: ReactivePropertyMode.Default | ReactivePropertyMode.IgnoreInitialValidationError)
            .SetValidateNotifyError(x => string.IsNullOrEmpty(x) ? "Email or Username is empty!" : null);

    private ReactiveProperty<string> _text2;
    public ReactiveProperty<string> Text2
        => _text2 ??= new ReactiveProperty<string>()
            .SetValidateNotifyError(x => x == null || x.Length < 8 ? "At least 8 characters" : null);

    #endregion

    #region Constructor

    public QuanTextBoxViewModel()
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