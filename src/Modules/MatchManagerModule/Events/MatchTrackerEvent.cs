using EvoSC.Common.Util.EnumIdentifier;

namespace EvoSC.Modules.Official.MatchManagerModule.Events;

public enum MatchTrackerEvent
{
    [Identifier(Name = "MatchManager.MatchTracker.StateTracked")]
    StateTracked,
}
