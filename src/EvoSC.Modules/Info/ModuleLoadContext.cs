using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Common.Interfaces.Middleware;
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
    public IActionPipeline ActionPipeline { get; init; }
}
