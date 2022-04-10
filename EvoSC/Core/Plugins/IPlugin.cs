using System;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Plugins;

public interface IPlugin
{
    public string Name { get; }
    public Version Version { get; }

    void Load(IServiceCollection services);
    void Execute();
    void Unload(IServiceCollection services);
}
