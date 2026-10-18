using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "MatchControl:MatchStarted")]
    MatchStarted,
    
    [Identifier(Name = "MatchControl:MatchEnded")]
    MatchEnded,
    
    [Identifier(Name = "MatchControl:EndRound")]
    EndRound,
    
    [Identifier(Name = "MatchControl:RestartMatch")]
    RestartMatch,
    
    [Identifier(Name = "MatchControl:SkipMap")]
    SkipMap,
    
    [Identifier(Name = "LiveMode:LoadMode")]
    LoadMode,
    
    [Identifier(Name = "MatchManager:MatchSettingsLoaded")]
    MatchSettingsLoaded,
    
    [Identifier(Name = "MatchManager:ScriptSettingsSet")]
    ScriptSettingsModified,
    
    [Identifier(Name = "MatchManager:TeamPointsSet")]
    TeamPointsSet,
    
    [Identifier(Name = "MatchManager:TeamRoundPointsSet")]
    TeamRoundPointsSet,
    
    [Identifier(Name = "MatchManager:TeamRoundMapSet")]
    TeamMapPointsSet,
    
    [Identifier(Name = "MatchManager:TeamRoundMatchSet")]
    TeamMatchPointsSet,
    
    [Identifier(Name = "MatchManager:MatchPaused")]
    MatchPaused,
    
    [Identifier(Name = "MatchManager:MatchUnpaused")]
    MatchUnpaused,
}
