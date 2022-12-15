using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Middleware;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using SimpleInjector;

namespace EvoSC.Modules.Models;

public class ModuleLoadContext : IModuleLoadContext
{
    public required IEvoScModule? Instance { get; init; }
    public required Container Services { get; init; }
    public required AssemblyLoadContext? AsmLoadContext { get; init; }
    public required Guid LoadId { get; init; }
    public required Type? MainClass { get; init; }
    public required IModuleInfo ModuleInfo { get; init; }
    public required IEnumerable<Assembly> Assemblies { get; init; }
    public required Dictionary<PipelineType, IActionPipeline> Pipelines { get; init; }
    public required List<IPermission> Permissions { get; set; }
    public required List<Guid> LoadedDependencies { get; init; }
}
