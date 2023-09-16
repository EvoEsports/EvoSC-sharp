using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.WorldRecordModule.Models;

namespace EvoSC.Modules.Official.WorldRecordModule.Interfaces;

public interface IWorldRecordService
{
    public Task FetchRecord(IMap map);
    public Task OverwriteRecord(WorldRecord newRecord);
    public Task<WorldRecord?> GetRecord();
}
