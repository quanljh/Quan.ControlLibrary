using System.Collections.Generic;
using Quan.ControlLibrary.Demo.Constants;
using Quan.ControlLibrary.Demo.Service.Interface;

namespace Quan.ControlLibrary.Demo.Service;

public class ControlDemoService : IControlDemo
{
    /// <inheritdoc />
    public List<Models.Demo> GetControlDemos()
    {
        return
        [
            new Models.Demo
            {
                Name = ViewNameConstants.QuanTextBoxView.Substring(0, ViewNameConstants.QuanTextBoxView.Length - 4),
                ViewName = ViewNameConstants.QuanTextBoxView,
            },

            new Models.Demo
            {
                Name = ViewNameConstants.QuanButtonView.Substring(0, ViewNameConstants.QuanButtonView.Length - 4),
                ViewName = ViewNameConstants.QuanButtonView,
            },

            new Models.Demo()
            {
                Name = ViewNameConstants.QuanExpanderView.Substring(0, ViewNameConstants.QuanExpanderView.Length - 4),
                ViewName = ViewNameConstants.QuanExpanderView,
            },

            new Models.Demo()
            {
                Name = ViewNameConstants.QuanTimePickerView.Substring(0,
                    ViewNameConstants.QuanTimePickerView.Length - 4),
                ViewName = ViewNameConstants.QuanTimePickerView,
            }
        ];
    }
}