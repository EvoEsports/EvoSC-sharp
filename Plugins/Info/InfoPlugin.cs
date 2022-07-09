using System.ComponentModel.Design;
using EvoSC.Core.Plugins;
using EvoSC.Interfaces.Commands;
using EvoSC.Interfaces.Players;
using Microsoft.Extensions.DependencyInjection;

namespace Info;

public class InfoPlugin : IPlugin
{
    public string Name => "Info";
    public Version Version => new(1, 0, 0);
    
    public void HandleEvents(IPlayerCallbacks playerCallbacks)
    {
    }

    public void Register(IServiceCollection services)
    {
    }

    public void Execute()
    {
    }

    public void Unregister(IServiceCollection services)
    {
    }

    public Task Load(IChatCommandsService commands)
    {
        commands.RegisterCommands<ChatCommands>();
        return Task.CompletedTask;
    }

    public Task Unload(IChatCommandsService commands)
    {
        commands.UnregisterCommands<ChatCommands>();
        return Task.CompletedTask;
    }
}