using EvoSC.Common.Util;

namespace EvoSC.Modules.Official.WorldRecordModule.Models;

public class WorldRecord
{
    public required string Name { get; init; }
    public required int Time { get; init; }
    public required string Source { get; init; }

    public string FormattedTime()
    {
        return FormattingUtils.FormatTime(Time);
    }
}
