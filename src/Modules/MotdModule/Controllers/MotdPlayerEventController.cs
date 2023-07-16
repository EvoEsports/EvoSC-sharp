using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Remote;
using EvoSC.Common.Util;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdPlayerEventController : EvoScController<IEventControllerContext>
{
    private readonly IPlayerManagerService _playerManager;
    private readonly IMotdService _motdService;
    private readonly IMotdRepository _motdRepository;
    
    public MotdPlayerEventController(IPlayerManagerService playerManager, IMotdService motdService, IMotdRepository motdRepository)
    {
        _playerManager = playerManager;
        _motdService = motdService;
        _motdRepository = motdRepository;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
        => await ShowAsync(args.Login);

    private async Task ShowAsync(string login)
    {
        await _motdService.ShowAsync(login, false);
    }
}
