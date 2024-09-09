using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchTrackerModule.Models.Database;

namespace EvoSC.Modules.Official.MatchTrackerModule.Interfaces;

public interface IMatchRecordRepository
{
    public Task<DbMatchRecord> InsertStateAsync(IMatchState state);
    public Task<DbMatchRecord[]> GetRecordsAsync();
}
