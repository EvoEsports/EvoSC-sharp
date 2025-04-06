using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Interfaces.Services;

public interface IMatchSettingsTrackerService
{
    /// <summary>
    /// The current matchsettings that is loaded on the server.
    /// </summary>
    public IMatchSettings CurrentMatchSettings { get; }
    
    /// <summary>
    /// Set the current matchsettings to the default one defined
    /// in the config.
    /// </summary>
    /// <returns></returns>
    public Task SetDefaultMatchSettingsAsync();
}
