using System.Reflection;
using SimpleInjector;

namespace EvoSC.Modules.Interfaces;

/// <summary>
/// Provides logic for handling of services within a module.
/// </summary>
public interface IModuleServicesManager
{
    /// <summary>
    /// Register a service container for a module.
    /// </summary>
    /// <param name="moduleId">The ID of the loaded module.</param>
    /// <param name="container">The container to register for the module.</param>
    public void AddContainer(Guid moduleId, Container container);
    public Container NewContainer(Guid moduleId, IEnumerable<Assembly> assemblies);
    /// <summary>
    /// Remove a container from a module.
    /// </summary>
    /// <param name="moduleId">ID of the loaded module.</param>
    public void RemoveContainer(Guid moduleId);
}
