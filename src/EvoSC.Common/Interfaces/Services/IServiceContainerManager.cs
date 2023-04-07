using System.Reflection;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Services;

public interface IServiceContainerManager
{
    /// <summary>
    /// Register a service container for a module.
    /// </summary>
    /// <param name="moduleId">The ID of the loaded module.</param>
    /// <param name="container">The container to register for the module.</param>
    public void AddContainer(Guid moduleId, Container container);
    
    /// <summary>
    /// Create a new module service container.
    /// </summary>
    /// <param name="moduleId">The unique container GUID. This is typically the load ID.</param>
    /// <param name="assemblies">Assemblies related to the module to scan for additional services.</param>
    /// <returns></returns>
    public Container NewContainer(Guid moduleId, IEnumerable<Assembly> assemblies, List<Guid> loadedDependencies);
    
    /// <summary>
    /// Remove a container from a module.
    /// </summary>
    /// <param name="moduleId">ID of the loaded module.</param>
    public void RemoveContainer(Guid moduleId);
    
    /// <summary>
    /// Register a service dependency for a module. This allows a module to inject
    /// services from the specified dependency.
    /// </summary>
    /// <param name="moduleId">The ID of the module that requires the dependency.</param>
    /// <param name="dependencyId">The ID of the dependency.</param>
    public void RegisterDependency(Guid moduleId, Guid dependencyId);
}
