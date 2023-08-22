namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

public interface IMatchTimeline
{
    public Guid TimelineId { get; }
    public List<IMatchState> States { get; }
}
