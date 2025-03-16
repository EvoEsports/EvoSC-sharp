using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.CurrentMapModule.Themes;

[Theme(Name = "Current Map", Description = "Default theme for the Current Map Module.")]
public class DefaultCurrentMapTheme : Theme<DefaultCurrentMapTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("CurrentMapModule.CurrentMapWidget.Default.BgHeaderGrad1").To(theme.UI_BgPrimary);
        Set("CurrentMapModule.CurrentMapWidget.Default.BgHeaderGrad2").To(ColorUtils.Darken(theme.UI_BgPrimary));
        Set("CurrentMapModule.CurrentMapWidget.Default.BgContent").To(theme.UI_BgHighlight);
        Set("CurrentMapModule.CurrentMapWidget.Default.BgRow").To(ColorUtils.Lighten(theme.UI_BgHighlight));
        Set("CurrentMapModule.CurrentMapWidget.Default.Logo").To(theme.UI_LogoLight);
        Set("CurrentMapModule.CurrentMapWidget.Default.Text").To(theme.UI_TextPrimary);
        
        return Task.CompletedTask;
    }
}
