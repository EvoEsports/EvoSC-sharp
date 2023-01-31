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
}
