using System.Reflection;

namespace EvoSC.Modules;

/// <summary>
/// Main manager for modules, provides loading/unloading and enabling/disabling of modules.
/// </summary>
public interface IModuleManager
{
    /// <summary>
    /// Load all modules from a provided assembly.
    /// </summary>
    /// <param name="assembly">The assembly to search modules in.</param>
    /// <returns></returns>
    public Task LoadModulesFromAssembly(Assembly assembly);
}
