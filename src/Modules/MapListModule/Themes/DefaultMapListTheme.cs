using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.MapListModule.Themes;

[Theme(Name = "Map List", Description = "Default theme for the Map List.")]
public class DefaultMapListTheme(IThemeManager theme) : Theme<DefaultMapListTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("MapListModule.MapListRow.Default.Bg").To(ColorUtils.Lighten(_theme.UI_BgSecondary));
        Set("MapListModule.MapListRow.Default.TagBg").To(ColorUtils.Lighten(_theme.UI_BgSecondary, 20));

        return Task.CompletedTask;
    }
}
