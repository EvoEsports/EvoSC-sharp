using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Controllers;

[Controller]
public class AdminCpComController : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.Echo)]
    public async Task OnEcho(object sender, EchoGbxEventArgs args)
    {
        
    }
}
