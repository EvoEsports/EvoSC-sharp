using EvoSC;
using EvoSC.Core.Plugins;
using EvoSC.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using PluginSample.Events;
using PluginSample.Interfaces;
using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Interfaces.Commands;
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

    public void Register(IServiceCollection services)
    {
        services.AddSingleton<ISampleService, SampleService>();
        services.AddSingleton<IPlayerEventHandler, PlayerEventHandler>();
        services.AddSingleton<IPlugin, MyPlugin>();
    }

    public void Unregister(IServiceCollection services)
    {
        services.Remove<SampleService>();

        Console.WriteLine("Unloading...");
    }

    public async Task Load(IChatCommandsService chatCommands, IPermissionsService permissions)
    {
        await permissions.AddPermission("admin", "Admin stuff");
        await chatCommands.RegisterCommands<ExampleChatCommands>();
    }

    public async Task Unload(IChatCommandsService chatCommands)
    {
        await chatCommands.UnregisterCommands<ExampleChatCommands>();
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