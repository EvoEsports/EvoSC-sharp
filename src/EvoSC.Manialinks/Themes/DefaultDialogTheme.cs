using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Dialog Theme", Description = "Default theme for the dialog component.")]
public class DefaultDialogTheme(IThemeManager theme) : Theme<DefaultDialogTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        return Task.CompletedTask;
    }
}
