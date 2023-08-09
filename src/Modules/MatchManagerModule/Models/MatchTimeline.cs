using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Models;

public class MatchTimeline : IMatchTimeline
{
    public List<IMatchState> States { get; init; } = new();
}
