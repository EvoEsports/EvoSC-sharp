using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Modules.Attributes;
using SimpleInjector;

namespace EvoSC.Modules;

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
    public AssemblyLoadContext? LoadContext { get; init; }
    /// <summary>
    /// Unique ID of the module, used for referencing it at runtime. This ID is generated when the
    /// module is loaded.
    /// </summary>
    public Guid LoadId { get; init; }
    /// <summary>
    /// The type of the module's main class.
    /// </summary>
    public Type? ModuleClass { get; init; }
    /// <summary>
    /// Meta info about a module.
    /// </summary>
    public ModuleAttribute ModuleInfo { get; init; }
    /// <summary>
    /// The module's assembly object.
    /// </summary>
    public Assembly Assembly { get; init; }
    
    public IActionPipeline ActionPipeline { get; init; }
    public List<IPermission> Permissions { get; set; }
}
