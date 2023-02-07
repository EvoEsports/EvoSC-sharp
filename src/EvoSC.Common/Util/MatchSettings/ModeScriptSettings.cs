using System.ComponentModel;
using EvoSC.Common.Util.MatchSettings.Attributes;

namespace EvoSC.Common.Util.MatchSettings;

/// <summary>
/// List of available match settings in Trackmania.
/// </summary>
public class ModeScriptSettings
{
    [ScriptSetting(Name = "S_BestLapBonusPoints")]
    [Description("Points bonus attributed to the player with the best lap")]
    protected int BestLapBonusPoints { get; set; }

    [ScriptSetting(Name = "S_ChatTime")]
    [Description("Chat time at the end of a map or match")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, 10)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, 6)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Laps, 10)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, 10)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TimeAttack, 10)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, 10)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, 600)]
    protected int ChatTime { get; set; }
    
    [ScriptSetting(Name = "S_CumulatePoints")]
    [Description("Cumulate points earned by players to their team score")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, false)]
    protected bool CumulatePoints { get; set; }
    
    [ScriptSetting(Name = "S_LoadingScreenImageUrl")]
    [Description("The image displayed when a player is loading the next map.")]
    protected string LoadingScreenImageUrl { get; set; }
    
    [ScriptSetting(Name = "S_DecoImageUrl_Checkpoint")]
    [Description("""
                Url of the image displayed on the checkpoints ground.
                Override the image set in the Club.
                """)]
    protected string DecoImageUrlCheckpoint { get; set; }
    
    [ScriptSetting(Name = "S_DecoImageUrl_DecalSponsor4x1")]
    [Description("""
                Url of the image displayed on the block border.
                Override the image set in the Club.
                """)]
    protected string DecoImageUrlDecalSponsor4X1 { get; set; }
    
    [ScriptSetting(Name = "S_DecoImageUrl_Screen16x1")]
    [Description("""
                Url of the image displayed below the podium and big screen.
                Override the image set in the Club.
                """)]
    protected string DecoImageUrlScreen16X1 { get; set; }
    
    [ScriptSetting(Name = "S_DecoImageUrl_Screen16x9")]
    [Description("""
                Url of the image displayed on the two big screens.
                Override the image set in the Club.
                """)]
    protected string DecoImageUrlScreen16X9 { get; set; }
    
    [ScriptSetting(Name = "S_DecoImageUrl_Screen8x1")]
    [Description("""
                Url of the image displayed on the bleachers.
                Override the image set in the Club.
                """)]
    protected string DecoImageUrlScreen8X1 { get; set; }
    
    [ScriptSetting(Name = "S_DecoImageUrl_WhoAmIUrl")]
    [Description("""
                Url of the API route to get the deco image url. 
                You can replace ":ServerLogin" with a login from a server in another club to use its images.
                """)]
    [DefaultScriptSettingValue("/api/club/room/:ServerLogin/whoami")]
    protected string DecoImageUrlWhoAmIUrl { get; set; }
    
    [ScriptSetting(Name = "S_DelayBeforeNextMap")]
    [Description("Minimal time before the server go to the next map in milliseconds.")]
    [DefaultScriptSettingValue(2000)]
    protected int DelayBeforeNextMap { get; set; }
    
    [ScriptSetting(Name = "S_DisableGiveUp")]
    [Description("Disable GiveUp, override S_RespawnBehaviour.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Laps, false)]
    protected bool DisableGiveUp { get; set; }
    
    [ScriptSetting(Name = "S_EarlyEndMatchCallback")]
    [Description("""
    Send End Match Callback early, used to speed up the API update in TMGL/OGL and the COTD.
    Very specific usage.
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, true)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, true)]
    protected bool EarlyEndMatchCallback { get; set; }
    
    [ScriptSetting(Name = "S_EliminatedPlayersNbRanks")]
    [Description("""
    Rank at which one more player is eliminated per round.
    You can use coma to add more values. For exemple, COTD use this setting: 8,16,16
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, "4,16,16")]
    protected string EliminatedPlayerNbRanks { get; set; }
    
    [ScriptSetting(Name = "S_EndRoundPostScoreUpdateDuration")]
    [Description("Time in seconds after score computed on scoreboard")]
    [DefaultScriptSettingValue(5)]
    protected int EndRoundPostScoreUpdateDuration { get; set; }
    
    [ScriptSetting(Name = "S_EndRoundPreScoreUpdateDuration")]
    [Description("Time in seconds before score computed on scoreboard")]
    [DefaultScriptSettingValue(5)]
    protected int EndRoundPreScoreUpdateDuration { get; set; }
    
    [ScriptSetting(Name = "S_FinishTimeout")]
    [Description("""
    Time to finish the race in seconds after the winners.
    Use -1 to based on the Author time ( 5 seconds + Author time / 6 )
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, 5)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Laps, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, -1)]
    protected int FinishTimeout { get; set; }
    
    [ScriptSetting(Name = "S_ForceLapsNb")]
    [Description("""
    Number of laps per rounds, can be override by S_InfiniteLaps
    -1    : use laps from map validation
    0     : independent laps (only useful in TimeAttack)
    > 0  : Nomber of laps
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Laps, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TimeAttack, 0)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, -1)]
    protected int ForceLapsNb { get; set; }
    
    [ScriptSetting(Name = "S_ForceWinnersNb")]
    [Description("""
    Force the number of players who can win points at the end of the round
    Set 0 to use S_WinnersRatio
    """)]
    [DefaultScriptSettingValue(0)]
    protected int ForceWinnersNb { get; set; }
    
    [ScriptSetting(Name = "S_InfiniteLaps")]
    [Description("Never end a race in laps, equivalent of S_ForceLapsNb = 0")]
    [DefaultScriptSettingValue(false)]
    protected bool InfiniteLaps { get; set; }
    
    [ScriptSetting(Name = "S_IsChannelServer")]
    [Description("This a channel's server")]
    [DefaultScriptSettingValue(false)]
    protected bool IsChannelServer { get; set; }
    
    [ScriptSetting(Name = "S_IsSplitScreen")]
    [Description("")]
    [DefaultScriptSettingValue(false)]
    protected bool IsSplitScreen { get; set; }
    
    [ScriptSetting(Name = "S_MapsPerMatch")]
    [Description("""
    Number of maps to play before finishing the match.
    Set 0 or -1 is equivalent to have only one map.
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, -1)]
    protected int MapsPerMatch { get; set; }
    
    [ScriptSetting(Name = "S_MatchPosition")]
    [Description("""
    Server number to define global player rank
    -1 :  Disable global ranking
    N : Position of the player + N * 64
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, -1)]
    protected int MatchPosition { get; set; }
    
    [ScriptSetting(Name = "S_MaxPointsPerRound")]
    [Description("""
    The maximum number of points attributed to the first player to cross the finish line.
    Only available when S_UseCustomPointsRepartition is set to false.
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, 6)]
    protected int MaxPointsPerRound { get; set; }
    
    [ScriptSetting(Name = "S_NbOfWinners")]
    [Description("Number of winners")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, 6)]
    protected int NbOfWinners { get; set; }
    
    [ScriptSetting(Name = "S_NeutralEmblemUrl")]
    [Description("Url of the neutral emblem url to use by default")]
    [DefaultScriptSettingValue("")]
    protected string NeutralEmblemUrl { get; set; }
    
    [ScriptSetting(Name = "S_PauseBeforeRoundNb")]
    [Description("""
    Round with a pause before its start, used in TMGL with the value 4.
    Set 0 to disable the feature. Linked to S_PauseDuration
    """)]
    [DefaultScriptSettingValue(0)]
    protected int PauseBeforeRoundNb { get; set; }
    
    [ScriptSetting(Name = "S_PauseDuration")]
    [Description("""
    Pause time in seconds.
    Set 0 to disable the feature. Linked to S_PauseBeforeRoundNb
    """)]
    [DefaultScriptSettingValue(360)]
    protected int PauseDuration { get; set; }
    
    [ScriptSetting(Name = "S_PointsGap")]
    [Description("The number of points lead a team must have to win the map.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, 6)]
    protected int PointsGap { get; set; }
    
    [ScriptSetting(Name = "S_PointsLimit")]
    [Description("""
    Limit number of points. 
    0 = unlimited for Champion & Rounds
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, 100)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, 5)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, 50)]
    protected int PointsLimit { get; set; }
    
    [ScriptSetting(Name = "S_PointsRepartition")]
    [Description("""
    Point repartition from first to last
    In Teams game mode, this depend of the setting S_UseCustomPointsRepartition
    In Knockout game mode, this setting is useless
    empty = 10,6,4,3,2,1
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, "")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, "")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, "")]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, "")]
    protected string PointsRepartition { get; set; }
    
    [ScriptSetting(Name = "S_RespawnBehaviour")]
    [Description("""
    0: Use the default behavior of the gamemode
    1: Use the normal behavior like in TimeAttack
    2: do nothing
    3: give up before first CP
    4: always give up
    5: never give up
    """)]
    [DefaultScriptSettingValue(0)]
    protected int RespawnBehaviour { get; set; }
    
    [ScriptSetting(Name = "S_RoundsLimit")]
    [Description("Number of rounds to play before finding a winner")]
    [DefaultScriptSettingValue(6)]
    protected int RoundsLimit { get; set; }
    
    [ScriptSetting(Name = "S_RoundsPerMap")]
    [Description("""
    Number of round to play on one map before going to the next one 
    -1 or 0 = unlimited
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Cup, 5)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, -1)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, -1)]
    protected int RoundsPerMap { get; set; }
    
    [ScriptSetting(Name = "S_RoundsWithAPhaseChange")]
    [Description("""
    Rounds with a Phase change (Openning, Semi-Final, Final)
    It's possible to skip a phase with multiple occurences of a round number, exemple "3,3"
    """)]
    [DefaultScriptSettingValue("3,5")]
    protected string RoundsWithAPhaseChange { get; set; }
    
    [ScriptSetting(Name = "S_RoundsWithoutElimination")]
    [Description("Rounds without elimination (like a Warmup, but just for the first map)")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Knockout, 1)]
    protected int RoundsWithoutElimination { get; set; }
    
    [ScriptSetting(Name = "S_ScriptEnvironment")]
    [Description("Use “development” to make script more verbose")]
    [DefaultScriptSettingValue("production")]
    protected string ScriptEnvironment { get; set; }
    
    [ScriptSetting(Name = "S_SeasonIds")]
    [Description("""
    JSON Formatted list of maps and their Season ID like 
    ["MapUid" => "SeasonId"]
    """)]
    [DefaultScriptSettingValue("")]
    protected string SeasonIds { get; set; }
    
    [ScriptSetting(Name = "S_SynchronizePlayersAtMapStart")]
    [Description("""
    Synchronize players at the launch of the map, to ensure that no one starts late.
    Can delay the start by a few seconds
    """)]
    [DefaultScriptSettingValue(true)]
    protected bool SynchronizePlayersAtMapStart { get; set; }
    
    [ScriptSetting(Name = "S_SynchronizePlayersAtRoundStart")]
    [Description("""
    Synchronize players at the launch of the round, to ensure that no one starts late.
    Can delay the start by a few seconds.
    """)]
    [DefaultScriptSettingValue(true)]
    protected bool SynchronizePlayersAtRoundStart { get; set; }
    
    [ScriptSetting(Name = "S_TimeLimit")]
    [Description("""
    Time limit before going to the next map
    0 or -1 for unlimited time
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Laps, 0)]
    [DefaultScriptSettingValue(DefaultModeScriptName.TimeAttack, 300)]
    protected int TimeLimit { get; set; }
    
    [ScriptSetting(Name = "S_TimeOutPlayersNumber")]
    [Description("Players crossing finish line before starting finish timeout. Linked to S_FinishTimeout")]
    [DefaultScriptSettingValue( 0)]
    protected int TimeOutPlayersNumber { get; set; }
    
    [ScriptSetting(Name = "S_TrustClientSimu")]
    [Description("""
    No clear official informations about this setting.
    It would seem that this tells the server to trust or not trust the network data sent by the client.
    """)]
    [DefaultScriptSettingValue(true)]
    protected bool TrustClientSimu { get; set; }
    
    [ScriptSetting(Name = "S_UseAlternateRules")]
    [Description("""
    False: Give 1 point to the all first players of a team
    True:  follow this equation: Number of players ( or S_MaxPointsPerRound if lower ) - Player position - 1 
    This setting is excluded by S_UseCustomPointsRepartition
    """)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, true)]
    protected bool UseAlternateRules { get; set; }
    
    [ScriptSetting(Name = "S_UseClublinks")]
    [Description("Use the players clublinks, or otherwise use the default teams.")]
    [DefaultScriptSettingValue(false)]
    protected bool UseClublinks { get; set; }
    
    [ScriptSetting(Name = "S_UseClublinksSponsors")]
    [Description("Display the clublinks sponsors")]
    [DefaultScriptSettingValue(false)]
    protected bool UseClublinksSponsors { get; set; }
    
    [ScriptSetting(Name = "S_UseCrudeExtrapolation")]
    [Description("""
    The car position of other players is extrapolated less precisely, disabling it has a big impact on performance.
    This replaces the "S_UseDelayedVisuals" option by removing the delay with ghosts for the modes that need it (There may be a delay in TimeAttack).
    """)]
    [DefaultScriptSettingValue(true)]
    protected bool UseCrudeExtrapolation { get; set; }
    
    [ScriptSetting(Name = "S_UseCustomPointsRepartition")]
    [Description("Use S_PointsRepartition instead of the rules defined by S_UseAlternateRules")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, false)]
    protected bool UseCustomPointsRepartition { get; set; }
    
    [ScriptSetting(Name = "S_UseTieBreak")]
    [Description("Continue to play the map until the tie is broken.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.Teams, true)]
    [DefaultScriptSettingValue(DefaultModeScriptName.Rounds, true)]
    protected bool UseTieBreak { get; set; }
    
    [ScriptSetting(Name = "S_WarmUpDuration")]
    [Description("""
    Time in seconds of the Warm Up.
    0 = Time based on the Author medal ( 5 seconds + Author Time on 1 lap + ( Author Time on 1 lap / 6 ) )
    -1 = Only one try like a round
    """)]
    [DefaultScriptSettingValue(0)]
    protected int WarmUpDuration { get; set; }
    
    [ScriptSetting(Name = "S_WarmUpNb")]
    [Description("Number of Warm Up")]
    [DefaultScriptSettingValue(0)]
    protected int WarmUpNb { get; set; }
    
    [ScriptSetting(Name = "S_WarmUpTimeout")]
    [Description("""
    Time to finish the race in seconds after the winners, equivalent of S_FinishTimeout but for Warm Up.
    Only when S_WarmUpDuration is set to -1.
    -1 : Time based on the Author medal ( 5 seconds + Author time / 6 )
    """)]
    [DefaultScriptSettingValue(-1)]
    protected int WarmUpTimeout { get; set; }
    
    [ScriptSetting(Name = "S_DisableGoToMap")]
    [Description("Disable the goto map vote option.")]
    [DefaultScriptSettingValue(false)]
    protected bool DisableGoToMap { get; set; }
    
    [ScriptSetting(Name = "S_MapPointsLimit")]
    [Description("Map points limit.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, 10)]
    protected int MapPointsLimit { get; set; }
    
    [ScriptSetting(Name = "S_MatchPointsLimit")]
    [Description("Match points limit, can be set from 1 to 4.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, 4)]
    protected int MatchPointsLimit { get; set; }
    
    [ScriptSetting(Name = "S_MatchInfo")]
    [Description("Match info displayed in the UI. Shows a message in the waiting screen before the match.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, "")]
    protected string MatchInfo { get; set; }
    
    [ScriptSetting(Name = "S_TeamsUrl")]
    [Description("A url to a HTTP page which returns JSON formatted info about the teams.")]
    [DefaultScriptSettingValue(DefaultModeScriptName.TmwtTeams, "")]
    protected string TeamsUrl { get; set; }
    
    [ScriptSetting(Name = "S_ForceRoadSpectatorsNb")]
    [Description("")]
    [DefaultScriptSettingValue("")]
    protected string ForceRoadSpectatorsNb { get; set; }
    
    [ScriptSetting(Name = "S_EnablePreMatch")]
    [Description("")]
    [DefaultScriptSettingValue(false)]
    protected bool EnablePreMatch { get; set; }
    
    [ScriptSetting(Name = "S_EnableDossardColor")]
    [Description("")]
    [DefaultScriptSettingValue(false)]
    protected bool EnableDossardColor { get; set; }
}
