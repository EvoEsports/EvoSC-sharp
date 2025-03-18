using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.TeamInfoModule.Themes;

[Theme(Name = "Team Info", Description = "Default theme for the Team Info module.")]
public class DefaultTeamInfoTheme : Theme<DefaultTeamInfoTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("TeamInfoModule.Widget.PointsBox.Text").To(theme.UI_TextPrimary);
        
        Set("TeamInfoModule.Widget.BottomInfo.Text").To(theme.UI_TextPrimary);
        Set("TeamInfoModule.Widget.BottomInfo.Bg").To(theme.UI_BgPrimary);
        Set("TeamInfoModule.Widget.BottomInfo.Bg.Opacity").To(0.87);
        
        Set("TeamInfoModule.Widget.EmblemBox.Text").To(theme.UI_TextPrimary);
        Set("TeamInfoModule.Widget.EmblemBox.Bg").To(theme.UI_HeaderBg);
        Set("TeamInfoModule.Widget.EmblemBox.Bg.Opacity").To(0.93);
        Set("TeamInfoModule.Widget.EmblemBox.Emblem.Opacity").To(1);
        
        Set("TeamInfoModule.Widget.GainedPoints.Text").To(theme.UI_TextPrimary);
        Set("TeamInfoModule.Widget.GainedPoints.Text.Size").To(theme.UI_FontSize);
        
        return Task.CompletedTask;
    }

    public override Task ConfigureDynamicAsync(dynamic theme)
    {
        // adjust background for opacity as it uses the Rectangle component
        Set("TeamInfoModule.Widget.BottomInfo.Bg").To(new ColorUtils()
            .Opacity(
                theme.TeamInfoModule_Widget_BottomInfo_Bg,
                theme.TeamInfoModule_Widget_BottomInfo_Bg_Opacity*100
            ));
        
        Set("TeamInfoModule.Widget.EmblemBox.Bg").To(new ColorUtils()
            .Opacity(
                theme.TeamInfoModule_Widget_EmblemBox_Bg,
                theme.TeamInfoModule_Widget_EmblemBox_Bg_Opacity*100
            ));

        return Task.CompletedTask;
    }
}
