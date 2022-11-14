using EvoSC.Common.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Middleware;

public interface IActionPipelineManager
{
    /// <summary>
    /// Add a pipeline to the manager assigned to a given GUID.
    /// </summary>
    /// <param name="guid">GUID to assign the pipeline to. Must be unique.</param>
    /// <param name="pipeline">The pipeline to add.</param>
    public void AddPipeline(Guid guid, IActionPipeline pipeline);
    /// <summary>
    /// Remove the pipeline for a given GUID.
    /// </summary>
    /// <param name="guid">The GUID for the pipeline to remove.</param>
    public void RemovePipeline(Guid guid);
    /// <summary>
    /// Add a new middleware to the main/core pipeline, which is executed first.
    /// </summary>
    /// <param name="middleware">Middleware function to add.</param>
    public void UseMiddleware(Func<ActionDelegate, ActionDelegate> middleware);
    /// <summary>
    /// Add a new middleware class to the main/core pipeline, which is executed first.
    /// </summary>
    /// <param name="services">Services for the ctor DI.</param>
    /// <typeparam name="TMiddleware">A properly defined middleware class.</typeparam>
    public void UseMiddleware<TMiddleware>(Container services);
    /// <summary>
    /// Add a new middleware to the main/core pipeline, which is executed first.
    /// </summary>
    /// <param name="middlewareType">A properly defined middleware class.</param>
    /// <param name="services">Services for the ctor DI.</param>
    public void UseMiddleware(Type middlewareType, Container services);
    /// <summary>
    /// Combine all pipelines and build the whole action chain.
    /// </summary>
    /// <param name="chain">Chain to add to the built chain. Will be executed last.</param>
    /// <returns>A callable function to execute the pipeline chain.</returns>
    public ActionDelegate BuildChain(ActionDelegate chain);
}
