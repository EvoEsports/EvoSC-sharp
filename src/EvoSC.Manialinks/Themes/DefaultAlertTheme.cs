using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Alert Component", Description = "Default theme for the Alert component.")]
public class DefaultAlertTheme : Theme<DefaultAlertTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Alert.Default.Text").To(theme.UI_TextPrimary);
        Set("UI.Alert.Default.BgPrimary").To(theme.UI_SurfaceBgPrimary);
        Set("UI.Alert.Default.BgSecondary").To(theme.UI_SurfaceBgSecondary);

        return Task.CompletedTask;
    }
}
