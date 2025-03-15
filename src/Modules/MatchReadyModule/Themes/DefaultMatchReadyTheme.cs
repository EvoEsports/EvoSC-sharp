using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.MatchReadyModule.Themes;

[Theme(Name = "Match Ready", Description = "Default theme for the Match Ready module.")]
public class DefaultMatchReadyTheme : Theme<DefaultMatchReadyTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("MatchReadyModule.ReadyWidget.Default.Ready").To(theme.Green);
        Set("MatchReadyModule.ReadyWidget.Default.ReadySecondary").To(ColorUtils.Darken((string)ThemeOptions["MatchReadyModule.ReadyWidget.Default.Ready"]));
        Set("MatchReadyModule.ReadyWidget.Default.NotReady").To(theme.Red);
        Set("MatchReadyModule.ReadyWidget.Default.NotReadySecondary").To(ColorUtils.Darken((string)ThemeOptions["MatchReadyModule.ReadyWidget.Default.NotReady"]));
        
        Set("MatchReadyModule.ReadyWidget.Default.PlayersReady.Bg").To(theme.UI_BgPrimary);
        Set("MatchReadyModule.ReadyWidget.Default.PlayersReady.Text").To(theme.UI_TextPrimary);
        
        Set("MatchReadyModule.ReadyWidget.Default.BgReady").To(ThemeOptions["MatchReadyModule.ReadyWidget.Default.Ready"]);
        Set("MatchReadyModule.ReadyWidget.Default.BgNotReady").To(ThemeOptions["MatchReadyModule.ReadyWidget.Default.NotReady"]);
        Set("MatchReadyModule.ReadyWidget.Default.Text").To(theme.UI_TextPrimary);
        
        Set("MatchReadyModule.ReadyWidget.Default.Button.BgGrad1").To(ThemeOptions["MatchReadyModule.ReadyWidget.Default.NotReadySecondary"]);
        Set("MatchReadyModule.ReadyWidget.Default.Button.BgGrad2").To(ThemeOptions["MatchReadyModule.ReadyWidget.Default.NotReady"]);
        Set("MatchReadyModule.ReadyWidget.Default.Button.Text").To(theme.Black);
        
        Set("MatchReadyModule.ReadyWidget.Default.BorderReady").To(ThemeOptions["MatchReadyModule.ReadyWidget.Default.Ready"]);
        Set("MatchReadyModule.ReadyWidget.Default.BorderNotReady").To(ThemeOptions["MatchReadyModule.ReadyWidget.Default.NotReady"]);

        return Task.CompletedTask;
    }
}
