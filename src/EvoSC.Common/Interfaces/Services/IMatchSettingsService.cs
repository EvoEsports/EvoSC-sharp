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
    /// Get the current ModeScript settings.
    /// </summary>
    /// <returns></returns>
    public Task<Dictionary<string, object>?> GetScriptSettingsAsync();

    /// <summary>
    /// Load a MatchSettings file.
    /// </summary>
    /// <param name="name">The name of the MatchSettings to load.</param>
    /// <returns></returns>
    public Task LoadMatchSettingsAsync(string name);
}
