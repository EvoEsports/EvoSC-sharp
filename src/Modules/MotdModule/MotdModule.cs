using EvoSC.Common.Interfaces.Services;
using EvoSC.Modules.Attributes;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.MotdModule.Interfaces;
using EvoSC.Modules.Official.MotdModule.Services;

namespace EvoSC.Modules.Official.MotdModule;

[Module(IsInternal = true)]
public class MotdModule : EvoScModule, IToggleable
{
    private readonly IMotdService _motdService;
    private readonly IPlayerManagerService _playerManager;
    
    public MotdModule(IMotdService motdService, IPlayerManagerService playerManager)
    {
        _motdService = motdService;
        _playerManager = playerManager;
    }
    
    public async Task EnableAsync()
    {
        var player = (await _playerManager.GetOnlinePlayersAsync()).FirstOrDefault();
        if(player is not null)
            await _motdService.ShowAsync(player);
    }

    public async Task DisableAsync()
    {
        
    }
}
