using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LocalRecordsModule.Controllers;

[Controller]
public class WidgetUpdateController(ILocalRecordsService localRecords) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        return Task.CompletedTask;
    }
}
