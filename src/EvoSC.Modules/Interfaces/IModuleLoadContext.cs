using System.Runtime.Loader;

namespace EvoSC.Modules;

public class IModuleLoadContext
{
    public IEvoScModule? Instance { get; }
    public AssemblyLoadContext? LoadContext { get; }
    public Guid LoadId { get; }
    public Type? PluginClass { get; }
}
