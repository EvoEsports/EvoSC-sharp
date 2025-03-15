using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.FastestCpModule.Themes;

[Theme(Name = "Fastest CP", Description = "Default them for the Fastest CP module.")]
public class DefaultFastestCPTheme: Theme<DefaultFastestCPTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("FastestCpModule.FastestCP.Default.Text").To(theme.UI_TextPrimary);
        Set("FastestCpModule.FastestCP.Default.Bg").To(theme.UI_BgHighlight);
        Set("FastestCpModule.FastestCP.Default.Divider").To(theme.UI_BgPrimary);
        
        return Task.CompletedTask;
    }
}
