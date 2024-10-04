using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class CheckpointData
{
    public IOnlinePlayer Player { get; init; }
    public int CheckpointId { get; init; }
    public IRaceTime Time { get; init; }
    public bool IsFinish { get; init; }
}
