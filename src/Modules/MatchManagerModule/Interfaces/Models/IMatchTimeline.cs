namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

public interface IMatchTimeline
{
    public Guid TimelineId { get; }
    public List<IMatchState> States { get; }
}
