using System.Reflection;
using System.Runtime.Loader;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules;

public interface IModuleLoadContext
{
    public IEvoScModule? Instance { get; init; }
    public AssemblyLoadContext? LoadContext { get; init; }
    public Guid LoadId { get; init; }
    public Type? ModuleClass { get; init; }
    public ModuleAttribute ModuleInfo { get; init; }
    public Assembly Assembly { get; init; }
}
