using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading.Tasks;
using EvoSC.Core.Plugins;
using EvoSC.Interfaces.Commands;
using EvoSC.Interfaces.Players;
using Microsoft.Extensions.DependencyInjection;

namespace Info;

public class ExamplePlugin : IPlugin
{
    public string Name => "Example";
    public Version Version => new(1, 0, 0);

    public void HandleEvents(IPlayerCallbacks playerCallbacks)
    {
    }

    public void Register(IServiceCollection services)
    {
        Console.WriteLine("hallo");
    }

    public void Execute()
    {
    }

    public void Unregister(IServiceCollection services)
    {
    }

    public Task Load(IChatCommandsService commands)
    {
        return Task.CompletedTask;
    }

    public Task Unload(IChatCommandsService commands)
    {
        return Task.CompletedTask;
    }
}