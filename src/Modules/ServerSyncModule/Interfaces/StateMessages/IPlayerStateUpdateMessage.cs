using System.Text.Json.Serialization;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;

namespace EvoSC.Modules.EvoEsports.ServerSyncModule.Interfaces.StateMessages;

public interface IPlayerStateUpdateMessage : IPlayerStateMessage
{
    /// <summary>
    /// Scores of the player.
    /// </summary>
    public IEnumerable<long> Scores { get; set; }
    
    /// <summary>
    /// Scoreboard of the player.
    /// </summary>
    public long Position { get; set; }
    
    /// <summary>
    /// Checkpoint scores/times of the player.
    /// </summary>
    public IEnumerable<long> CheckpointScores { get; set; }
    
    /// <summary>
    /// Times driven by the player.
    /// </summary>
    public IEnumerable<long> Times { get; set; }

    /// <summary>
    /// The player's current score.
    /// </summary>
    [JsonIgnore]
    public long Score => Scores.First();
    
    /// <summary>
    /// The player's driven time.
    /// </summary>
    [JsonIgnore]
    public IRaceTime Time => RaceTime.FromMilliseconds((int)Times.First());

    /// <summary>
    /// Checkpoint times driven by the player.
    /// </summary>
    [JsonIgnore]
    public IEnumerable<IRaceTime> Checkpoints => CheckpointScores.Select(cs => RaceTime.FromMilliseconds((int)cs));
}
