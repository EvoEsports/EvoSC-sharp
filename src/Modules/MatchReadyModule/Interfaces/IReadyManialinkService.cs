using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchReadyModule.Interfaces;

public interface IReadyManialinkService
{
    /// <summary>
    /// Send the ready widget to all players.
    /// </summary>
    /// <returns></returns>
    public Task SendWidgetAsync();
    
    /// <summary>
    /// Send the ready widget to one player, if possible.
    /// </summary>
    /// <param name="player">The player to attempt sending the ready widget to.</param>
    /// <returns></returns>
    public Task SendWidgetAsync(IPlayer player);

    /// <summary>
    /// Update the widget based on the current state.
    /// </summary>
    /// <returns></returns>
    public Task UpdateWidgetAsync();

    /// <summary>
    /// Hide the widget for all players.
    /// </summary>
    /// <returns></returns>
    public Task HideWidgetAsync();
}
