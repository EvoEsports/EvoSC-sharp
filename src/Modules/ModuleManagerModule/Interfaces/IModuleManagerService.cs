using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

public interface IModuleManagerService
{
    /// <summary>
    /// Enables a module.
    /// </summary>
    /// <param name="module">The module to enable</param>
    /// <returns></returns>
    public Task EnableModuleAsync(IModuleLoadContext module);
    
    /// <summary>
    /// Disables a module.
    /// </summary>
    /// <param name="module">The module to disable.</param>
    /// <returns></returns>
    public Task DisableModuleAsync(IModuleLoadContext module);
    
    /// <summary>
    /// Print a list of loaded modules in the chat to a player.
    /// </summary>
    /// <param name="actor">The player to send to</param>
    /// <returns></returns>
    public Task ListModulesAsync(IPlayer actor);
}
