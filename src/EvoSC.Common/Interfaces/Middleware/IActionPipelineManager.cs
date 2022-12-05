using EvoSC.Common.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Middleware;

public interface IActionPipelineManager
{
    /// <summary>
    /// Add a pipeline to the manager assigned to a given GUID.
    /// </summary>
    /// <param name="pipelineType">The type of the pipeline to add to.</param>
    /// <param name="guid">GUID to assign the pipeline to. Must be unique.</param>
    /// <param name="pipeline">The pipeline to add.</param>
    public void AddPipeline(PipelineType pipelineType, Guid guid, IActionPipeline pipeline);

    /// <summary>
    /// Remove the pipeline for a given GUID.
    /// </summary>
    /// <param name="pipelineType">The type of pipeline to remove from.</param>
    /// <param name="guid">The GUID for the pipeline to remove.</param>
    public void RemovePipeline(PipelineType pipelineType, Guid guid);

    /// <summary>
    /// Add a new middleware to the main/core pipeline, which is executed first.
    /// </summary>
    /// <param name="pipelineType">The pipeline type to add the middleware to.</param>
    /// <param name="middleware">The middleware callback chain method.</param>
    public void UseMiddleware(PipelineType pipelineType, Func<ActionDelegate, ActionDelegate> middleware);
    
    /// <summary>
    /// Add a new middleware to the main/core pipeline, which is executed first.
    /// </summary>
    /// <param name="pipelineType">The pipeline type to add the middleware to.</param>
    /// <param name="services">Services for the ctor DI.</param>
    /// <typeparam name="TMiddleware">Type of the middleware class.</typeparam>
    public void UseMiddleware<TMiddleware>(PipelineType pipelineType, Container services);

    /// <summary>
    /// Add a new middleware to the main/core pipeline, which is executed first.
    /// </summary>
    /// <param name="pipelineType">The pipeline type to add the middleware to.</param>
    /// <param name="middlewareType">Type of the middleware class.</param>
    /// <param name="services">Services for the ctor DI.</param>
    public void UseMiddleware(PipelineType pipelineType, Type middlewareType, Container services);

    /// <summary>
    /// Combine all pipelines and build the whole action chain.
    /// </summary>
    /// <param name="pipelineType">The pipeline type to build the chain for.</param>
    /// <param name="chain">Chain to add to the built chain. Will be executed last.</param>
    /// <returns></returns>
    public ActionDelegate BuildChain(PipelineType pipelineType, ActionDelegate chain);
}
