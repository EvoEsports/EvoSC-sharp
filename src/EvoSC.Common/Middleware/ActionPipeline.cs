using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Util;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;

namespace EvoSC.Common.Middleware;

public class ActionPipeline : IActionPipeline
{
    public List<Func<ActionDelegate, ActionDelegate>> _components = new();

    public IActionPipeline AddComponent(Func<ActionDelegate, ActionDelegate> middleware)
    {
        _components.Add(middleware);
        return this;
    }

    public IActionPipeline AddComponent<TMiddleware>(Container services) => AddComponent(typeof(TMiddleware), services);

    public IActionPipeline AddComponent(Type middlewareType, Container services) => AddComponent(next =>
    {
        var args = new object[] {next};
        var instance = ActivatorUtilities.CreateInstance(services, middlewareType, args);
        var method = instance.GetInstanceMethod("ExecuteAsync");

        if (method == null)
        {
            throw new InvalidOperationException("Middleware must include method 'ExecuteAsync'.");
        }

        return context =>
        {
            var invokeArgs = new object[] {context};
            var task = (Task)method.Invoke(instance, invokeArgs);

            return task;
        };
    });

    public ActionDelegate Build(ActionDelegate chain)
    {
        // execute in reverse, otherwise last added will be executed first
        // but we want first to last order rather than last to first
        for (var i = _components.Count - 1; i >= 0; i--)
        {
            chain = _components[i](chain);
        }

        return chain;
    }

    public ActionDelegate Build()
    {
        ActionDelegate chain = context =>
        {
            return Task.CompletedTask;
        };

        return Build(chain);
    }
}
