using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Themes;

[Theme(Name = "Spectator Target Info", Description = "Default theme for the Spectator Target Info module.")]
public class DefaultSpectatorTargetInfoTheme : Theme<DefaultSpectatorTargetInfoTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.Bg").To(theme.UI_BgPrimary);
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.BgGrad1").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.BgGrad2").To(theme.UI_BgHighlight);
        
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.Bg").To(theme.UI_BgPrimary);
        
        return Task.CompletedTask;
    }
}
