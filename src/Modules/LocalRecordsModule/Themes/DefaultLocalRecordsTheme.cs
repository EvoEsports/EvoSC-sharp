using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.LocalRecordsModule.Themes;

[Theme(Name = "Local Records", Description = "Default theme for the local records widget.")]
public class DefaultLocalRecordsTheme: Theme<DefaultLocalRecordsTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.LocalRecordsModule.Widget.AccentColor").To((int pos) => pos switch
        {
            1 => theme.Gold,
            2 => theme.Silver,
            3 => theme.Bronze,
            _ => theme.UI_AccentPrimary
        });

        Set("UI.LocalRecordsModule.Widget.Row.Bg").To(theme.UI_BgPrimary);
        Set("UI.LocalRecordsModule.Widget.Row.Bg.Opacity").To(theme.UI_Widget_Body_Bg_Opacity);
        Set("UI.LocalRecordsModule.Widget.Row.Bg.Highlight").To(ColorUtils.Lighten(theme.UI_BgPrimary));

        return Task.CompletedTask;
    }
}

