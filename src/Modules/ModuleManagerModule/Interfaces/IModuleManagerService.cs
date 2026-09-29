using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ModuleManagerModule.Interfaces;

public interface IModuleManagerService
{
    /// <summary>
    /// Enables a module.
    /// </summary>
    Task EnableModuleAsync(IModuleLoadContext module);

    /// <summary>
    /// Disables a module. A module another enabled module depends on is refused instead.
    /// </summary>
    Task DisableModuleAsync(IModuleLoadContext module);

    /// <summary>
    /// Loads a module from a directory containing its info.toml and binaries. Unlike installing, a
    /// loaded module does not run any uninstallable install routines.
    /// </summary>
    Task LoadModuleAsync(string directory);

    /// <summary>
    /// Reloads an external module from its directory. Internal modules cannot be reloaded.
    /// </summary>
    Task ReloadModuleAsync(IModuleLoadContext module);

    /// <summary>
    /// Unloads a module, together with the modules that depend on it. Internal modules cannot be
    /// unloaded.
    /// </summary>
    Task UnloadModuleAsync(IModuleLoadContext module);

    /// <summary>
    /// Installs a module from a directory containing its info.toml and binaries. Installing runs
    /// the module's installation in addition to loading it.
    /// </summary>
    Task InstallModuleAsync(string directory);

    /// <summary>
    /// Runs the uninstallation of a module, which is the module's own uninstall routine. The module
    /// stays loaded afterwards.
    /// </summary>
    Task UninstallModuleAsync(IModuleLoadContext module);

    /// <summary>
    /// Prints a list of the loaded modules to the chat.
    /// </summary>
    Task ListModulesAsync();
}