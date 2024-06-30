using EvoSC.Manialinks.Interfaces.Models;

namespace EvoSC.Manialinks.Interfaces;

/// <summary>
/// Manialink template & ManiaScript registry, and can render, display and hide Manialink to players.
/// </summary>
public interface IManialinkManager : IManialinkOperations
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
    /// Remove a template so that it can no longer be used. In addition to that
    /// hide it from all players.
    /// </summary>
    /// <param name="name">The name of the template</param>
    /// <returns></returns>
    public Task RemoveAndHideTemplateAsync(string name);

    /// <summary>
    /// Remove a ManiaScript resource.
    /// </summary>
    /// <param name="name">The name of the ManiaScript resource</param>
    public void RemoveManiaScript(string name);
    
    /// <summary>
    /// Pre-process all current templates registered.
    /// </summary>
    /// <returns></returns>
    public Task PreprocessAllAsync();

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
    /// Send a manialink to all players which persists even if a player re-connects.
    /// It will also automatically show for new players.
    ///
    /// This method allows for dynamic data updates by a callback method.
    /// </summary>
    /// <param name="name">Name of the template to show.</param>
    /// <param name="setupData">Method that returns data to be sent.</param>
    /// <returns></returns>
    public Task SendPersistentManialinkAsync(string name, Func<Task<dynamic>> setupData);
    
    /// <summary>
    /// Send a manialink to all players which persists even if a player re-connects.
    /// It will also automatically show for new players.
    ///
    /// This method allows for dynamic data updates by a callback method.
    /// </summary>
    /// <param name="name">Name of the template to show.</param>
    /// <param name="setupData">Method that returns data to be sent.</param>
    /// <returns></returns>
    public Task SendPersistentManialinkAsync(string name, Func<Task<IDictionary<string, object?>>> setupData);

    /// <summary>
    /// Hides and removes a persistent mainalink.
    /// </summary>
    /// <param name="name">Name of the template to hide.</param>
    /// <returns></returns>
    public Task RemovePersistentManialinkAsync(string name);
    
    /// <summary>
    /// Add a global variable that is accessible from all templates.
    /// </summary>
    /// <param name="name">Name of the variable.</param>
    /// <param name="value">Value of the variable.</param>
    /// <typeparam name="T">Variable type.</typeparam>
    public void AddGlobalVariable<T>(string name, T value);
    
    /// <summary>
    /// Remove a global variable.
    /// </summary>
    /// <param name="name">Name of the variable to remove.</param>
    public void RemoveGlobalVariable(string name);
    
    /// <summary>
    /// Remove all global variables.
    /// </summary>
    public void ClearGlobalVariables();

    /// <summary>
    /// Get the rendered contents of a template.
    /// </summary>
    /// <param name="name">Name of the template.</param>
    /// <param name="data">Data to send to the template.</param>
    /// <returns></returns>
    public Task<string> PrepareAndRenderAsync(string name, IDictionary<string, object?> data);
    
    /// <summary>
    /// Get the rendered contents of a template.
    /// </summary>
    /// <param name="name">Name of the template.</param>
    /// <param name="data">Data to send to the template.</param>
    /// <returns></returns>
    public Task<string> PrepareAndRenderAsync(string name, dynamic data);
    
    /// <summary>
    /// Get the effective name of a template's name. This may change
    /// depending on the theme.
    /// </summary>
    /// <param name="name">Original name of the template.</param>
    /// <returns></returns>
    public string GetEffectiveName(string name);

    /// <summary>
    /// Create a new transaction for manialink operations.
    /// Nothing is done until the transaction is committed,
    /// and all operations are sent at once for speed.
    /// </summary>
    /// <returns></returns>
    public IManialinkTransaction CreateTransaction();
}
