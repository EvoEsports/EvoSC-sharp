using EvoSC.Common.Database.Repository.Maps;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Repository;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Official.PlayerRecords.Database.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerRecordsService : IPlayerRecordsService
{
    private readonly IPlayerRecordsRepository _recordsRepo;
    private readonly MapRepository _mapRepo;
    
    public PlayerRecordsService(IPlayerRecordsRepository recordsRepo, MapRepository mapRepo)
    {
        _recordsRepo = recordsRepo;
        _mapRepo = mapRepo;
    }

    public async Task AddRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints)
    {
        var record = await _recordsRepo.GetRecordAsync(player, map);

        if (record != null)
        {
            record.Score = score;
            record.Checkpoints = string.Join(',', checkpoints);
            record.UpdatedAt = DateTime.UtcNow;
            await _recordsRepo.UpdateRecordAsync(record);
            return;
        }
        
        record = new DbPlayerRecord
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

        await _recordsRepo.InsertRecordAsync(record);
    }
}
