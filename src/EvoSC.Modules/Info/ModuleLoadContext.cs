using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Middleware;
using EvoSC.Modules.Attributes;
using SimpleInjector;

namespace EvoSC.Modules.Info;

public class ModuleLoadContext : IModuleLoadContext
{
    public IEvoScModule? Instance { get; init; }
    public Container Services { get; init; }
    public AssemblyLoadContext? LoadContext { get; init; }
    public Guid LoadId { get; init; }
    public Type? ModuleClass { get; init; }
    public ModuleAttribute ModuleInfo { get; init; }
    public Assembly Assembly { get; init; }
    public Dictionary<PipelineType, IActionPipeline> Pipelines { get; init; }
    public List<IPermission> Permissions { get; set; }
}
