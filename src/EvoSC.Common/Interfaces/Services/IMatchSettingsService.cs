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

    public Task LoadMatchSettingsAsync(string name);
    public IEnumerable<string> GetMatchSettingsAsync();
}
