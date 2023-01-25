using EvoSC.Common.Interfaces.Middleware;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace EvoSC.Common.Middleware;

public class ActionPipelineManager : IActionPipelineManager
{
    private readonly ILogger<ActionPipelineManager> _logger;
    private readonly Dictionary<PipelineType, IActionPipeline> _mainPipelines = new();

    private readonly Dictionary<
        PipelineType,
        Dictionary<
            Guid,
            IActionPipeline
        >
    > _pipelines = new();

    public ActionPipelineManager(ILogger<ActionPipelineManager> logger)
    {
        _logger = logger;
        
        RegisterPipelineType(PipelineType.ControllerAction);
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

    public void UseMiddleware(PipelineType pipelineType, Func<ActionDelegate, ActionDelegate> middleware)
    {
        _mainPipelines[pipelineType].AddComponent(middleware);
    }

    public void UseMiddleware<TMiddleware>(PipelineType pipelineType, Container services) =>
        _mainPipelines[pipelineType].AddComponent<TMiddleware>(services);

    public void UseMiddleware(PipelineType pipelineType, Type middlewareType, Container services) =>
        _mainPipelines[pipelineType].AddComponent(middlewareType, services);

    public ActionDelegate BuildChain(PipelineType pipelineType, ActionDelegate chain)
    {
        if (_pipelines.Count == 0)
        {
            throw new InvalidOperationException("Cannot build chain with no pipelines.");
        }

        var pipelines = _pipelines[pipelineType].Values.ToArray();

        try
        {
            if (pipelines.Length > 0)
            {
                var last = pipelines.Last();

                chain = last.Build(chain);

                for (int i = pipelines.Length - 2; i >= 0; i--)
                {
                    chain = pipelines[i].Build(chain);
                }
            }

            return _mainPipelines[pipelineType].Build(chain);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to build action pipeline for type {Type}", pipelineType);
            throw;
        }
    }
}
