using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces;

public interface ILocalRecord
{
    /// <summary>
    /// The ID of the record.
    /// </summary>
    public long Id { get; }
    
    /// <summary>
    /// The position of the record in the leaderboard.
    /// </summary>
    public int Position { get; }
    
    /// <summary>
    /// The map the record is set on.
    /// </summary>
    public IMap Map { get; }
    
    /// <summary>
    /// The player record.
    /// </summary>
    public IPlayerRecord Record { get; }
}
