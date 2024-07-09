using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkOperations
{
    /// <summary>
    /// Render a template and send it to all players.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Any kind of data the template uses.</param>
    /// <returns></returns>
    public Task SendManialinkAsync(string name, IDictionary<string, object?> data);
    
    /// <summary>
    /// Render a template and send it to all players.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Any kind of data the template uses.</param>
    /// <returns></returns>
    public Task SendManialinkAsync(string name, dynamic data);
    
    /// <summary>
    /// Render a template and send it to all players without data.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <returns></returns>
    
    public Task SendManialinkAsync(string name);
    /// <summary>
    /// Render a template and send it to a specific player.
    /// </summary>
    /// <param name="player">The player to send to.</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses.</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IPlayer player, string name, IDictionary<string, object?> data);

    /// <summary>
    /// Render a template and send it to a specific player.
    /// </summary>
    /// <param name="player">The player to send to.</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IPlayer player, string name, dynamic data);

    /// <summary>
    /// Render a template and send it to a specific player.
    /// </summary>
    /// <param name="playerLogin">The login to send to.</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <returns></returns>
    public Task SendManialinkAsync(string playerLogin, string name, dynamic data);

    /// <summary>
    /// Render a template and send it to a specific player without data.
    /// </summary>
    /// <param name="player">The player to send to.</param>
    /// <param name="name">The name of the template.</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IPlayer player, string name) => SendManialinkAsync(player, name, new { });
    
    /// <summary>
    /// Render a template and send it to a set of players.
    /// </summary>
    /// <param name="players">The players to send to</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, IDictionary<string, object?> data);
    
    /// <summary>
    /// Render a template and send it to a set of players.
    /// </summary>
    /// <param name="players">The players to send to</param>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IEnumerable<IPlayer> players, string name, dynamic data);
    
    /// <summary>
    /// Render a template and send it to a set of players without data.
    /// </summary>
    /// <param name="players">The players to send to</param>
    /// <param name="name">The name of the template.</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IEnumerable<IPlayer> players, string name) =>
        SendManialinkAsync(players, name, new { });

    /// <summary>
    /// Hide a manialink from all players.
    /// </summary>
    /// <param name="name">Name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideManialinkAsync(string name);
    
    /// <summary>
    /// Hide a manialink from a player.
    /// </summary>
    /// <param name="player">The player to hide the manialink from.</param>
    /// <param name="name">Name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideManialinkAsync(IPlayer player, string name);
    
    /// <summary>
    /// Hide a manialink from a player.
    /// </summary>
    /// <param name="playerLogin">The player to hide the manialink from.</param>
    /// <param name="name">Name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideManialinkAsync(string playerLogin, string name);
    
    /// <summary>
    /// Hide a manialink from a set of players.
    /// </summary>
    /// <param name="players">The players to hide the manialink from.</param>
    /// <param name="name">Name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideManialinkAsync(IEnumerable<IPlayer> players, string name);
}
