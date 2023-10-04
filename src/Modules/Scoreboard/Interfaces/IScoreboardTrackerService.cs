namespace EvoSC.Modules.Official.Scoreboard.Interfaces;

public interface IScoreboardTrackerService
{
    public int RoundsPerMap { get; set; }
    
    public int PointsLimit { get; set; }
    
    public int CurrentRound { get; set; }
    
    public int MaxPlayers { get; set; }
}
