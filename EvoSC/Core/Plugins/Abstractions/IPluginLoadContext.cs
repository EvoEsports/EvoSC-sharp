using System;
using System.Runtime.Loader;

namespace EvoSC.Core.Plugins.Abstractions;

public interface IPluginLoadContext
{
    public IPluginMetaInfo MetaInfo { get; }
    public IPlugin? Instance { get; }
    public AssemblyLoadContext? LoadContext { get; }
    public Guid LoadId { get; }
    public IServiceProvider? ServiceProvider { get; }
    public Type? PluginClass { get; }

    public bool UnloadAssemblies();
    public void SetAssemblyContext(AssemblyLoadContext loadContext);
    public void SetInstance(IPlugin instance);
    public void SetServiceProvider(IServiceProvider serviceProvider);
    public void SetPluginClass(Type type);
}
