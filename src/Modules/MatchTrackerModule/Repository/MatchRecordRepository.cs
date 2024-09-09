using System.Text.Json;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces;
using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models.Database;
using EvoSC.Modules.Official.MatchTrackerModule.Util;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.MatchTrackerModule.Repository;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class MatchRecordRepository(IDbConnectionFactory dbConnFactory, ILogger<MatchRecordRepository> logger)
    : DbRepository(dbConnFactory), IMatchRecordRepository
{
    public async Task<DbMatchRecord> InsertStateAsync(IMatchState state)
    {
        var report = JsonSerializer.Serialize(state.CastToSuperClass());
        var record = new DbMatchRecord
        {
            TimelineId = state.TimelineId,
            Report = report,
            Timestamp = state.Timestamp,
            Status = state.Status
        };

        try
        {
            var id = await Database.InsertWithIdentityAsync(record);
            record.Id = Convert.ToInt64(id);

            return record;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to add match state record");
            throw;
        }
    }

    public Task<DbMatchRecord[]> GetRecordsAsync() => Table<DbMatchRecord>().ToArrayAsync();
}
