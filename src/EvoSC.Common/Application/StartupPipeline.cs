using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using SimpleInjector;

namespace EvoSC.Common.Application;

public class StartupPipeline : IStartupPipeline
{
    private readonly IEvoScBaseConfig _config;
    private readonly ServicesBuilder _services;

    public StartupPipeline(IEvoScBaseConfig config)
    {
        _config = config;
        _services = new ServicesBuilder();
    }

    public Container ServiceContainer => _services;

    public IStartupPipeline Services(string name, Action<ServicesBuilder> servicesConfig)
    {
        throw new NotImplementedException();
    }

    public IStartupPipeline Action(string name, Action<ServicesBuilder> action)
    {
        throw new NotImplementedException();
    }

    public IStartupPipeline ActionAsync(string name, Func<ServicesBuilder, Task> action)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(params string[] components)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAllAsync()
    {
        throw new NotImplementedException();
    }
}
