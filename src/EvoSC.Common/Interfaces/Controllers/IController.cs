namespace EvoSC.Common.Interfaces.Controllers;

public interface IController : IDisposable
{
    public IControllerContext Context { get; }
    
    public void SetContext(IControllerContext context);
}
