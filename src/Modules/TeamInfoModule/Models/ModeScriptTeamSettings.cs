namespace EvoSC.Modules.Official.TeamInfoModule.Models;

public class ModeScriptTeamSettings
{
    /// <summary>
    /// The amount of points a team has to reach to win the match.
    /// </summary>
    public int PointsLimit { init; get; } = 5;

    /// <summary>
    /// Finish timeout.
    /// </summary>
    public int FinishTimeout { init; get; } = -1;
    
    /// <summary>
    /// The maximum number of points attributed to the first player to cross the finish line.
    /// </summary>
    public int MaxPointsPerRound { init; get; } = 6;
    
    /// <summary>
    /// Defines how much points difference there has to be for a team to win (tennis mode).
    /// </summary>
    public int PointsGap { init; get; } = 1;
    
    /// <summary>
    /// Use a custom points repartition.
    /// If false points repartition is [10, 6, 4, 3, 2, 1].
    /// </summary>
    public bool UseCustomPointsRepartition { init; get; } = false;
    
    /// <summary>
    /// At the end of the round both teams win their players points.
    /// </summary>
    public bool CumulatePoints { init; get; } = false;
    
    /// <summary>
    /// Number of rounds to play on one map, before going to the next one.
    /// </summary>
    public int RoundsPerMap { init; get; } = -1;
    
    /// <summary>
    /// Number of maps to play before the match ends.
    /// </summary>
    public int MapsPerMatch { init; get; } = -1;

    /// <summary>
    /// Continue to play the map until the tie is broken.
    /// </summary>
    public bool UseTieBreak { init; get; } = true;
    
    /// <summary>
    /// Number of warmups.
    /// </summary>
    public int WarmUpNb { init; get; } = 0;
    
    /// <summary>
    /// Duration of one warmup in seconds.
    /// </summary>
    public int WarmUpDuration { init; get; } = 0;
    
    /// <summary>
    /// Use alternate rules.
    /// </summary>
    public bool UseAlternateRules { init; get; } = true;
    
    /// <summary>
    /// Neutral emblem URL.
    /// </summary>
    public string NeutralEmblemUrl { init; get; } = "";
}
