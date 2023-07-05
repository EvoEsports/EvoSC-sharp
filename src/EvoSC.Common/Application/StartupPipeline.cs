using EvoSC.Common.Application.Exceptions;
using EvoSC.Common.Application.Models;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Application;
using EvoSC.Common.Logging;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace EvoSC.Common.Application;

public class StartupPipeline : IStartupPipeline
{
    private readonly ServicesBuilder _services;
    private readonly ILogger<StartupPipeline> _logger;
    private readonly Dictionary<string, IStartupComponent> _components = new();
    
    public StartupPipeline(IEvoScBaseConfig config)
    {
        _services = new ServicesBuilder();

        _logger = new Container().AddEvoScLogging(config.Logging).GetInstance<ILogger<StartupPipeline>>();
    }

    public Container ServiceContainer => _services;
    
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
    
    private async Task ExecuteAsyncInternalAsync(IEnumerable<string> components, List<string> dependencyPath)
    {
        foreach (var name in components)
        {
            if (dependencyPath.Contains(name))
            {
                throw new StartupDependencyCycleException(dependencyPath);
            }
            
            if (_components.TryGetValue(name, out var component))
            {
                if (component.Dependencies.Count > 0)
                {
                    var newPath = new List<string>();
                    newPath.AddRange(dependencyPath);
                    newPath.Add(name);
                    await ExecuteAsyncInternalAsync(component.Dependencies.ToArray(), newPath);
                }

                await ExecuteComponentAsync(component);
            }
            else
            {
                throw new StartupPipelineException($"Startup component {name} does not exist.");
            }
        }
    }
    
    public Task ExecuteAsync(params string[] components)
    {
        return ExecuteAsyncInternalAsync(components, Array.Empty<string>().ToList());
    }

    public async Task ExecuteAllAsync()
    {
        foreach (var component in _components.Values)
        {
            await ExecuteComponentAsync(component);
        }
    }
}
