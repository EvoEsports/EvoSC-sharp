using System.Reflection;

namespace EvoSC.Modules.Interfaces;

/// <summary>
/// Main manager for modules, provides loading/unloading and enabling/disabling of modules.
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// Load all modules from a provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly which should be scanned for modules.</param>
    /// <returns></returns>
    public Task LoadModulesFromAssembly(Assembly assembly);
    /// <summary>
    /// Enable the module assigned to the specified load ID.
    /// </summary>
    /// <param name="loadId">Load ID for the module to enable.</param>
    /// <returns></returns>
    public Task EnableModule(Guid loadId);
}
