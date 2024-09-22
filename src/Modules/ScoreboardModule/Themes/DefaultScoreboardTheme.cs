using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.ScoreboardModule.Themes;

[Theme(Name = "Scoreboard", Description = "Default theme for the scoreboard.")]
public class DefaultScoreboardTheme(IThemeManager theme) : Theme<DefaultScoreboardTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("ScoreboardModule.BackgroundBox.BgHeader").To(_theme.UI_BgPrimary);
        Set("ScoreboardModule.BackgroundBox.BgHeaderGrad").To(ColorUtils.Darken(_theme.UI_BgPrimary));
        Set("ScoreboardModule.BackgroundBox.BgList").To(_theme.UI_BgHighlight);
        
        Set("ScoreboardModule.ScoreboardHeader.Text").To(_theme.UI_TextPrimary);
        Set("ScoreboardModule.ScoreboardHeader.Logo").To(_theme.UI_LogoLight);
        
        Set("ScoreboardModule.ClubTag.Bg").To(_theme.UI_BgHighlight);
        
        Set("ScoreboardModule.PlayerRow.Text").To(_theme.UI_TextPrimary);
        Set("ScoreboardModule.PlayerRow.CustomLabelBackground.Bg").To(_theme.Black);
        
        Set("ScoreboardModule.PlayerRow.PlayerActions.BgHighlight").To(_theme.UI_BgHighlight);
        
        Set("ScoreboardModule.PlayerRow.PlayerRowBackground.Bg").To(ColorUtils.Lighten(_theme.UI_BgHighlight));
        Set("ScoreboardModule.PlayerRow.PlayerRowBackground.BgHighlight").To(ColorUtils.SetLightness(_theme.UI_BgHighlight, 70));

        Set("ScoreboardModule.PlayerRow.PointsBox.Bg").To(ColorUtils.SetLightness(_theme.UI_BgHighlight, 70));
        Set("ScoreboardModule.PlayerRow.PointsBox.Text").To(ColorUtils.SetLightness(_theme.UI_BgHighlight, 20));
        
        Set("ScoreboardModule.PlayerRow.PositionBox.Bg").To(_theme.UI_BgHighlight);
        
        Set("ScoreboardModule.PlayerRow.FrameModel.Bg").To(_theme.UI_BgHighlight);
        
        Set("ScoreboardModule.PlayerRow.FrameModel.Text").To(_theme.UI_TextPrimary);
        Set("ScoreboardModule.PlayerRow.FrameModel.BgRow").To(_theme.UI_BgHighlight);
        Set("ScoreboardModule.PlayerRow.FrameModel.TextRoundPoints").To(_theme.UI_TextSecondary);
        
        Set("ScoreboardModule.Scoreboard.BgPosition").To(_theme.UI_BgHighlight);
        
        Set("ScoreboardModule.Settings.Text").To(_theme.UI_TextPrimary);

        return Task.CompletedTask;
    }
}
