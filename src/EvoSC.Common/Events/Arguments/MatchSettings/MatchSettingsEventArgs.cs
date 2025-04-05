using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Events.Arguments.MatchSettings;

public class MatchSettingsEventArgs : EventArgs
{
    public IMatchSettings MatchSettings { get; init; }
}
