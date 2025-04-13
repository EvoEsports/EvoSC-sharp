using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;

namespace EvoSC.Modules.Official.MatchSettingsEditorModule.Themes;

[Theme(Name = "MatchSettings Editor", Description = "Default theme for the match settings editor.")]
public class DefaultMatchSettingsEditorTheme : Theme<DefaultMatchSettingsEditorTheme>
{
    public override Task ConfigureAsync(dynamic theme)
    {
        Set("UI.MatchSettingsEditor.Overview.MatchSettingsRow.Bg").To(theme.UI_SurfaceBgPrimary);
        Set("UI.MatchSettingsEditor.Overview.MatchSettingsRow.Name").To(theme.UI_TextPrimary);
        Set("UI.MatchSettingsEditor.Overview.MatchSettingsRow.ScriptName").To(theme.UI_TextMuted);
        
        return Task.CompletedTask;
    }

    public override Task ConfigureDynamicAsync(dynamic theme)
    {
        Set("UI.MatchSettingsEditor.GetCommonScriptName").To((string? name) => (name ?? "<unknown>") switch
        {
            "Trackmania/TM_TMWTTeams_Online" => "TrackMania World Tour - Teams",
            "Trackmania/TM_TimeAttackDaily_Online" => "Time Attack Daily",
            "Trackmania/TM_TimeAttack_Online" => "Time Attack",
            "Trackmania/TM_Teams_Online.Script.txt" => "Teams",
            "Trackmania/TM_Teams_Matchmaking_Online" => "Teams Matchmaking",
            "Trackmania/TM_RoyalTimeAttack_Online" => "Royal Time Attack",
            "Trackmania/TM_Royal_Online" => "Royal",
            "Trackmania/TM_Laps_Online" => "Laps",
            "Trackmania/TM_KnockoutDaily_Online" => "Knockout Daily",
            "Trackmania/TM_HotSeat_Local" => "HotSeat",
            "Trackmania/TM_Campaign_Local" => "Campaign",
            _ => name
        });
        
        return Task.CompletedTask;
    }
}
