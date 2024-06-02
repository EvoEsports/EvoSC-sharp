using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.FastestCpModule.Themes;

[Theme(Name = "Fastest CP", Description = "Default them for the Fastest CP module.")]
public class DefaultFastestCPTheme(IThemeManager theme) : Theme<DefaultFastestCPTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("FastestCpModule.FastestCP.Default.Text").To(_theme.UI_TextPrimary);
        Set("FastestCpModule.FastestCP.Default.Bg").To(_theme.UI_BgHighlight);
        Set("FastestCpModule.FastestCP.Default.Divider").To(_theme.UI_BgPrimary);
        
        return Task.CompletedTask;
    }
}
