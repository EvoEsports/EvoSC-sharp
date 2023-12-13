using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.SpectatorTargetInfoModule.Themes;

[Theme(Name = "Spectator Target Info", Description = "Default theme for the Spectator Target Info module.")]
public class DefaultSpectatorTargetInfoTheme(IThemeManager theme) : Theme<DefaultSpectatorTargetInfoTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.Bg").To(_theme.UI_BgPrimary);
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.BgGrad1").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.BgGrad2").To(_theme.UI_BgSecondary);
        
        Set("SpectatorTargetInfoModule.SpectatorTargetInfo.Bg").To(_theme.UI_BgPrimary);
        
        return Task.CompletedTask;
    }
}
