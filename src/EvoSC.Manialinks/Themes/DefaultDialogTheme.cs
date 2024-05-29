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
        Set("UI.Dialog.BtnWidth").To((bool showOk, bool showCancel, double width) =>
            showOk && showCancel
                ? width / 2 - 5
                : width - 10
        );

        Set("UI.Dialog.OkX").To((bool showCancel, double width) =>
            showCancel
                ? width - (width / 2 - 5) - 3.5
                : 3.5
        );
        
        Set("UI.Chip.Default.Text").To(_theme.UI_TextSecondary);
        
        return Task.CompletedTask;
    }
}
