using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

public enum MatchSettingsEvent
{
    [Identifier(Name = "MatchManager.MatchSettings.Loaded")]
    MatchSettingsLoaded,
    
    [Identifier(Name = "MatchManager.MatchSettings.LiveModeSet")]
    LiveModeSet
}
