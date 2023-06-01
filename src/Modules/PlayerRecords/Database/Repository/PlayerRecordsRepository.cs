using EvoSC.Common.Database;
using EvoSC.Common.Database.Models.Maps;
using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.PlayerRecords.Database.Repository;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerRecordsRepository : DbRepository, IPlayerRecordsRepository
{
    private readonly ILogger<PlayerRecordsRepository> _logger;
    
    public PlayerRecordsRepository(DbConnectionFactory dbConnFactory, ILogger<PlayerRecordsRepository> logger) : base(dbConnFactory)
    {
        _logger = logger;
    }

    public Task<DbPlayerRecord?> GetRecordAsync(IPlayer player, IMap map) =>
        Table<DbPlayerRecord>()
            .LoadWith(r => r.DbPlayer)
            .LoadWith(r => r.DbMap)
            .SingleOrDefaultAsync(r => r.PlayerId == player.Id && r.MapId == map.Id);

    public Task UpdateRecordAsync(DbPlayerRecord record) => Database.UpdateAsync(record);

    public async Task<DbPlayerRecord> InsertRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints)
    {
        var record = new DbPlayerRecord
        {
            PlayerId = player.Id,
            MapId = map.Id,
            Score = score,
            RecordType = PlayerRecordType.Time,
            Checkpoints = string.Join(',', checkpoints),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DbPlayer = new DbPlayer(player),
            DbMap = new DbMap(map)
        };

        try
        {
            await Database.InsertAsync(record);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add record");
            throw;
        }

        return record;
    }
}
