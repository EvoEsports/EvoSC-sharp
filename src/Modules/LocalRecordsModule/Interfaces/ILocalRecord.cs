using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces;

public interface ILocalRecord
{
    public long Id { get; }
    public int Position { get; }
    public IMap Map { get; }
    public IPlayerRecord Record { get; }
}
