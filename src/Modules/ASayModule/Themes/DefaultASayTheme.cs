using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.ASayModule.Themes;

[Theme(Name = "ASay", Description = "Default theme for ASay")]
public class DefaultASayTheme : Theme<DefaultASayTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("ASayModule.Announcement.Default.TextSize").To(3);
        Set("ASayModule.Announcement.Default.IconSize").To(4);
        Set("ASayModule.Announcement.Default.Bg").To(theme.UI_BgPrimary);
        Set("ASayModule.Announcement.Default.BgSecondary").To(theme.UI_AccentSecondary);
        Set("ASayModule.Announcement.Default.Text").To(theme.UI_TextPrimary);
        
        return Task.CompletedTask;
    }
}
