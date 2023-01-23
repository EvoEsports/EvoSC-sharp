using EvoSC.Common.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Middleware;

public interface IActionPipeline
{
    /// <summary>
    /// Add a middleware component to this pipeline.
    /// </summary>
    /// <param name="middleware">A function delegate that represents the middleware.</param>
    public IActionPipeline AddComponent(Func<ActionDelegate, ActionDelegate> middleware);
    
    /// <summary>
    /// Add a new middleware component to this pipeline as a middleware class.
    /// </summary>
    /// <param name="services">Service container for the ctor DI.</param>
    /// <typeparam name="TMiddleware">A properly defined middleware class.</typeparam>
    /// <returns></returns>
    public IActionPipeline AddComponent<TMiddleware>(Container services);
    
    /// <summary>
    /// Add a new middleware component to this pipeline as a middleware class.
    /// </summary>
    /// <param name="middlewareType">A properly defined middleware class.</param>
    /// <param name="services">Service container for the ctor DI.</param>
    /// <returns></returns>
    public IActionPipeline AddComponent(Type middlewareType, Container services);
    
    /// <summary>
    /// Build the pipeline chain and return a callable function to execute it.
    /// </summary>
    /// <param name="chain">A previous chain to build upon.</param>
    /// <returns></returns>
    public ActionDelegate Build(ActionDelegate chain);
    
    /// <summary>
    /// Build the pipeline chain and return a callable function to execute it.
    /// </summary>
    /// <returns></returns>
    public ActionDelegate Build();
}
