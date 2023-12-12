using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "ToggleSwitch Component", Description = "Default theme for the ToggleSwitch component.")]
public class DefaultToggleSwitchTheme : Theme<DefaultToggleSwitchTheme>
{
    private readonly dynamic _theme;
    public DefaultToggleSwitchTheme(IThemeManager theme) => _theme = theme.Theme;
    
    public override Task ConfigureAsync()
    {
        Set("UI.ToggleSwitch.Default.OnText").To(_theme.UI_BgPrimary);
        Set("UI.ToggleSwitch.Default.OnBg").To(_theme.UI_BgPrimary);
        Set("UI.ToggleSwitch.Default.OffText").To(_theme.UI_BgSecondary);
        Set("UI.ToggleSwitch.Default.OffBg").To(_theme.UI_BgSecondary);
        Set("UI.ToggleSwitch.Default.BgSecondary").To(_theme.White);

        return Task.CompletedTask;
    }
}
