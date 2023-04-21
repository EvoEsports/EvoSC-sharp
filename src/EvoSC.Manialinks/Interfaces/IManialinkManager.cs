using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

/// <summary>
/// Manialink template & ManiaScript registry, and can render, display and hide Manialink to players.
/// </summary>
public interface IManialinkManager
{
    /// <summary>
    /// Add all the default templates from EvoSC.
    /// </summary>
    /// <returns></returns>
    public Task AddDefaultTemplatesAsync();

    /// <summary>
    /// Add and immediately pre-process a template.
    /// </summary>
    /// <param name="template">The template to add.</param>
    /// <returns></returns>
    public Task AddAndPreProcessTemplateAsync(IManialinkTemplateInfo template);

    /// <summary>
    /// Register a template, does not pre-process the template.
    /// </summary>
    /// <param name="template">The template to add.</param>
    public void AddTemplate(IManialinkTemplateInfo template);
    
    /// <summary>
    /// Register a new ManiaScript resource.
    /// </summary>
    /// <param name="maniaScript">The ManiaScript to add</param>
    public void AddManiaScript(IManiaScriptInfo maniaScript);
    
    /// <summary>
    /// Remove a template so that it can no longer be used.
    /// </summary>
    /// <param name="name">The name of the template</param>
    public void RemoveTemplate(string name);
    
    /// <summary>
    /// Remove a ManiaScript resource.
    /// </summary>
    /// <param name="name">The name of the ManiaScript resource</param>
    public void RemoveManiaScript(string name);
    
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
    /// Send a manialink to all players which persists even if a player re-connects.
    /// It will also automatically show for new players.
    /// </summary>
    /// <param name="name">Name of the template to show.</param>
    /// <param name="data">any kind of data the template uses.</param>
    /// <returns></returns>
    public Task SendPersistentManialinkAsync(string name, IDictionary<string, object?> data);
    
    /// <summary>
    /// Send a manialink to all players which persists even if a player re-connects.
    /// It will also automatically show for new players.
    /// </summary>
    /// <param name="name">Name of the template to show.</param>
    /// <param name="data">Any kind of data the template uses.</param>
    /// <returns></returns>
    public Task SendPersistentManialinkAsync(string name, dynamic data);
    
    /// <summary>
    /// Send a manialink to all players which persists even if a player re-connects.
    /// It will also automatically show for new players.
    /// </summary>
    /// <param name="name">Name of the template to show.</param>
    /// <returns></returns>
    public Task SendPersistentManialinkAsync(string name);
    
    /// <summary>
    /// Render a template and send it to a specific player.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses.</param>
    /// <param name="player">The player to send to.</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IPlayer player, string name, IDictionary<string, object?> data);
    
    /// <summary>
    /// Render a template and send it to a specific player.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <param name="player">The player to send to</param>
    /// <returns></returns>
    public Task SendManialinkAsync(IPlayer player, string name, dynamic data);
    
    /// <summary>
    /// Render a template and send it to a specific player without data.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="player">The player to send to</param>
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
    /// Hide a manialink from a set of players.
    /// </summary>
    /// <param name="players">The players to hide the manialink from.</param>
    /// <param name="name">Name of the manialink to hide.</param>
    /// <returns></returns>
    public Task HideManialinkAsync(IEnumerable<IPlayer> players, string name);
    
    /// <summary>
    /// Pre-process all current templates registered.
    /// </summary>
    /// <returns></returns>
    public Task PreprocessAllAsync();
}
