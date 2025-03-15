using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.ASayModule.Themes;

[Theme(Name = "ASay", Description = "Default theme for ASay")]
public class DefaultASayTheme : Theme<DefaultASayTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("ASayModule.Announcement.Text").To(theme.UI_TextPrimary);
        Set("ASayModule.Announcement.Text.Size").To(3);
        
        Set("ASayModule.Announcement.Icon.Size").To(4);
        Set("ASayModule.Announcement.Icon").To(theme.UI_TextPrimary);
        
        Set("ASayModule.Announcement.Bg").To(theme.UI_BgPrimary);
        Set("ASayModule.Announcement.Bg.Opacity").To(0.7);
        Set("ASayModule.Announcement.Bg.Secondary").To(theme.UI_AccentPrimary);
        
        return Task.CompletedTask;
    }
}
