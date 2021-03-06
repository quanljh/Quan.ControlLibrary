﻿
using System.Windows;

namespace Quan.ControlLibrary
{
    public class QuanScaleHost : FrameworkElement
    {
        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(QuanScaleHost), new PropertyMetadata(0.0));


    }
}
