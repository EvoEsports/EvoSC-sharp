using EvoSC.Modules.Official.MatchManagerModule.Interfaces.Models;
using EvoSC.Modules.Official.MatchManagerModule.Models.Database;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface IMatchRecordRepository
{
    public Task<DbMatchRecord> InsertStateAsync(IMatchState state);
}
