using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Modules.Attributes;
using SimpleInjector;

namespace EvoSC.Modules;

public interface IModuleLoadContext
{
    public IEvoScModule? Instance { get; init; }
    public Container Services { get; init; }
    public AssemblyLoadContext? LoadContext { get; init; }
    public Guid LoadId { get; init; }
    public Type? ModuleClass { get; init; }
    public ModuleAttribute ModuleInfo { get; init; }
    public Assembly Assembly { get; init; }
}
