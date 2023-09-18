namespace EvoSC.Modules.Official.LiveRankingModule.Models;

public class MatchInfo
{
    /// <summary>
    /// The name of the map.
    /// </summary>
    public string MapName { get; set; }
    /// <summary>
    /// The number of the round.
    /// </summary>
    public int NumRound { get; set; }
    /// <summary>
    /// The number of the map.
    /// </summary>
    public int NumTrack { get; set; }
    /// <summary>
    /// Name of world record holder.
    /// </summary>
    public string WrHolderName { get; set; }
    /// <summary>
    /// Current world record time.
    /// </summary>
    public string WrTime { get; set; }
}
