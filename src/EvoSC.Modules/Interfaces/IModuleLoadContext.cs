using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Middleware;
using SimpleInjector;

namespace EvoSC.Modules.Interfaces;

/// <summary>
/// Information about a loaded module.
/// </summary>
public interface IModuleLoadContext
{
    /// <summary>
    /// The instance of the module's main class.
    /// </summary>
    public IEvoScModule? Instance { get; init; }
    
    /// <summary>
    /// The service container for this module.
    /// </summary>
    public Container Services { get; init; }
    
    /// <summary>
    /// Load context for the module's assembly.
    /// </summary>
    public AssemblyLoadContext? AsmLoadContext { get; init; }
    
    /// <summary>
    /// Unique ID of the module, used for referencing it at runtime. This ID is generated when the
    /// module is loaded.
    /// </summary>
    public Guid LoadId { get; init; }
    
    /// <summary>
    /// The type of the module's main class.
    /// </summary>
    public Type? MainClass { get; init; }
    
    /// <summary>
    /// Meta info about a module.
    /// </summary>
    public IModuleInfo ModuleInfo { get; init; }
    
    /// <summary>
    /// The module's assembly object.
    /// </summary>
    public IEnumerable<Assembly> Assemblies { get; init; }
    
    /// <summary>
    /// Middleware pipelines registered from this module.
    /// </summary>
    public Dictionary<PipelineType, IActionPipeline> Pipelines { get; init; }
    
    /// <summary>
    /// Permissions registered from this module.
    /// </summary>
    public List<IPermission> Permissions { get; set; }
    
    /// <summary>
    /// References to loaded modules that this module depends on.
    /// </summary>
    public List<Guid> LoadedDependencies { get; init; }
    
    public List<IModuleManialinkTemplate> ManialinkTemplates { get; init; }
    
    public string RootNamespace { get; init; }

    /// <summary>
    /// Whether this module is currently enabled.
    /// </summary>
    public bool IsEnabled { get; }

    /// <summary>
    /// Set the enabled status of this module.
    /// </summary>
    /// <param name="enabled">True if enabled, false otherwise.</param>
    internal void SetEnabled(bool enabled);
}
