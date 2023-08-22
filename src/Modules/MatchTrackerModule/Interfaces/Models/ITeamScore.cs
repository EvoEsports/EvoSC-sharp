namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

public interface ITeamScore
{
    public int TeamId { get; }
    public string TeamName { get; }
    public int RoundPoints { get; }
    public int MapPoints { get; }
    public int MatchPoints { get; }
}
