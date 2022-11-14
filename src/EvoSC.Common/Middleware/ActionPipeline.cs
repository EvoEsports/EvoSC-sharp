using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Middleware;

namespace EvoSC.Common.Middleware;

public class ActionPipeline : IActionPipeline
{
    public List<Func<ActionDelegate, ActionDelegate>> _components = new();

    public IActionPipeline AddComponent(Func<ActionDelegate, ActionDelegate> middleware)
    {
        _components.Add(middleware);
        return this;
    }
    
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
