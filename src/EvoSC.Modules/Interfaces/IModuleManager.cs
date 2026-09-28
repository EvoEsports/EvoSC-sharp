using System.Reflection;

namespace EvoSC.Modules.Interfaces;

/// <summary>
/// Main manager for modules, provides loading/unloading and enabling/disabling of modules.
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// Get a snapshot of the currently loaded modules.
    /// </summary>
    public IReadOnlyList<IModuleLoadContext> GetLoadedModules();

    /// <summary>
    /// Get the loaded modules ordered so that a module comes after the modules it depends on. Use
    /// this where the order matters, such as running migrations: a foreign key can only point at a
    /// table that already exists. Modules that depend on each other cannot be ordered, and are
    /// returned in the order they were loaded in.
    /// </summary>
    public IReadOnlyList<IModuleLoadContext> GetLoadedModulesByDependency();

    /// <summary>
    /// Get the load context of a module by it's load ID.
    /// </summary>
    /// <param name="loadId">The load ID of the module.</param>
    /// <returns></returns>
    public IModuleLoadContext GetModule(Guid loadId);
    
    /// <summary>
    /// Enable a module.
    /// </summary>
    /// <param name="loadId">The load ID of the module to enable.</param>
    /// <returns></returns>
    public Task EnableAsync(Guid loadId);

    /// <summary>
    /// Enable all modules that allow for it.
    /// </summary>
    /// <returns></returns>
    public Task EnableModulesAsync();
    
    /// <summary>
    /// Disable a module.
    /// </summary>
    /// <param name="loadId">The load ID of the module to disable.</param>
    /// <returns></returns>
    public Task DisableAsync(Guid loadId);

    /// <summary>
    /// Run the installation of a module.
    /// </summary>
    /// <param name="loadId">The load ID of the module.</param>
    /// <returns></returns>
    public Task InstallAsync(Guid loadId);
    
    /// <summary>
    /// Run the uninstallation of a module.
    /// </summary>
    /// <param name="loadId">The load ID of the module.</param>
    /// <returns></returns>
    public Task UninstallAsync(Guid loadId);
    
    /// <summary>
    /// Load an external module from a directory.
    /// </summary>
    /// <param name="directory">The directory containing module info and binaries.</param>
    /// <returns></returns>
    public Task LoadAsync(string directory);
    
    /// <summary>
    /// Load an external module.
    /// </summary>
    /// <param name="moduleInfo">Module info for the external module.</param>
    /// <param name="install">Whether to run the module's installation as part of the load. Pass
    /// false to load a module that was previously unloaded without running its installation
    /// again.</param>
    /// <returns></returns>
    public Task LoadAsync(IExternalModuleInfo moduleInfo, bool install = true);
    
    /// <summary>
    /// Load the modules that ship with EvoSC from a fixed directory. An internal module is loaded
    /// exactly like an external one - its own load context, its own dependencies, its own
    /// migrations - the difference being that the application registers it by id instead of it
    /// being discovered, and that it can never be unloaded or reloaded at run time.
    /// </summary>
    /// <param name="moduleIds">The ids of the internal modules to load.</param>
    /// <param name="directory">The directory containing the module directories.</param>
    /// <returns></returns>
    public Task LoadInternalModulesAsync(IEnumerable<string> moduleIds, string directory);

    /// <summary>
    /// Load a collection of external modules. This will load modules in the order represented
    /// by the collection. You can use SortedModuleCollection to sort by dependencies.
    /// </summary>
    /// <param name="collection">The collection of modules to load.</param>
    /// <returns></returns>
    public Task LoadAsync(IModuleCollection<IExternalModuleInfo> collection);
    
    /// <summary>
    /// Unload a module. This disables and removes the module from memory.
    /// </summary>
    /// <param name="loadId">The load ID of the module to unload.</param>
    /// <returns></returns>
    public Task UnloadAsync(Guid loadId);

    /// <summary>
    /// Reload an external module from its directory. Dependents that were unloaded as
    /// part of the dependency cascade are not automatically reloaded.
    /// </summary>
    /// <param name="loadId">The load ID of the module to reload.</param>
    /// <returns></returns>
    public Task ReloadAsync(Guid loadId);
}
