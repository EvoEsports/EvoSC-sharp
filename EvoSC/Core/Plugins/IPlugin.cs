using System;
using EvoSC.Interfaces.Players;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Plugins;

public interface IPlugin
{
    public string Name { get; }
    public Version Version { get; }
    public void HandleEvents(IPlayerCallbacks playerCallbacks);
    void Load(IServiceCollection services);
    void Execute();
    void Unload(IServiceCollection services);
}
