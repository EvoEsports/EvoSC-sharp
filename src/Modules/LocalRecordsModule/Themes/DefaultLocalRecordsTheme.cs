using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.LocalRecordsModule.Themes;

[Theme(Name = "Local Records", Description = "Default theme for the local records widget.")]
public class DefaultLocalRecordsTheme(IThemeManager theme) : Theme<DefaultLocalRecordsTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.LocalRecordsModule.Widget.AccentColor").To((int pos) => pos switch
        {
            1 => _theme.Gold,
            2 => _theme.Silver,
            3 => _theme.Bronze,
            _ => _theme.UI_AccentPrimary
        });

        Set("UI.LocalRecordsModule.Widget.Row.Bg").To(_theme.UI_BgPrimary);
        Set("UI.LocalRecordsModule.Widget.Row.Bg.Opacity").To(_theme.UI_Widget_Body_Bg_Opacity);
        Set("UI.LocalRecordsModule.Widget.Row.Bg.Highlight").To(ColorUtils.Lighten(_theme.UI_BgPrimary));

        return Task.CompletedTask;
    }
}

