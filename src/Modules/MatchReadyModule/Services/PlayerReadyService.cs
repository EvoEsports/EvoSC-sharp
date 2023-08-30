using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.MatchReadyModule.Interfaces;

namespace EvoSC.Modules.Official.MatchReadyModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class PlayerReadyService : IPlayerReadyService
{
    private readonly IPlayerReadyTrackerService _playerReadyTrackerService;
    private readonly IManialinkManager _manialinks;
    private readonly IServerClient _server;
    
    public PlayerReadyService(IPlayerReadyTrackerService playerReadyTrackerService, IManialinkManager manialinks, IServerClient server)
    {
        _playerReadyTrackerService = playerReadyTrackerService;
        _manialinks = manialinks;
        _server = server;
    }
    
    public async Task SetPlayerReadyStatusAsync(IPlayer player, bool isReady)
    {
        await _playerReadyTrackerService.SetIsReadyAsync(player, isReady);

        if (isReady)
        {
            await _server.InfoMessageAsync($"$<{player.NickName}$> is ready.");
        }
        else
        {
            await _server.InfoMessageAsync($"$<{player.NickName}$> is no longer ready.");
        }
        
        await _manialinks.SendManialinkAsync(player, "MatchReadyModule.UpdateWidget", new
        {
            PlayerCount = _playerReadyTrackerService.ReadyPlayers.Count(),
            IsReady = isReady
        });
    }

    public Task ResetReadyWidgetAsync()
    {
        _playerReadyTrackerService.Reset();
        return Task.CompletedTask;
    }
}
