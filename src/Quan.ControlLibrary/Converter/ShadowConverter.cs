using System.Globalization;
using System.Windows.Media.Effects;

namespace Quan.ControlLibrary
{
    public class ShadowConverter : BaseValueConverter<ShadowEffect, DropShadowEffect>
    {
        public override DropShadowEffect Convert(ShadowEffect value, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case ShadowEffect.Effect1:
                    return Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect1"));
                case ShadowEffect.Effect2:
                    return Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect2"));
                case ShadowEffect.Effect3:
                    return Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect3"));
                case ShadowEffect.Effect4:
                    return Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect4"));
                case ShadowEffect.Effect5:
                    return Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect5"));
                default:
                    return Clone(ResourceHelper.GetResource<DropShadowEffect>("Quan.ShadowEffects.Effect0"));
            }
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
}
