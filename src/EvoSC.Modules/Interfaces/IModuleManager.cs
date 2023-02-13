using System.Reflection;

namespace EvoSC.Modules.Interfaces;

/// <summary>
/// Main manager for modules, provides loading/unloading and enabling/disabling of modules.
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// Warning: creates a copy of loaded modules. O(n)
    /// </summary>
    public IReadOnlyList<IModuleLoadContext> LoadedModules { get; }

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
    /// <returns></returns>
    public Task LoadAsync(IExternalModuleInfo moduleInfo);
    
    /// <summary>
    /// Load a module from an assembly.
    /// </summary>
    /// <param name="assembly">The assembly that contains the module.</param>
    /// <returns></returns>
    public Task LoadAsync(Assembly assembly);
    
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
}
