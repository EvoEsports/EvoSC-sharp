using EvoSC.Common.Application;
using EvoSC.Common.Util;
using EvoSC.Common.Util.EnumIdentifier;
using SimpleInjector;

namespace EvoSC.Common.Interfaces;

public interface IStartupPipeline
{
    public Container ServiceContainer { get; }
    
    public IStartupPipeline Services(string name, Action<ServicesBuilder> servicesConfig);

    public IStartupPipeline Services(Enum name, Action<ServicesBuilder> servicesConfig) =>
        Services(name.GetIdentifier(), servicesConfig);
    public IStartupPipeline Action(string name, Action<ServicesBuilder> action);
    public IStartupPipeline Action(Enum name, Action<ServicesBuilder> action) => Action(name.GetIdentifier(), action);
    public IStartupPipeline ActionAsync(string name, Func<ServicesBuilder, Task> action);

    public IStartupPipeline ActionAsync(Enum name, Func<ServicesBuilder, Task> action) =>
        ActionAsync(name.GetIdentifier(), action);

    public Task ExecuteAsync(params string[] components);
    public Task ExecuteAllAsync();
}
