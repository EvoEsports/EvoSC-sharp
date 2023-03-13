using System.Reflection;
using EvoSC.Common.Controllers;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Manialinks;

public class ManialinkController : EvoScController<ManialinkInteractionContext>
{
    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink) => Context.ManialinkManager.SendManialinkAsync(maniaLink, new object());
    
    /// <summary>
    /// Display a manialink to all players.
    /// </summary>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(string maniaLink, dynamic data) => Context.ManialinkManager.SendManialinkAsync(maniaLink, data);

    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, new object(), player);
    
    /// <summary>
    /// Display a manialink to a player.
    /// </summary>
    /// <param name="player">The player to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IOnlinePlayer player, string maniaLink, dynamic data) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, data, player);
    
    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, new object(), players);

    /// <summary>
    /// Display a manialink to a set of players.
    /// </summary>
    /// <param name="players">The players to show the manialink to.</param>
    /// <param name="maniaLink">The manialink to show.</param>
    /// <param name="data">Data to send to use with manialink template.</param>
    /// <returns></returns>
    public Task ShowAsync(IEnumerable<IOnlinePlayer> players, string maniaLink, dynamic data) =>
        Context.ManialinkManager.SendManialinkAsync(maniaLink, data, players);

    public Task HideAsync(string maniaLink) => Context.ManialinkManager.HideManialinkAsync(maniaLink);

    public Task HideAsync(IOnlinePlayer player, string maniaLink) =>
        Context.ManialinkManager.HideManialinkAsync(maniaLink, player);

    public Task HideAsync(IEnumerable<IOnlinePlayer> players, string maniaLink) =>
        Context.ManialinkManager.HideManialinkAsync(maniaLink, players);
}
