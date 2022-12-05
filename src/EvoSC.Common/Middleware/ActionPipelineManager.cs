using EvoSC.Common.Interfaces.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Middleware;

public class ActionPipelineManager : IActionPipelineManager
{
    private readonly Dictionary<PipelineType, IActionPipeline> _mainPipelines = new();

    private readonly Dictionary<
        PipelineType,
        Dictionary<
            Guid,
            IActionPipeline
        >
    > _pipelines = new();

    public ActionPipelineManager()
    {
        RegisterPipelineType(PipelineType.Action);
        RegisterPipelineType(PipelineType.ChatRouter);
    }

    private void RegisterPipelineType(PipelineType type)
    {
        _mainPipelines[type] = new ActionPipeline();
        _pipelines[type] = new Dictionary<Guid, IActionPipeline>();
    }
    
    public void AddPipeline(PipelineType pipelineType, Guid guid, IActionPipeline pipeline)
    {
        _pipelines[pipelineType][guid] = pipeline;
    }

    public void RemovePipeline(PipelineType pipelineType, Guid guid) => _pipelines[pipelineType].Remove(guid);

    public void UseMiddleware<TContext>(PipelineType pipelineType,
        Func<ActionDelegate, ActionDelegate> middleware)
        where TContext : IPipelineContext
    {
        _mainPipelines[pipelineType].AddComponent(middleware);
    }

    public void UseMiddleware<TMiddleware>(PipelineType pipelineType, Container services) =>
        _mainPipelines[pipelineType].AddComponent<TMiddleware>(services);

    public void UseMiddleware(PipelineType pipelineType, Type middlewareType, Container services) =>
        _mainPipelines[pipelineType].AddComponent(middlewareType, services);

    public ActionDelegate BuildChain<TContext>(PipelineType pipelineType, ActionDelegate chain)
        where TContext : IPipelineContext
    {
        if (_pipelines.Count == 0)
        {
            throw new InvalidOperationException("Cannot build chain with no pipelines.");
        }

        var pipelines = _pipelines[pipelineType].Values.ToArray();
        var last = pipelines.Last();

        chain = last.Build(chain);
        
        for (int i = _pipelines.Count - 2; i >= 0; i--)
        {
            chain = pipelines[i].Build(chain);
        }

        return _mainPipelines[pipelineType].Build(chain);
    }

    /* private readonly IActionPipeline _mainPipeline = new ActionPipeline();
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
    } */
}
