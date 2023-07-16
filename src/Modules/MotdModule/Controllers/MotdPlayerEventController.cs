using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Events.Attributes;
using EvoSC.Common.Interfaces.Controllers;
using EvoSC.Common.Remote;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.MotdModule.Controllers;

[Controller]
public class MotdPlayerEventController : EvoScController<IEventControllerContext>
{
    private readonly IMotdService _motdService;
    
    public MotdPlayerEventController(IMotdService motdService)
    {
        _motdService = motdService;
    }

    [Subscribe(GbxRemoteEvent.PlayerConnect)]
    public async Task OnPlayerConnectAsync(object sender, PlayerConnectGbxEventArgs args)
        => await ShowAsync(args.Login);

    private async Task ShowAsync(string login)
    {
        await _motdService.ShowAsync(login, false);
    }
}
