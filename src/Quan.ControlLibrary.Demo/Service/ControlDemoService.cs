using System.Collections.Generic;

namespace Quan.ControlLibrary.Demo;

public class ControlDemoService : IControlDemo
{
    /// <inheritdoc />
    public List<Demo> GetControlDemos()
    {
        return new List<Demo>
            {
                new Demo
                {
                    Name = ViewNameConstants.QuanTextBoxView.Substring(0, ViewNameConstants.QuanTextBoxView.Length - 4),
                    ViewName = ViewNameConstants.QuanTextBoxView,
                },
                new Demo
                {
                    Name =  ViewNameConstants.QuanButtonView.Substring(0, ViewNameConstants.QuanButtonView.Length - 4),
                    ViewName = ViewNameConstants.QuanButtonView,
                },
                new Demo()
                {
                    Name =  ViewNameConstants.QuanExpanderView.Substring(0, ViewNameConstants.QuanExpanderView.Length - 4),
                    ViewName = ViewNameConstants.QuanExpanderView,
                }
            };
    }
}