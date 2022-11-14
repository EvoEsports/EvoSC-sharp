using EvoSC.Common.Middleware;
using SimpleInjector;

namespace EvoSC.Common.Interfaces.Middleware;

public interface IActionPipelineManager
{
    public void AddPipeline(Guid guid, IActionPipeline pipeline);
    public void RemovePipeline(Guid guid);
    public void UseMiddleware(Func<ActionDelegate, ActionDelegate> middleware);
    public void UseMiddleware<TMiddleware>(Container container);
    public void UseMiddleware(Type middlewareType, Container container);
    public ActionDelegate BuildChain(ActionDelegate chain);
}
