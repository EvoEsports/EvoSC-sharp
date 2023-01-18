using EvoSC.Common.Database.Extensions;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;
using Microsoft.Extensions.Logging;
using RepoDb;

namespace EvoSC.Modules.Official.PlayerRecords.Database.Repository;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerRecordsRepository : EvoScDbRepository, IPlayerRecordsRepository
{
    private readonly ILogger<PlayerRecordsRepository> _logger;
    
    public PlayerRecordsRepository(IDbConnectionFactory connectionFactory, ILogger<PlayerRecordsRepository> logger) : base(connectionFactory)
    {
        _logger = logger;
    }

    public async Task<DbPlayerRecord?> GetRecordAsync(IPlayer player, IMap map)
    {
        try
        {
            var (sql, values) = Query("PlayerRecords")
                .Where("PlayerId", player.Id)
                .Where("MapId", map.Id)
                .Compile();
            
            var records = await Database.ExecuteQueryAsync<DbPlayerRecord>(sql, values);

            var record = records?.FirstOrDefault();

            if (record == null)
            {
                return null;
            }

            record.Player = player;
            record.Map = map;

            return record;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to get record: {Msg}", ex.Message);
        }

        return null;
    }

    public Task UpdateRecordAsync(DbPlayerRecord record)
    {
        try
        {
            return Database.UpdateAsync(record);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to update record: {Msg}", ex.Message);
        }

        return Task.CompletedTask;
    }

    public async Task<DbPlayerRecord> InsertRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints)
    {
        var record = new DbPlayerRecord
        {
            Player = player,
            PlayerId = player.Id,
            Map = map,
            MapId = map.Id,
            Score = score,
            RecordType = PlayerRecordType.Time,
            Checkpoints = string.Join(',', checkpoints),
            CreatedAt = default,
            UpdatedAt = default
        };

        try
        {
            var id = await Database.InsertAsync(record);
            record.Id = (long)(id ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to insert record: {Msg}", ex.Message);
        }
        
        return record;
    }
}
