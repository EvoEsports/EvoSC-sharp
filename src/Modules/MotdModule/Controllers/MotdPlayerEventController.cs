using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdPlayerEventController : EvoScController<IEventControllerContext>
{
    private readonly IManialinkManager _manialink;
    private readonly IPlayerManagerService _playerManager;
    private readonly MotdManialinkController _manialinkController;
    
    public MotdPlayerEventController(IManialinkManager manialink,IPlayerManagerService playerManager)
    {
        _manialink = manialink;
        _playerManager = playerManager;
        _manialinkController = new MotdManialinkController(manialink);
    }
    
    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task ShowMotdAsync(object sender, PlayerConnectGbxEventArgs args)
    {
        var accountId = PlayerUtils.ConvertLoginToAccountId(args.Login);
        var player = await _playerManager.GetPlayerAsync(accountId);
        
        if(player is not null)
            await _manialinkController.ShowMotdAsync(player);
    }
}
