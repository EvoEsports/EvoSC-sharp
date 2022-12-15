using System.Threading.Tasks;
using EvoSC.Common.Interfaces.Middleware;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.ExampleModule;

[Module(IsInternal = true)]
public class ExampleModule : EvoScModule, IToggleable
{
    public Task Enable()
    {
        return Task.CompletedTask;
    }

    public Task Disable()
    {
        return Task.CompletedTask;
    }
}
