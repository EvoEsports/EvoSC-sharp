using System;
using System.Threading.Tasks;
using EvoSC.Interfaces.Players;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Plugins;

public interface IPlugin
{
    public string Name { get; }

    public Version Version { get; }

    public void HandleEvents(IPlayerCallbacks playerCallbacks);

    void Register(IServiceCollection services);

    void Execute();

    void Unregister(IServiceCollection services);
}
