using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Remote.EventArgsModels;
using EvoSC.Common.Util;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdPlayerEventController : EvoScController<IEventControllerContext>
{
    private readonly IManialinkManager _manialink;
    private readonly IPlayerManagerService _playerManager;
    private readonly IMotdService _motdService;
    
    public MotdPlayerEventController(IManialinkManager manialink,
        IPlayerManagerService playerManager, IMotdService motdService)
    {
        _manialink = manialink;
        _playerManager = playerManager;
        _motdService = motdService;
    }

    [Subscribe(GbxRemoteEvent.PlayerChat)]
    public async Task OnPlayerChat(object sender, PlayerChatGbxEventArgs args)
        => await _motdService.ShowAsync();

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args)
        => await _motdService.ShowAsync();
}
