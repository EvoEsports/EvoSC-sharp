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
    private readonly IMotdRepository _motdRepository;
    
    public MotdPlayerEventController(IManialinkManager manialink,
        IPlayerManagerService playerManager, IMotdService motdService, IMotdRepository motdRepository)
    {
        _manialink = manialink;
        _playerManager = playerManager;
        _motdService = motdService;
        _motdRepository = motdRepository;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnect(object sender, PlayerConnectGbxEventArgs args)
        => await ShowAsync(args.Login);

    private async Task ShowAsync(string login)
    {
        var player = await _playerManager.GetPlayerAsync(PlayerUtils.ConvertLoginToAccountId(login));
        if (player is null)
            return;
        
        var playerEntry = await _motdRepository.GetEntryAsync(player);
        if (playerEntry is not null && playerEntry.Hidden)
            return;
        
        await _motdService.ShowAsync(player);
    }
}
