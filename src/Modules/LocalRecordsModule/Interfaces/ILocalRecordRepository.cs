using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.LocalRecordsModule.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces;

public interface ILocalRecordRepository
{
    public Task<IEnumerable<DbLocalRecord>> GetLocalRecordsOfMapByIdAsync(long mapId);
    public Task<DbLocalRecord> AddOrUpdateRecordAsync(IMap map, IPlayerRecord record);
    public Task RecalculatePositionsOfMapAsync(IMap map);
    public Task<IEnumerable<DbLocalRecord>> GetRecordsByPlayerAsync(IPlayer player);
    public Task<DbLocalRecord?> GetRecordOfPlayerInMapAsync(IPlayer player, IMap map);
}
