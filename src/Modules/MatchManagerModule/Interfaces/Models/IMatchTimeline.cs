namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

public interface IMatchTimeline
{
    public List<IMatchState> States { get; }
}
