using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Alert Component", Description = "Default theme for the Alert component.")]
public class DefaultAlertTheme : Theme<DefaultAlertTheme>
{
    private readonly dynamic _theme;
    public DefaultAlertTheme(IThemeManager theme) => _theme = theme.Theme;
    
    public override Task ConfigureAsync()
    {
        Set("UI.Alert.Default.Text").To(_theme.UI_TextPrimary);
        Set("UI.Alert.Default.BgSecondary").To(_theme.White);

        return Task.CompletedTask;
    }
}
