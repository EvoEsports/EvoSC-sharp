using EvoSC.Common.Application.Exceptions;
using EvoSC.Common.Application.Models;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Application;
using EvoSC.Common.Logging;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace EvoSC.Common.Application;

public class StartupPipeline(IEvoScBaseConfig config) : IStartupPipeline, IDisposable
{
    private readonly ServicesBuilder _services = new();
    private readonly ILogger<StartupPipeline> _logger = new Container().AddEvoScLogging(config.Logging).GetInstance<ILogger<StartupPipeline>>();
    private readonly Dictionary<string, IStartupComponent> _components = new();
    
    public Container ServiceContainer => _services;

    private void CreatedDependencyOrder(IEnumerable<string> components, Dictionary<string, IStartupComponent> services,
        Dictionary<string, IStartupComponent> actions, HashSet<string> previousComponents)
    {
        foreach (var name in components)
        {
            if (previousComponents.Contains(name))
            {
                throw new StartupDependencyCycleException(previousComponents, name);
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

    /// <summary>
    /// Runs every component in order, stopping before the first one once <paramref name="token"/> is
    /// cancelled. Returns false when execution stopped early, so a caller running several batches in
    /// sequence knows not to start the next one.
    /// </summary>
    private async Task<bool> ExecuteComponentsAsync(IEnumerable<IStartupComponent> components, CancellationToken token,
        Action<IStartupComponent>? onCancelled = null)
    {
        foreach (var component in components)
        {
            if (token.IsCancellationRequested)
            {
                onCancelled?.Invoke(component);
                return false;
            }

            await ExecuteComponentAsync(component);
        }

        return true;
    }

    public async Task ExecuteAsync(params string[] components) => await ExecuteAsync(CancellationToken.None, components);

    public async Task ExecuteAsync(CancellationToken token, params string[] components)
    {
        var services = new Dictionary<string, IStartupComponent>();
        var actions = new Dictionary<string, IStartupComponent>();

        CreatedDependencyOrder(components, services, actions, new HashSet<string>());

        if (!await ExecuteComponentsAsync(services.Values, token) ||
            !await ExecuteComponentsAsync(actions.Values, token))
        {
            return;
        }

        LogExecutionSuccess();
    }

    public async Task ExecuteAllAsync() => await ExecuteAllAsync(CancellationToken.None);

    public async Task ExecuteAllAsync(CancellationToken token)
    {
        // A component can shut the application down, for example when the connection to the
        // Trackmania server cannot be established. The components that come after it expect
        // the state it was supposed to set up, so they must not run in that case.
        var completed = await ExecuteComponentsAsync(_components.Values, token, component =>
            _logger.LogWarning("Startup was aborted before the component {Name} could be executed", component.Name));

        if (completed)
        {
            LogExecutionSuccess();
        }
    }

    public void Dispose()
    {
        _services.Dispose();
    }
}
