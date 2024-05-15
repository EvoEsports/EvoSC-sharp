using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Checkbox Component", Description = "Default theme for the Checkbox component.")]
public class DefaultCheckboxTheme(IThemeManager theme) : Theme<DefaultCheckboxTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("UI.Checkbox.Default.Bg").To(_theme.UI_SurfaceBgPrimary);
        Set("UI.Checkbox.Default.Text").To(_theme.UI_TextPrimary);
        Set("UI.Checkbox.Default.CheckColor").To(_theme.UI_SurfaceBgSecondary);

        return Task.CompletedTask;
    }
}
