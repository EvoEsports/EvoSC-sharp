using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.NextMapModule.Themes;

[Theme(Name = "Next Map", Description = "Default theme for the Next Map module.")]
public class DefaultNextMapTheme : Theme<DefaultNextMapTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("NextMapModule.NextMap.Default.Text").To(theme.UI_TextPrimary);
        Set("NextMapModule.NextMap.Default.BgHeaderGrad1").To(theme.UI_BgPrimary);
        Set("NextMapModule.NextMap.Default.BgHeaderGrad2").To(ColorUtils.Darken(theme.UI_BgPrimary));
        Set("NextMapModule.NextMap.Default.BgContent").To(theme.UI_BgHighlight);
        Set("NextMapModule.NextMap.Default.BgRow").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        Set("NextMapModule.NextMap.Default.Logo").To(theme.UI_LogoLight);
        
        return Task.CompletedTask;
    }
}
