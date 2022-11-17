using EvoSC.Common.Interfaces.Middleware;

namespace EvoSC.Common.Middleware;

public class ActionPipelineBuilder
{
    private readonly List<IActionPipeline> _pipelines = new();

    /// <summary>
    /// Add a new action pipeline to the builder.
    /// </summary>
    /// <param name="pipeline">Action pipeline.</param>
    /// <returns></returns>
    public ActionPipelineBuilder AddPipeline(IActionPipeline pipeline)
    {
        _pipelines.Add(pipeline);
        return this;
    }
    
    /// <summary>
    /// Create a new pipeline and configure it with an action.
    /// </summary>
    /// <param name="pipelineAction"></param>
    /// <returns></returns>
    public ActionPipelineBuilder AddPipeline(Action<IActionPipeline> pipelineAction)
    {
        var pipeline = new ActionPipeline();
        pipelineAction(pipeline);
        _pipelines.Add(pipeline);
        return this;
    }

    /// <summary>
    /// Build a combined middleware chain from the added action pipelines.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ActionDelegate Build()
    {
        if (_pipelines.Count == 0)
        {
            throw new InvalidOperationException("Cannot build chain with no pipelines.");
        }

        var last = _pipelines.Last();
        var chain = last.Build();
        
        for (int i = _pipelines.Count - 2; i >= 0; i--)
        {
            chain = _pipelines[i].Build(chain);
        }

        return chain;
    }
}
