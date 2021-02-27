using Prism.Mvvm;

namespace Quan.ControlLibrary.Demo
{
    public class Demo : BindableBase
    {
        private string? _name;

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _viewName;

        public string? ViewName
        {
            get => _viewName;
            set => SetProperty(ref _viewName, value);
        }
    }
}
