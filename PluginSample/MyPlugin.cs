using EvoSC;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PluginSample.Events;
using PluginSample.Interfaces;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Interfaces.Players;

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
        services.AddSingleton<ISampleService, SampleService>();
        services.AddSingleton<IPlayerEventHandler, PlayerEventHandler>();
        services.AddSingleton<IPlugin, MyPlugin>();

    }

    public void Unload(IServiceCollection services)
    {
        services.Remove<SampleService>();

        Console.WriteLine("Unloading...");
    }


    public void HandleEvents(IPlayerCallbacks playerCallbacks)
    {
        playerCallbacks.PlayerConnect += PlayerCallbacksOnPlayerConnect;
    }

    private void PlayerCallbacksOnPlayerConnect(object? sender, PlayerConnectEventArgs e)
    {
        Console.WriteLine("Fired playerConnect");
    }
}