using EvoSC;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PluginSample.Events;
using PluginSample.Interfaces;

namespace PluginSample;

public class MyPlugin : IPlugin
{
    public string Name => "MyPlugin";

    public Version Version => Assembly.GetExecutingAssembly().GetName().Version!;

    public void Execute()
    {
        Console.WriteLine("Executing...");
        
    }

    public void Load(IServiceCollection services)
    {
        services.AddTransient<ISampleService, SampleService>();
        services.AddSingleton<IPlayerEventHandler, PlayerEventHandler>();
    }

    public void Unload(IServiceCollection services)
    {
        services.Remove<SampleService>();

        Console.WriteLine("Unloading...");
    }
}