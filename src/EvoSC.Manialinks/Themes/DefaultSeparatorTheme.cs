using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Separator Component", Description = "Default theme for the Separator component.")]
public class DefaultSeparatorTheme : Theme<DefaultSeparatorTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        var bgLuma = ColorUtils.Luma((string)theme.UI_BgHighlight);
        
        Set("UI.Separator.Default.Bg").To(theme.UI_BgHighlight);
        
        return Task.CompletedTask;
    }
}
