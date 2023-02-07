using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchManagerModule.Interfaces;

public interface IMatchManagerHandlerService
{
    /// <summary>
    /// Handle a player setting the current game mode of the server.
    /// </summary>
    /// <param name="mode">The game mode to set.</param>
    /// <param name="actor">The player that did the action.</param>
    /// <returns></returns>
    public Task SetModeAsync(string mode, IPlayer actor);
    
    /// <summary>
    /// Handle a player loading a match settings file.
    /// </summary>
    /// <param name="name">The name of the match settings to load.</param>
    /// <param name="actor">The player that did the action.</param>
    /// <returns></returns>
    public Task LoadMatchSettingsAsync(string name, IPlayer actor);

    /// <summary>
    /// Set the value of a single script setting.
    /// </summary>
    /// <param name="name">The name of the script setting.</param>
    /// <param name="value">Value of the script setting.</param>
    /// <param name="actor">The player that set the script setting.</param>
    /// <returns></returns>
    Task SetScriptSettingAsync(string name, string value, IPlayer actor);
}
