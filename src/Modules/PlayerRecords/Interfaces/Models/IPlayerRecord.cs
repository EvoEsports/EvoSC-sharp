using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

public interface IPlayerRecord : IComparable<IPlayerRecord>
{
    public long Id { get; }
    public IPlayer Player { get; }
    public IMap Map { get; }
    public int Score { get; }
    public PlayerRecordType RecordType { get; }
    public string Checkpoints { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
}
