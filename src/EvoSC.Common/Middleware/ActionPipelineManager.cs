using EvoSC.Common.Interfaces.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Middleware;

public class ActionPipelineManager : IActionPipelineManager
{
    private readonly IActionPipeline _mainPipeline = new ActionPipeline();
    private readonly Dictionary<Guid, IActionPipeline> _pipelines = new();

    public void AddPipeline(Guid guid, IActionPipeline pipeline) => _pipelines[guid] = pipeline;

    public void RemovePipeline(Guid guid) => _pipelines.Remove(guid);

    public void UseMiddleware(Func<ActionDelegate, ActionDelegate> middleware) =>
        _mainPipeline.AddComponent(middleware);

    public void UseMiddleware<TMiddleware>(Container services) =>
        _mainPipeline.AddComponent<TMiddleware>(services);

    public void UseMiddleware(Type middlewareType, Container services) =>
        _mainPipeline.AddComponent(middlewareType, services);

    public ActionDelegate BuildChain(ActionDelegate chain)
    {
        if (_pipelines.Count == 0)
        {
            throw new InvalidOperationException("Cannot build chain with no pipelines.");
        }

        var pipelines = _pipelines.Values.ToArray();
        var last = pipelines.Last();
        chain = last.Build(chain);
        
        for (int i = _pipelines.Count - 2; i >= 0; i--)
        {
            chain = pipelines[i].Build(chain);
        }

        return _mainPipeline.Build(chain);
    }
}
