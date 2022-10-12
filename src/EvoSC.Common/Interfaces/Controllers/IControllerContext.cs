using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Common.Interfaces.Controllers;

public interface IControllerContext
{
    public IServiceScope ServiceScope { get; }

    public void SetScope(IServiceScope scope);
}
