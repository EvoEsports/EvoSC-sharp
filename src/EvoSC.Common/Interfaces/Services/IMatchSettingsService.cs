using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util.MatchSettings.Builders;
using GbxRemoteNet.XmlRpc.ExtraTypes;

namespace EvoSC.Common.Interfaces.Services;

public interface IMatchSettingsService
{
    /// <summary>
    /// Set the current ModeScript settings.
    /// </summary>
    /// <param name="settingsAction">Fluent action for modifying script settings.</param>
    /// <returns></returns>
    public Task SetScriptSettingsAsync(Action<Dictionary<string, object>> settingsAction);

    /// <summary>
    /// Set the current ModeScript settings from a match settings info object.
    /// </summary>
    /// <param name="matchSettings">The match settings containing the mode settings to set.</param>
    /// <returns></returns>
    public Task SetScriptSettingsAsync(IMatchSettings matchSettings);
    
    /// <summary>
    /// Get the current ModeScript settings.
    /// </summary>
    /// <returns></returns>
    public Task<Dictionary<string, object>?> GetCurrentScriptSettingsAsync();

    /// <summary>
    /// Load a MatchSettings file.
    /// </summary>
    /// <param name="name">The name of the MatchSettings to load.</param>
    /// <returns></returns>
    public Task LoadMatchSettingsAsync(string name);

    /// <summary>
    /// Create a new match settings file and return the info object for it.
    /// </summary>
    /// <param name="name">The name of the file. Do not include extension. Directory paths allowed.</param>
    /// <param name="matchSettings">Match settings builder for creation.</param>
    /// <returns></returns>
    public Task<IMatchSettings> CreateMatchSettingsAsync(string name, Action<MatchSettingsBuilder> matchSettings);

    public Task<IMatchSettings> GetMatchSettingsAsync(string name);
}
