using EvoSC.Common.Application.Exceptions;
using EvoSC.Common.Application.Models;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Application;
using EvoSC.Common.Logging;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Security;
using SimpleInjector;

namespace EvoSC.Common.Application;

public class StartupPipeline : IStartupPipeline, IDisposable
{
    private readonly ServicesBuilder _services;
    private readonly ILogger<StartupPipeline> _logger;
    private readonly Dictionary<string, IStartupComponent> _components = new();
    
    public Container ServiceContainer => _services;
    
    public StartupPipeline(IEvoScBaseConfig config)
    {
        _services = new ServicesBuilder();

        _logger = new Container().AddEvoScLogging(config.Logging).GetInstance<ILogger<StartupPipeline>>();
    }

    private void CreatedDependencyOrder(IEnumerable<string> components, Dictionary<string, IStartupComponent> services,
        Dictionary<string, IStartupComponent> actions, HashSet<string> previousComponents)
    {
        foreach (var name in components)
        {
            if (previousComponents.Contains(name))
            {
                throw new StartupDependencyCycleException(previousComponents);
            }
            
            if (!_components.ContainsKey(name))
            {
                throw new StartupPipelineException($"Startup component {name} does not exist.");
            }
            
            var component = _components[name];

            var currentComponents = new HashSet<string>(previousComponents) {name};
            CreatedDependencyOrder(component.Dependencies, services, actions, currentComponents);
            
            if (component is IServiceStartupComponent)
            {
                services[name] = component;
            }
            else
            {
                actions[name] = component;
            }
        }
    }

    private void LogExecutionSuccess()
    {
        _logger.LogDebug("Startup pipeline finished");
    }
    
    public IStartupPipeline Services(string name, Action<ServicesBuilder> servicesConfig, params string[] dependencies)
    {
        _components[name] = new ServiceStartupComponent
        {
            Name = name, Dependencies = dependencies.ToList(), ConfigAction = servicesConfig
        };

        return this;
    }

    public IStartupPipeline Action(string name, Action<ServicesBuilder> actionFunc, params string[] dependencies)
    {
        _components[name] = new ActionStartupComponent
        {
            Name = name, Dependencies = dependencies.ToList(), Action = actionFunc
        };
        
        return this;
    }

    public IStartupPipeline AsyncAction(string name, Func<ServicesBuilder, Task> actionFunc, params string[] dependencies)
    {
        _components[name] = new AsyncActionStartupComponent
        {
            Name = name, Dependencies = dependencies.ToList(), AsyncAction = actionFunc
        };

        return this;
    }

    private async Task ExecuteComponentAsync(IStartupComponent component)
    {
        _logger.LogDebug("Executing startup component {Name}", component.Name);
        
        if (component is IServiceStartupComponent serviceComponent)
        {
            serviceComponent.ConfigAction(_services);
        }
        else if (component is IActionStartupComponent actionComponent)
        {
            actionComponent.Action(_services);
        }
        else if (component is IAsyncActionStartupComponent asyncActionComponent)
        {
            await asyncActionComponent.AsyncAction(_services);
        }
    }

    public async Task ExecuteAsync(params string[] components)
    {
        var services = new Dictionary<string, IStartupComponent>();
        var actions = new Dictionary<string, IStartupComponent>();

        CreatedDependencyOrder(components, services, actions, new HashSet<string>());

        foreach (var (_, component) in services)
        {
            await ExecuteComponentAsync(component);
        }

        foreach (var (_, component) in actions)
        {
            await ExecuteComponentAsync(component);
        }
        
        LogExecutionSuccess();
    }

    public async Task ExecuteAllAsync()
    {
        foreach (var component in _components.Values)
        {
            await ExecuteComponentAsync(component);
        }
        
        LogExecutionSuccess();
    }

    public void Dispose()
    {
        _services.Dispose();
    }
}
