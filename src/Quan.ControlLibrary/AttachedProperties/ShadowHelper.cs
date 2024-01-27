using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Quan.ControlLibrary;

public enum ShadowEffect
{
    Effect0,
    Effect1,
    Effect2,
    Effect3,
    Effect4,
    Effect5
}

internal class ShadowLocalInfo
{
    public ShadowLocalInfo(double opacity)
    {
        DefaultOpacity = opacity;
    }

    public double DefaultOpacity { get; }
}

public static class ShadowHelper
{
    #region ShadowEffect

    public static readonly DependencyProperty ShadowEffectProperty = DependencyProperty.RegisterAttached(
        "ShadowEffect", typeof(ShadowEffect), typeof(ShadowHelper), new FrameworkPropertyMetadata(default(ShadowEffect), FrameworkPropertyMetadataOptions.AffectsRender));

    public static void SetShadowEffect(DependencyObject element, ShadowEffect value)
    {
        element.SetValue(ShadowEffectProperty, value);
    }

    public static ShadowEffect GetShadowEffect(DependencyObject element)
    {
        return (ShadowEffect)element.GetValue(ShadowEffectProperty);
    }

    #endregion

    #region LocalInfo

    private static readonly DependencyPropertyKey LocalInfoPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
        "LocalInfo", typeof(ShadowLocalInfo), typeof(ShadowHelper), new PropertyMetadata(default(ShadowLocalInfo)));

    private static void SetLocalInfo(DependencyObject element, ShadowLocalInfo value)
        => element.SetValue(LocalInfoPropertyKey, value);

    private static ShadowLocalInfo GetLocalInfo(DependencyObject element)
        => (ShadowLocalInfo)element.GetValue(LocalInfoPropertyKey.DependencyProperty);

    #endregion

    #region Darken

    public static readonly DependencyProperty DarkenProperty = DependencyProperty.RegisterAttached(
        "Darken", typeof(bool), typeof(ShadowHelper), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsRender, DarkenPropertyChangedCallback));

    public static void SetDarken(DependencyObject element, bool value)
    {
        element.SetValue(DarkenProperty, value);
    }

    public static bool GetDarken(DependencyObject element)
    {
        return (bool)element.GetValue(DarkenProperty);
    }

    private static void DarkenPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        var uiElement = dependencyObject as UIElement;
        var dropShadowEffect = uiElement?.Effect as DropShadowEffect;

        if (dropShadowEffect == null) return;

        if ((bool)dependencyPropertyChangedEventArgs.NewValue)
        {
            var shadowLocalInfo = GetLocalInfo(dependencyObject);
            if (shadowLocalInfo == null)
                SetLocalInfo(dependencyObject, new ShadowLocalInfo(dropShadowEffect.Opacity));

            var doubleAnimation = new DoubleAnimation(1, new Duration(TimeSpan.FromMilliseconds(350)))
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            dropShadowEffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
        }
        else
        {
            var shadowLocalInfo = GetLocalInfo(dependencyObject);
            if (shadowLocalInfo == null) return;

            var doubleAnimation = new DoubleAnimation(shadowLocalInfo.DefaultOpacity, new Duration(TimeSpan.FromMilliseconds(350)))
            {
                FillBehavior = FillBehavior.HoldEnd
            };
            dropShadowEffect.BeginAnimation(DropShadowEffect.OpacityProperty, doubleAnimation);
        }
    }

    #endregion
}