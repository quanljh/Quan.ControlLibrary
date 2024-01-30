using System.Globalization;
using System.Windows.Media.Effects;
using Quan.ControlLibrary.AttachedProperties;
using Quan.ControlLibrary.Helpers;

namespace Quan.ControlLibrary.Converters;

public class ShadowConverter : BaseValueConverter<ShadowEffect, DropShadowEffect>
{
    public override DropShadowEffect Convert(ShadowEffect value, object parameter, CultureInfo culture)
    {
        return value switch
        {
            ShadowEffect.Effect1 => Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect1")),
            ShadowEffect.Effect2 => Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect2")),
            ShadowEffect.Effect3 => Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect3")),
            ShadowEffect.Effect4 => Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect4")),
            ShadowEffect.Effect5 => Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect5")),
            _ => Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect0"))
        };
    }

    public override ShadowEffect ConvertBack(DropShadowEffect value, object parameter, CultureInfo culture)
    {
        throw new System.NotImplementedException();
    }

    private static DropShadowEffect Clone(DropShadowEffect dropShadowEffect)
    {
        if (dropShadowEffect is null) return null;
        return new DropShadowEffect()
        {
            BlurRadius = dropShadowEffect.BlurRadius,
            Color = dropShadowEffect.Color,
            Direction = dropShadowEffect.Direction,
            Opacity = dropShadowEffect.Opacity,
            RenderingBias = dropShadowEffect.RenderingBias,
            ShadowDepth = dropShadowEffect.ShadowDepth
        };
    }
}