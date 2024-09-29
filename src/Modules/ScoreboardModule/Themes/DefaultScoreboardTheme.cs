using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.ScoreboardModule.Themes;

[Theme(Name = "Scoreboard", Description = "Default theme for the scoreboard.")]
public class DefaultScoreboardTheme(IThemeManager theme) : Theme<DefaultScoreboardTheme>
{
    private readonly dynamic _theme = theme.Theme;

    public override Task ConfigureAsync()
    {
        Set("ScoreboardModule.Text_Color").To(_theme.UI_TextPrimary);

        Set("ScoreboardModule.Logo_URL").To("file://Media/Manialinks/Nadeo/Trackmania/Menus/TMLogo.dds");
        Set("ScoreboardModule.Logo_Width").To("20.0");
        Set("ScoreboardModule.Logo_Height").To("10.0");
        
        Set("ScoreboardModule.Background_Opacity").To("0.0");
        Set("ScoreboardModule.Background_Image").To("");
        
        Set("ScoreboardModule.Background_Header_Color").To(_theme.UI_HeaderBg);
        Set("ScoreboardModule.Background_Header_Opacity").To("0.95");
        
        Set("ScoreboardModule.Background_Legend_Color").To(_theme.UI_HeaderBg);
        Set("ScoreboardModule.Background_Legend_Opacity").To("1.0");
        Set("ScoreboardModule.Background_Legend_Text_Color").To(_theme.UI_TextPrimary);
        Set("ScoreboardModule.Background_Legend_Text_Opacity").To("0.75");
        
        Set("ScoreboardModule.Background_Row_Color").To(_theme.UI_BgPrimary);
        Set("ScoreboardModule.Background_Row_Opacity").To("0.9");
        
        Set("ScoreboardModule.Background_Hover_Color").To(_theme.UI_BgHighlight);
        Set("ScoreboardModule.Background_Hover_Opacity").To("0.9");
        
        Set("ScoreboardModule.PositionBox_ShowAccent").To("True");
        Set("ScoreboardModule.PositionBox_Color").To(_theme.UI_AccentSecondary);
        Set("ScoreboardModule.PositionBox_Opacity").To("1.0");
        Set("ScoreboardModule.PositionBox_TextColor").To(_theme.UI_TextSecondary);
        Set("ScoreboardModule.PositionBox_TextOpacity").To("1.0");
        
        Set("ScoreboardModule.GainedPoints.Color").To(_theme.UI_AccentPrimary);

        Set("ScoreboardModule.Background_Row_Flag_AlphaMaskUrl").To("file://Media/Manialinks/Nadeo/Trackmania/Menus/Common/Common_Flag_Mask.dds");
        
        Set("ScoreboardModule.FinalistColor").To("");
        Set("ScoreboardModule.WinnerColor").To("");
        
        return Task.CompletedTask;
    }
}
