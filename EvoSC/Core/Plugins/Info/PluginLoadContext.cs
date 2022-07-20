using System;
using System.Runtime.Loader;
using EvoSC.Core.Plugins.Abstractions;

namespace EvoSC.Core.Plugins.Info;

public class PluginLoadContext : IPluginLoadContext
{
    public IPluginMetaInfo MetaInfo { get; init; }
    public IPlugin Instance { get; private set; }
    public AssemblyLoadContext LoadContext { get; private set; }
    public Guid LoadId { get; init; }
    public IServiceProvider ServiceProvider { get; private set; }
    public Type? PluginClass { get; private set; }

    public bool UnloadAssemblies()
    {
        var weakRef = new WeakReference(Instance);
        Instance = null;
        PluginClass = null;
        ServiceProvider = null;
        
        LoadContext.Unload();
        LoadContext = null;

        for (var i = 0; i < 10 && weakRef.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        return !weakRef.IsAlive;
    }

    public void SetAssemblyContext(AssemblyLoadContext loadContext)
    {
        LoadContext = loadContext;
    }

    public void SetInstance(IPlugin instance)
    {
        Instance = instance;
    }

    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void SetPluginClass(Type type)
    {
        PluginClass = type;
    }
}
