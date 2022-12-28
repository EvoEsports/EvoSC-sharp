using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces;

public interface IPlayerRecordsRepository
{
    public Task<DbPlayerRecord?> GetRecordAsync(IPlayer player, IMap map);
    public Task UpdateRecordAsync(DbPlayerRecord record);
    public Task InsertRecordAsync(DbPlayerRecord record);
}
