using System.ComponentModel.Design;
using System.Diagnostics;
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

    public async Task Load(IChatCommandsService commands, IPermissionsService permissions)
    {
        permissions.AddPermission("test", "something");
        await commands.RegisterCommands<ChatCommands>();
    }

    public async Task Unload(IChatCommandsService commands)
    {
        await commands.UnregisterCommands<ChatCommands>();
    }
}