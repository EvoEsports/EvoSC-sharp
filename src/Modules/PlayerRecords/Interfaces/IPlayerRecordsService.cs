using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Official.PlayerRecords.Interfaces.Models;

namespace EvoSC.Modules.Official.PlayerRecords.Interfaces;

public interface IPlayerRecordsService
{
    public Task AddRecordAsync(IPlayer player, IMap map, int score, IEnumerable<int> checkpoints);
}
