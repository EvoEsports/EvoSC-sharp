using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Checkbox Component", Description = "Default theme for the Checkbox component.")]
public class DefaultCheckboxTheme : Theme<DefaultCheckboxTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.Checkbox.Default.Bg").To(theme.UI_SurfaceBgPrimary);
        Set("UI.Checkbox.Default.Text").To(theme.UI_TextPrimary);
        Set("UI.Checkbox.Default.CheckColor").To(theme.UI_SurfaceBgSecondary);

        return Task.CompletedTask;
    }
}
