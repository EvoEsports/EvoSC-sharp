using EvoSC.Common.Application;
using EvoSC.Common.Util.EnumIdentifier;
using SimpleInjector;

namespace EvoSC.Common.Interfaces;

public interface IStartupPipeline
{
    public Container ServiceContainer { get; }
    
    public IStartupPipeline Services(string name, Action<ServicesBuilder> servicesConfig, params string[] dependencies);

    public IStartupPipeline Services(Enum name, Action<ServicesBuilder> servicesConfig, params string[] dependencies) =>
        Services(name.GetIdentifier(), servicesConfig, dependencies);
    public IStartupPipeline Action(string name, Action<ServicesBuilder> actionFunc, params string[] dependencies);

    public IStartupPipeline Action(Enum name, Action<ServicesBuilder> actionFunc, params string[] dependencies) =>
        Action(name.GetIdentifier(), actionFunc, dependencies);
    
    public IStartupPipeline AsyncAction(string name, Func<ServicesBuilder, Task> actionFunc, params string[] dependencies);

    public IStartupPipeline AsyncAction(Enum name, Func<ServicesBuilder, Task> actionFunc, params string[] dependencies) =>
        AsyncAction(name.GetIdentifier(), actionFunc, dependencies);

    public Task ExecuteAsync(params string[] components);
    public Task ExecuteAllAsync();
}
