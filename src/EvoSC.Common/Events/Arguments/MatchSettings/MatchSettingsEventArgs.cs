using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Events.Arguments.MatchSettings;

public class MatchSettingsEventArgs : EventArgs
{
    /// <summary>
    /// The changed matchsettings.
    /// </summary>
    public IMatchSettings MatchSettings { get; init; }
}
