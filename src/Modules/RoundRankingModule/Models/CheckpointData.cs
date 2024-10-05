using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.RoundRankingModule.Models;

public class CheckpointData
{
    public required IOnlinePlayer Player { get; init; }
    public required int CheckpointId { get; init; }
    public required IRaceTime Time { get; set; }
    public IRaceTime? TimeDifference { get; set; }
    public required bool IsFinish { get; init; }
    public required bool IsDNF { get; init; }
    public int GainedPoints { get; set; } = 0;
    public string? AccentColor { get; set; }

    public IRaceTime GetTimeDifferenceAbsolute(CheckpointData checkpointData)
    {
        return RaceTime.FromMilliseconds(int.Abs(this.Time.TotalMilliseconds - checkpointData.Time.TotalMilliseconds));
    }
}
