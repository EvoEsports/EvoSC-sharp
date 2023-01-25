using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

public interface IPlayerRecord
{
    public IPlayer Player { get; }
    public IMap Map { get; }
    public int Score { get; }
    public PlayerRecordType RecordType { get; }
    public string Checkpoints { get; }
}
