using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Manialinks.Themes;

[Theme(Name = "Dialog Theme", Description = "Default theme for the dialog component.")]
public class DefaultDialogTheme : Theme<DefaultDialogTheme>
{
    public override Task ConfigureAsync(dynamic theme)
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
        
        Set("UI.Chip.Default.Text").To(theme.UI_TextSecondary);
        
        return Task.CompletedTask;
    }
}
