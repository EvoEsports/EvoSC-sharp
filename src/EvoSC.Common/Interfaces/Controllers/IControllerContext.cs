using EvoSC.Common.Remote;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerContext
{
    public IServiceScope ServiceScope { get; }
    public IServerClient Server { get; }

    public void SetScope(IServiceScope scope);
}
