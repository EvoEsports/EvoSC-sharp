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
    /// Disables a module. A module another enabled module depends on is refused instead.
    /// </summary>
    /// <param name="module">The module to disable.</param>
    /// <returns></returns>
    public Task DisableModuleAsync(IModuleLoadContext module);
    
    /// <summary>
    /// Loads a module from a directory containing its info.toml and binaries. Unlike installing, a
    /// loaded module does not run any uninstallable install routines.
    /// </summary>
    /// <param name="directory">The directory of the module to load.</param>
    /// <returns></returns>
    public Task LoadModuleAsync(string directory);
    
    /// <summary>
    /// Reloads an external module from its directory. Internal modules cannot be reloaded.
    /// </summary>
    /// <param name="module">The module to reload.</param>
    /// <returns></returns>
    public Task ReloadModuleAsync(IModuleLoadContext module);
    
    /// <summary>
    /// Unloads a module, together with the modules that depend on it. Internal modules cannot be
    /// unloaded.
    /// </summary>
    /// <param name="module">The module to unload.</param>
    /// <returns></returns>
    public Task UnloadModuleAsync(IModuleLoadContext module);
    
    /// <summary>
    /// Installs a module from a directory containing its info.toml and binaries. Installing runs
    /// the module's installation in addition to loading it.
    /// </summary>
    /// <param name="directory">The directory of the module to install.</param>
    /// <returns></returns>
    public Task InstallModuleAsync(string directory);
    
    /// <summary>
    /// Runs the uninstallation of a module, which is the module's own uninstall routine. The module
    /// stays loaded afterwards.
    /// </summary>
    /// <param name="module">The module to uninstall.</param>
    /// <returns></returns>
    public Task UninstallModuleAsync(IModuleLoadContext module);
    
    /// <summary>
    /// Print a list of loaded modules in the chat to a player.
    /// </summary>
    /// <param name="actor">The player to send to</param>
    /// <returns></returns>
    public Task ListModulesAsync(IPlayer actor);
}
