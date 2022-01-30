using EvoSC.Contracts;
using EvoSC.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PluginSample;

public class MyPlugin : IEvoScPlugin
{
    public string Name => "MyPlugin";

    public Version Version => Assembly.GetExecutingAssembly().GetName().Version!;

    public void Execute()
    {
        throw new NotImplementedException();
    }

    public void Load(IServiceCollection services)
    {
        services.AddTransient<ISampleService, SampleService>();
    }

    public void Unload()
    {
        throw new NotImplementedException();
    }
}