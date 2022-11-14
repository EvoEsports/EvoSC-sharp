using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Middleware;

namespace EvoSC.Common.Interfaces.Middleware;

public interface IActionPipeline
{
    /// <summary>
    /// Add a middleware component to this pipeline.
    /// </summary>
    /// <param name="middleware">A function delegate that represents the middleware.</param>
    public IActionPipeline AddComponent(Func<ActionDelegate, ActionDelegate> middleware);
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
