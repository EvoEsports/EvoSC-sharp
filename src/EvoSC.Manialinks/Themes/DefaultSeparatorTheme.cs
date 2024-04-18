using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Separator Component", Description = "Default theme for the Separator component.")]
public class DefaultSeparatorTheme(IThemeManager theme) : Theme<DefaultSeparatorTheme>
{
    private readonly dynamic _theme = theme.Theme;
    
    public override Task ConfigureAsync()
    {
        var bgLuma = ColorUtils.Luma((string)_theme.UI_BgHighlight);
        
        Set("UI.Separator.Default.Bg").To(ColorUtils.Lighten((string)_theme.UI_BgHighlight, bgLuma < 50 ? -40 : 40));
        
        return Task.CompletedTask;
    }
}
