using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

public interface IManialinkManager
{
    /// <summary>
    /// Add all the default templates from EvoSC.
    /// </summary>
    /// <returns></returns>
    public Task AddDefaultTemplatesAsync();
    
    /// <summary>
    /// Register a template, does not pre-process the template.
    /// </summary>
    /// <param name="template">The template to add.</param>
    internal void AddTemplate(IManialinkTemplateInfo template);
    
    /// <summary>
    /// Add and immediately pre-process a template.
    /// </summary>
    /// <param name="template">The template to add.</param>
    /// <returns></returns>
    public Task AddAndPreProcessTemplateAsync(IManialinkTemplateInfo template);
    
    /// <summary>
    /// Register a new ManiaScript resource.
    /// </summary>
    /// <param name="maniaScript">The ManiaScript to add</param>
    internal void AddManiaScript(IManiaScriptInfo maniaScript);
    
    /// <summary>
    /// Register a new ManiaScript resource.
    /// </summary>
    /// <param name="maniaScript">The ManiaScript to add</param>
    public Task AddManiaScriptAsync(IManiaScriptInfo maniaScript);
    
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
    public Task SendManialinkAsync(string name, dynamic data);
    
    /// <summary>
    /// Render a template and send it to a specific player.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <param name="player">The player to send to</param>
    /// <returns></returns>
    public Task SendManialinkAsync(string name, IDictionary<string, object?> data, IPlayer player);
    public Task SendManialinkAsync(string name, dynamic data, IPlayer player);
    
    /// <summary>
    /// Render a template and send it to a set of players.
    /// </summary>
    /// <param name="name">The name of the template.</param>
    /// <param name="data">Data which the template uses</param>
    /// <param name="player">The players to send to</param>
    /// <returns></returns>
    public Task SendManialinkAsync(string name, IDictionary<string, object?> data, IEnumerable<IPlayer> players);
    public Task SendManialinkAsync(string name, dynamic data, IEnumerable<IPlayer> players);

    public Task HideManialinkAsync(string name);
    public Task HideManialinkAsync(string name, IPlayer player);
    public Task HideManialinkAsync(string name, IEnumerable<IPlayer> players);
    
    /// <summary>
    /// Pre-process all current templates registered.
    /// </summary>
    /// <returns></returns>
    public Task PreprocessAllAsync();
}
