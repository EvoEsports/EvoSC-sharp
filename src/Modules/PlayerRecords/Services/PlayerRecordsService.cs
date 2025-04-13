using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.PlayerRecords.Events;
using EvoSC.Modules.Official.PlayerRecords.Interfaces;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class PlayerRecordsService(IPlayerRecordsRepository recordsRepo)
    : IPlayerRecordsService
{

    public async Task<IPlayerRecord?> GetPlayerRecordAsync(IPlayer player, IMap map)
    {
        return await recordsRepo.GetRecordAsync(player, map);
    }

    public async Task<(IPlayerRecord, RecordUpdateStatus)> SetPbRecordAsync(IPlayer player, IMap map, int score,
        IEnumerable<int> checkpoints)
    {
        var record = await recordsRepo.GetRecordAsync(player, map);

        if (record == null)
        {
            record = await recordsRepo.InsertRecordAsync(player, map, score, checkpoints);
            return (record, RecordUpdateStatus.New);
        }

        if (score >= record.Score)
        {
            return (record, score > record.Score ? RecordUpdateStatus.NotUpdated : RecordUpdateStatus.Equal);
        }
        
        record = await recordsRepo.InsertRecordAsync(player, map, score, checkpoints);

        return (record, RecordUpdateStatus.Updated);
    }
}
