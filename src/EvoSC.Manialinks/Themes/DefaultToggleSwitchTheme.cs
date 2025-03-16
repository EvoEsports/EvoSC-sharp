using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "ToggleSwitch Component", Description = "Default theme for the ToggleSwitch component.")]
public class DefaultToggleSwitchTheme : Theme<DefaultToggleSwitchTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.ToggleSwitch.Default.OnText").To(theme.UI_BgPrimary);
        Set("UI.ToggleSwitch.Default.OnBg").To(theme.UI_BgPrimary);
        Set("UI.ToggleSwitch.Default.OffText").To(theme.UI_BgHighlight);
        Set("UI.ToggleSwitch.Default.OffBg").To(theme.UI_BgHighlight);
        Set("UI.ToggleSwitch.Default.BgSecondary").To(theme.White);

        return Task.CompletedTask;
    }
}
