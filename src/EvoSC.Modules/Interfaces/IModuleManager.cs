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
}
