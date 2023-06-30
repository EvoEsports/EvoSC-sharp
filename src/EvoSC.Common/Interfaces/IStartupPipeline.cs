using EvoSC.Common.Application;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using SimpleInjector;

namespace EvoSC.Common.Interfaces;

public interface IStartupPipeline
{
    public Container ServiceContainer { get; }
    
    public IStartupPipeline Services(string name, Action<ServicesBuilder> servicesConfig, params string[] dependencies);

    public IStartupPipeline Services(Enum name, Action<ServicesBuilder> servicesConfig, params string[] dependencies) =>
        Services(name.GetIdentifier(), servicesConfig, dependencies);
    public IStartupPipeline Action(string name, Action<ServicesBuilder> action, params string[] dependencies);

    public IStartupPipeline Action(Enum name, Action<ServicesBuilder> action, params string[] dependencies) =>
        Action(name.GetIdentifier(), action, dependencies);
    
    public IStartupPipeline ActionAsync(string name, Func<ServicesBuilder, Task> action, params string[] dependencies);

    public IStartupPipeline ActionAsync(Enum name, Func<ServicesBuilder, Task> action, params string[] dependencies) =>
        ActionAsync(name.GetIdentifier(), action, dependencies);

    public Task ExecuteAsync(params string[] components);
    public Task ExecuteAllAsync();
}
