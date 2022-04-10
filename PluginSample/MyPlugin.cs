using EvoSC;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PluginSample;

public class MyPlugin : IPlugin
{
    public string Name => "MyPlugin";

    public Version Version => Assembly.GetExecutingAssembly().GetName().Version!;

    public void Execute()
    {
        System.Console.WriteLine("Executing...");
    }

    public void Load(IServiceCollection services)
    {
        services.AddTransient<ISampleService, SampleService>();
    }

    public void Unload(IServiceCollection services)
    {
        services.Remove<SampleService>();

        System.Console.WriteLine("Unloading...");
    }
}