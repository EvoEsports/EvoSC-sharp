namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;

public interface ITeamScore
{
    public int TeamId { get; }
    public string TeamName { get; }
    public int RoundPoints { get; }
    public int MapPoints { get; }
    public int MatchPoints { get; }
}
