using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

public enum AuditEvents
{
    [Identifier(Name = "FlowControl:EndRound")]
    EndRound,
    
    [Identifier(Name = "FlowControl:RestartMatch")]
    RestartMatch,
    
    [Identifier(Name = "FlowControl:SkipMap")]
    SkipMap,
    
    [Identifier(Name = "LiveMode:LoadMode")]
    LoadMode,
    
    [Identifier(Name = "MatchManager:MatchSettingsLoaded")]
    MatchSettingsLoaded,
    
    [Identifier(Name = "MatchManager:ScriptSettingsSet")]
    ScriptSettingsModified
}
