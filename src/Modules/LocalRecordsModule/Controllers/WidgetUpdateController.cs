using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;
using EvoSC.Modules.Official.PlayerRecords.Events;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.LocalRecordsModule.Controllers;

[Controller]
public class WidgetUpdateController(ILocalRecordsService localRecords, IPlayerManagerService playerManagerService) : EvoScController<IEventControllerContext>
{
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        var player = await playerManagerService.GetOnlinePlayerAsync(PlayerUtils.ConvertLoginToAccountId(args.Login));
        await localRecords.ShowWidgetAsync(player);
    }

    [Subscribe(GbxRemoteEvent.BeginMap)]
    public Task OnBeginMapAsync(object sender, MapGbxEventArgs args) => localRecords.ShowWidgetToAllAsync();

    [Subscribe(PlayerRecordsEvent.PbRecord)]
    public Task OnPbAsync(object sender, PbRecordUpdateEventArgs args) => localRecords.UpdatePbAsync(args.Record);
}
